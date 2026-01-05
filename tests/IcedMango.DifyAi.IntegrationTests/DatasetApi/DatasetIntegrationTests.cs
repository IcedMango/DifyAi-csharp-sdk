using Xunit.Abstractions;

namespace IcedMango.DifyAi.IntegrationTests.DatasetApi;

/// <summary>
/// Integration tests for Dataset API using real Dify API.
/// These tests require valid API keys configured in appsettings.json or environment variables.
/// </summary>
[Collection("IntegrationTests")]
public class DatasetIntegrationTests : IDisposable
{
    private readonly IntegrationTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public DatasetIntegrationTests(IntegrationTestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        // Reset service provider to use the current test's output helper
        _fixture.ResetServiceProvider();
    }

    #region Configuration Tests

    [Fact]
    [Trait("Category", "Integration")]
    public void TestConfiguration_ShouldLoadCorrectly()
    {
        // This test always runs to verify test infrastructure
        _fixture.Configuration.Should().NotBeNull();
        _fixture.Configuration.DatasetName.Should().NotBeNullOrEmpty();
    }

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task InvalidApiKey_ShouldReturnError()
    {
        // Skip if network issues occur (SSL/connection problems)
        // This test uses an invalid key to verify error handling
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        // Use the same BaseUrl as the valid configuration to avoid SSL issues
        services.AddDifyAi(register =>
        {
            register.RegisterDataset("InvalidDataset", "dataset-invalid-key-12345", _fixture.Configuration.DatasetBaseUrl);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();
        var datasetService = factory.GetDatasetService("InvalidDataset");

        // Act
        DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>? result = null;
        try
        {
            result = await datasetService.GetDatasetListAsync(1, 10);
        }
        catch (HttpRequestException ex)
        {
            Skip.If(true, $"Network error occurred: {ex.Message}");
        }

        // Assert - Should return error result (unauthorized or similar)
        result.Should().NotBeNull();
        result!.Success.Should().BeFalse();
    }

    #endregion

    #region Dataset List Test (Independent)

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task GetDatasetListAsync_ShouldReturnList()
    {
        // Skip if not configured
        Skip.If(!_fixture.Configuration.IsDatasetConfigured, _fixture.Configuration.DatasetSkipReason);

        // Arrange
        var datasetService = _fixture.GetDatasetService(_output);

        // Act
        var result = await datasetService.GetDatasetListAsync(1, 20);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue($"API returned error: {result.Message}");
    }

    #endregion

    #region Dataset Full Lifecycle Test (Ordered Steps in Single Test)

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task Dataset_FullLifecycle_CreateUpdateDelete()
    {
        Skip.If(!_fixture.Configuration.IsDatasetConfigured, _fixture.Configuration.DatasetSkipReason);

        var datasetService = _fixture.GetDatasetService(_output);
        string datasetId = null;

        try
        {
            // Step 1: Create Dataset
            var testName = $"IntegrationTest_{DateTime.Now:yyyyMMddHHmmss}";
            var createResult = await datasetService.CreateDatasetAsync(new DifyCreateDatasetParamDto
            {
                Name = testName
            });

            createResult.Should().NotBeNull();
            createResult.Success.Should().BeTrue($"Create failed: {createResult.Message}");
            createResult.Data.Should().NotBeNull();
            createResult.Data!.Name.Should().Be(testName);
            datasetId = createResult.Data.Id;

            // Step 2: Get Dataset Detail
            var detailResult = await datasetService.GetDatasetDetailAsync(datasetId);
            detailResult.Should().NotBeNull();
            detailResult.Success.Should().BeTrue($"Get detail failed: {detailResult.Message}");
            detailResult.Data.Should().NotBeNull();
            detailResult.Data!.Id.Should().Be(datasetId);

            // Step 3: Update Dataset
            var updatedDescription = "Updated by integration test";
            var updateResult = await datasetService.UpdateDatasetAsync(new DifyUpdateDatasetParamDto
            {
                DatasetId = datasetId,
                Description = updatedDescription
            });
            updateResult.Should().NotBeNull();
            updateResult.Success.Should().BeTrue($"Update failed: {updateResult.Message}");
            updateResult.Data.Should().NotBeNull();
            updateResult.Data!.Description.Should().Be(updatedDescription);

            // Step 4: Delete Dataset
            var deleteResult = await datasetService.DeleteDatasetAsync(datasetId);
            deleteResult.Should().NotBeNull();
            deleteResult.Success.Should().BeTrue($"Delete failed: {deleteResult.Message}");

            // Verify deletion
            var verifyResult = await datasetService.GetDatasetDetailAsync(datasetId);
            verifyResult.Success.Should().BeFalse("Dataset should be deleted");

            datasetId = null; // Clear to skip cleanup
        }
        finally
        {
            // Cleanup if test failed midway
            if (!string.IsNullOrEmpty(datasetId))
            {
                try { await datasetService.DeleteDatasetAsync(datasetId); }
                catch { /* Ignore cleanup errors */ }
            }
        }
    }

    #endregion

    #region Document Full Lifecycle Test (Ordered Steps in Single Test)

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task Document_FullLifecycle_CreateByText()
    {
        Skip.If(!_fixture.Configuration.IsDatasetConfigured, _fixture.Configuration.DatasetSkipReason);

        var datasetService = _fixture.GetDatasetService(_output);
        string datasetId = null;
        string documentId = null;

        try
        {
            // Step 1: Create Dataset
            var datasetResult = await datasetService.CreateDatasetAsync(new DifyCreateDatasetParamDto
            {
                Name = $"DocTest_{DateTime.Now:yyyyMMddHHmmss}"
            });
            datasetResult.Success.Should().BeTrue($"Create dataset failed: {datasetResult.Message}");
            datasetId = datasetResult.Data!.Id;

            // Step 2: Create Document by Text
            var docResult = await datasetService.CreateDocumentByTextAsync(new DifyCreateDocumentByTextParamDto
            {
                DatasetId = datasetId,
                Name = "Test Document",
                Text = "This is a test document content for integration testing. It contains multiple sentences to ensure proper segmentation.",
                IsAutomaticProcess = true,
                EnableHighQualityIndex = true
            });
            docResult.Should().NotBeNull();
            docResult.Success.Should().BeTrue($"Create document failed: {docResult.Message}");
            docResult.Data.Should().NotBeNull();
            docResult.Data!.Document.Should().NotBeNull();
            documentId = docResult.Data.Document!.Id;
            var batch = docResult.Data.Batch;

            // Step 3: Wait for document indexing (with timeout)
            var indexingCompleted = await WaitForDocumentIndexingAsync(datasetService, datasetId, batch, TimeSpan.FromSeconds(30));
            // Note: We continue even if indexing is not completed

            // Step 4: Get Document List
            var docListResult = await datasetService.GetDocumentListAsync(new DifyGetDocumentListParamDto
            {
                DatasetId = datasetId,
                Page = 1,
                Limit = 10
            });
            docListResult.Should().NotBeNull();
            docListResult.Success.Should().BeTrue($"Get document list failed: {docListResult.Message}");
            docListResult.Data.Should().NotBeNull();
            docListResult.Data!.Data.Should().NotBeNullOrEmpty();

            // Step 5: Get Document Detail
            var docDetailResult = await datasetService.GetDocumentDetailAsync(datasetId, documentId);
            docDetailResult.Should().NotBeNull();
            docDetailResult.Success.Should().BeTrue($"Get document detail failed: {docDetailResult.Message}");
            docDetailResult.Data.Should().NotBeNull();
            docDetailResult.Data!.Id.Should().Be(documentId);

            // Step 6: Delete Document
            var deleteDocResult = await datasetService.DeleteDocumentAsync(datasetId, documentId);
            deleteDocResult.Should().NotBeNull();
            deleteDocResult.Success.Should().BeTrue($"Delete document failed: {deleteDocResult.Message}");
            documentId = null;
        }
        finally
        {
            // Cleanup
            if (!string.IsNullOrEmpty(datasetId) && !string.IsNullOrEmpty(documentId))
            {
                try { await datasetService.DeleteDocumentAsync(datasetId, documentId); }
                catch { /* Ignore */ }
            }
            if (!string.IsNullOrEmpty(datasetId))
            {
                try { await datasetService.DeleteDatasetAsync(datasetId); }
                catch { /* Ignore */ }
            }
        }
    }

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task Document_FullLifecycle_CreateByFile()
    {
        Skip.If(!_fixture.Configuration.IsDatasetConfigured, _fixture.Configuration.DatasetSkipReason);

        var datasetService = _fixture.GetDatasetService(_output);
        string datasetId = null;
        string testFilePath = null;

        try
        {
            // Create test file
            testFilePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.txt");
            await File.WriteAllTextAsync(testFilePath, "This is test content for file upload integration test.\nLine 2\nLine 3");

            // Step 1: Create Dataset
            var datasetResult = await datasetService.CreateDatasetAsync(new DifyCreateDatasetParamDto
            {
                Name = $"FileUploadTest_{DateTime.Now:yyyyMMddHHmmss}"
            });
            datasetResult.Success.Should().BeTrue($"Create dataset failed: {datasetResult.Message}");
            datasetId = datasetResult.Data!.Id;

            // Step 2: Create Document by File
            var docResult = await datasetService.CreateDocumentByFileAsync(new DifyCreateDocumentByFileParamDto
            {
                DatasetId = datasetId,
                FilePath = testFilePath,
                IsAutomaticProcess = true,
                EnableHighQualityIndex = true
            });
            docResult.Should().NotBeNull();
            docResult.Success.Should().BeTrue($"Create document by file failed: {docResult.Message}");
            docResult.Data.Should().NotBeNull();
            docResult.Data!.Document.Should().NotBeNull();
        }
        finally
        {
            // Cleanup test file
            if (!string.IsNullOrEmpty(testFilePath) && File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
            // Cleanup dataset
            if (!string.IsNullOrEmpty(datasetId))
            {
                try { await datasetService.DeleteDatasetAsync(datasetId); }
                catch { /* Ignore */ }
            }
        }
    }

    #endregion

    #region Segment Full Lifecycle Test (Ordered Steps in Single Test)

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task Segment_FullLifecycle_AddGetUpdateDelete()
    {
        Skip.If(!_fixture.Configuration.IsDatasetConfigured, _fixture.Configuration.DatasetSkipReason);

        var datasetService = _fixture.GetDatasetService(_output);
        string datasetId = null;
        string documentId = null;

        try
        {
            // Step 1: Create Dataset
            var datasetResult = await datasetService.CreateDatasetAsync(new DifyCreateDatasetParamDto
            {
                Name = $"SegmentTest_{DateTime.Now:yyyyMMddHHmmss}"
            });
            datasetResult.Success.Should().BeTrue($"Create dataset failed: {datasetResult.Message}");
            datasetId = datasetResult.Data!.Id;

            // Step 2: Create Document
            var docResult = await datasetService.CreateDocumentByTextAsync(new DifyCreateDocumentByTextParamDto
            {
                DatasetId = datasetId,
                Name = "Segment Test Doc",
                Text = "Initial content for segment testing.",
                IsAutomaticProcess = true
            });
            docResult.Success.Should().BeTrue($"Create document failed: {docResult.Message}");
            documentId = docResult.Data!.Document!.Id;
            var batch = docResult.Data.Batch;

            // Step 3: Wait for indexing
            var indexingCompleted = await WaitForDocumentIndexingAsync(datasetService, datasetId, batch, TimeSpan.FromSeconds(30));
            Skip.If(!indexingCompleted, "Document indexing did not complete within timeout");

            // Step 4: Add Segment
            var addResult = await datasetService.AddDocumentSegmentAsync(new DifyAddDocumentSegmentParamDto
            {
                DatasetId = datasetId,
                DocumentId = documentId,
                Segments = new List<DifyAddDocumentSegment_Segment>
                {
                    new DifyAddDocumentSegment_Segment
                    {
                        Content = "This is a new segment content added by integration test.",
                        Answer = "This is the answer for Q&A mode.",
                        Keywords = new List<string> { "test", "segment", "integration" }
                    }
                }
            });

            // Skip if document not ready
            Skip.If(addResult.Message?.Contains("not completed") == true,
                "Document indexing not completed");

            addResult.Should().NotBeNull();
            addResult.Success.Should().BeTrue($"Add segment failed: {addResult.Message}");
            var segmentId = addResult.Data?.Data?.FirstOrDefault()?.Id;
            segmentId.Should().NotBeNullOrEmpty("Segment ID should be returned");

            // Step 5: Get Segments
            var getResult = await datasetService.GetDocumentSegmentAsync(new DifyGetDocumentSegmentParamDto
            {
                DatasetId = datasetId,
                DocumentId = documentId,
                Page = 1,
                Limit = 10
            });
            getResult.Should().NotBeNull();
            getResult.Success.Should().BeTrue($"Get segments failed: {getResult.Message}");

            // Step 6: Update Segment (if we have a segment ID)
            if (!string.IsNullOrEmpty(segmentId))
            {
                var updateResult = await datasetService.UpdateDocumentSegmentAsync(new DifyUpdateDocumentSegmentParamDto
                {
                    DatasetId = datasetId,
                    DocumentId = documentId,
                    SegmentId = segmentId,
                    Segment = new DifyUpdateDocumentSegment_Segment
                    {
                        Content = "Updated segment content.",
                        Answer = "Updated answer",
                        Keywords = new List<string> { "updated" }
                    }
                });
                updateResult.Should().NotBeNull();
                updateResult.Success.Should().BeTrue($"Update segment failed: {updateResult.Message}");
            }
        }
        finally
        {
            // Cleanup
            if (!string.IsNullOrEmpty(datasetId))
            {
                try { await datasetService.DeleteDatasetAsync(datasetId); }
                catch { /* Ignore */ }
            }
        }
    }

    #endregion

    #region Retrieve Test

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task RetrieveAsync_WithExistingDataset_ShouldWork()
    {
        Skip.If(!_fixture.Configuration.IsDatasetConfigured, _fixture.Configuration.DatasetSkipReason);

        // Arrange - Get existing dataset
        var datasetService = _fixture.GetDatasetService(_output);
        var datasetList = await datasetService.GetDatasetListAsync(1, 1);
        Skip.If(datasetList.Data?.Data == null || datasetList.Data.Data.Count == 0, "No datasets available");

        var datasetId = datasetList.Data!.Data![0].Id;

        // Act
        var result = await datasetService.RetrieveAsync(new DifyRetrieveParamDto
        {
            DatasetId = datasetId,
            Query = "test",
            TopK = 3
        });

        // Assert
        result.Should().NotBeNull();
        // Skip if server-side database error (embedding index missing)
        Skip.If(result.Message?.Contains("does not exist") == true, "Server-side database index issue - skipping");
        result.Success.Should().BeTrue($"API returned error: {result.Message}");
    }

    #endregion

    #region Helper Methods

    private static async Task<bool> WaitForDocumentIndexingAsync(
        IDifyAiDatasetServices datasetService,
        string datasetId,
        string batch,
        TimeSpan maxWaitTime)
    {
        var pollInterval = TimeSpan.FromSeconds(2);
        var startTime = DateTime.Now;

        while (DateTime.Now - startTime < maxWaitTime)
        {
            var statusResult = await datasetService.GetDocumentEmbeddingAsync(datasetId, batch);

            if (statusResult.Success == true && statusResult.Data?.Data?.Count > 0 &&
                statusResult.Data.Data[0].IndexingStatus == "completed")
            {
                return true;
            }

            await Task.Delay(pollInterval);
        }

        return false;
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    #endregion
}
