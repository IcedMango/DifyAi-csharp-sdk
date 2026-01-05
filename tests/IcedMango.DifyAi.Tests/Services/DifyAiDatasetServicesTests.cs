namespace IcedMango.DifyAi.Tests.Services;

/// <summary>
/// Unit tests for DifyAiDatasetServices
/// </summary>
public class DifyAiDatasetServicesTests
{
    private readonly Mock<IRequestExtension> _mockRequestExtension;
    private readonly DifyAiDatasetServices _sut;

    public DifyAiDatasetServicesTests()
    {
        _mockRequestExtension = new Mock<IRequestExtension>();
        _sut = new DifyAiDatasetServices(_mockRequestExtension.Object);
    }

    #region Dataset Operations

    [Fact]
    public async Task CreateDatasetAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateDatasetResDto>(
                "datasets",
                It.IsAny<DifyCreateDatasetParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyCreateDatasetResDto>
            {
                Success = true,
                Data = new DifyCreateDatasetResDto { Id = TestConstants.TestDatasetId }
            });

        var paramDto = new DifyCreateDatasetParamDto
        {
            Name = "Test Dataset"
        };

        // Act
        var result = await _sut.CreateDatasetAsync(paramDto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data?.Id.Should().Be(TestConstants.TestDatasetId);
    }

    [Fact]
    public async Task GetDatasetListAsync_WithPageAndLimit_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>> { Success = true });

        // Act
        await _sut.GetDatasetListAsync(1, 20);

        // Assert
        capturedUrl.Should().Be("datasets?page=1&limit=20");
    }

    [Fact]
    public async Task GetDatasetListAsync_WithExtendedParams_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestResDto<List<DifyGetDatasetListResDto>>> { Success = true });

        var paramDto = new DifyGetDatasetListParamDto
        {
            Page = 1,
            Limit = 10,
            Keyword = "test",
            TagIds = new List<string> { "tag1", "tag2" },
            IncludeAll = true
        };

        // Act
        await _sut.GetDatasetListAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain("page=1");
        capturedUrl.Should().Contain("limit=10");
        capturedUrl.Should().Contain("keyword=test");
        capturedUrl.Should().Contain("tag_ids=tag1,tag2");
        capturedUrl.Should().Contain("include_all=true");
    }

    [Fact]
    public async Task GetDatasetDetailAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyDatasetDetailResDto>(
                $"datasets/{TestConstants.TestDatasetId}",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyDatasetDetailResDto> { Success = true });

        // Act
        await _sut.GetDatasetDetailAsync(TestConstants.TestDatasetId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpGet<DifyDatasetDetailResDto>(
                $"datasets/{TestConstants.TestDatasetId}",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateDatasetAsync_ShouldCallHttpPatch()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPatch<DifyDatasetDetailResDto>(
                $"datasets/{TestConstants.TestDatasetId}",
                It.IsAny<DifyUpdateDatasetParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyDatasetDetailResDto> { Success = true });

        var paramDto = new DifyUpdateDatasetParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            Name = "Updated Name"
        };

        // Act
        await _sut.UpdateDatasetAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPatch<DifyDatasetDetailResDto>(
                $"datasets/{TestConstants.TestDatasetId}",
                It.IsAny<DifyUpdateDatasetParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteDatasetAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpDelete<DifyBaseRequestResDto>(
                $"datasets/{TestConstants.TestDatasetId}",
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestResDto> { Success = true });

        // Act
        await _sut.DeleteDatasetAsync(TestConstants.TestDatasetId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpDelete<DifyBaseRequestResDto>(
                $"datasets/{TestConstants.TestDatasetId}",
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Document Operations

    [Fact]
    public async Task CreateDocumentByTextAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateModifyDocumentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/document/create-by-text",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyCreateModifyDocumentResDto>
            {
                Success = true,
                Data = new DifyCreateModifyDocumentResDto
                {
                    Document = new DifyDatasetDocument { Id = TestConstants.TestDocumentId }
                }
            });

        var paramDto = new DifyCreateDocumentByTextParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            Name = "Test Document",
            Text = "Test content"
        };

        // Act
        var result = await _sut.CreateDocumentByTextAsync(paramDto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task CreateDocumentByTextAsync_WithHighQualityIndex_ShouldSetIndexingTechnique()
    {
        // Arrange
        DifyBaseRequestParamDto capturedParam = null;
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateModifyDocumentResDto>(
                It.IsAny<string>(),
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, DifyBaseRequestParamDto, CancellationToken>((url, param, ct) => capturedParam = param)
            .ReturnsAsync(new DifyApiResult<DifyCreateModifyDocumentResDto> { Success = true });

        var paramDto = new DifyCreateDocumentByTextParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            Name = "Test Document",
            Text = "Test content",
            EnableHighQualityIndex = true
        };

        // Act
        await _sut.CreateDocumentByTextAsync(paramDto);

        // Assert
        capturedParam.Should().NotBeNull();
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(capturedParam);
        json.Should().Contain("high_quality");
    }

    [Fact]
    public async Task UpdateDocumentByTextAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateModifyDocumentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/update-by-text",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyCreateModifyDocumentResDto> { Success = true });

        var paramDto = new DifyUpdateDocumentByTextParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            DocumentId = TestConstants.TestDocumentId,
            Name = "Updated Document",
            Text = "Updated content"
        };

        // Act
        await _sut.UpdateDocumentByTextAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyCreateModifyDocumentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/update-by-text",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateDocumentByFileAsync_ShouldCallUploadEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.PostUploadDocumentAsync<DifyCreateModifyDocumentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/document/create-by-file",
                It.IsAny<DifyBaseFileRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyCreateModifyDocumentResDto> { Success = true });

        var paramDto = new DifyCreateDocumentByFileParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            FilePath = "/path/to/file.txt"
        };

        // Act
        await _sut.CreateDocumentByFileAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.PostUploadDocumentAsync<DifyCreateModifyDocumentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/document/create-by-file",
                It.IsAny<DifyBaseFileRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteDocumentAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpDelete<DifyBaseRequestResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}",
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestResDto> { Success = true });

        // Act
        await _sut.DeleteDocumentAsync(TestConstants.TestDatasetId, TestConstants.TestDocumentId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpDelete<DifyBaseRequestResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}",
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDocumentDetailAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyDocumentDetailResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyDocumentDetailResDto> { Success = true });

        // Act
        await _sut.GetDocumentDetailAsync(TestConstants.TestDatasetId, TestConstants.TestDocumentId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpGet<DifyDocumentDetailResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyRetrieveResDto>(
                $"datasets/{TestConstants.TestDatasetId}/retrieve",
                It.IsAny<DifyRetrieveParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyRetrieveResDto> { Success = true });

        var paramDto = new DifyRetrieveParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            Query = "test query"
        };

        // Act
        await _sut.RetrieveAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyRetrieveResDto>(
                $"datasets/{TestConstants.TestDatasetId}/retrieve",
                It.IsAny<DifyRetrieveParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDocumentEmbeddingAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var batch = "batch-123";
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{batch}/indexing-status",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>> { Success = true });

        // Act
        await _sut.GetDocumentEmbeddingAsync(TestConstants.TestDatasetId, batch);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpGet<DifyBaseRequestListResDto<List<DifyGetDocumentEmbeddingResDto>>>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{batch}/indexing-status",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDocumentListAsync_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyBaseRequestResDto<List<DifyGetDocumentListResDto>>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestResDto<List<DifyGetDocumentListResDto>>> { Success = true });

        var paramDto = new DifyGetDocumentListParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            Keyword = "test",
            Page = 1,
            Limit = 20
        };

        // Act
        await _sut.GetDocumentListAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain($"datasets/{TestConstants.TestDatasetId}/documents");
        capturedUrl.Should().Contain("keyword=test");
        capturedUrl.Should().Contain("page=1");
        capturedUrl.Should().Contain("limit=20");
    }

    #endregion

    #region Segment Operations

    [Fact]
    public async Task AddDocumentSegmentAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyDocumentSegmentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/segments",
                It.IsAny<DifyAddDocumentSegmentParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyDocumentSegmentResDto> { Success = true });

        var paramDto = new DifyAddDocumentSegmentParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            DocumentId = TestConstants.TestDocumentId
        };

        // Act
        await _sut.AddDocumentSegmentAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyDocumentSegmentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/segments",
                It.IsAny<DifyAddDocumentSegmentParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateDocumentSegmentAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifySingleDocumentSegmentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/segments/{TestConstants.TestSegmentId}",
                It.IsAny<DifyUpdateDocumentSegmentParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifySingleDocumentSegmentResDto> { Success = true });

        var paramDto = new DifyUpdateDocumentSegmentParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            DocumentId = TestConstants.TestDocumentId,
            SegmentId = TestConstants.TestSegmentId
        };

        // Act
        await _sut.UpdateDocumentSegmentAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifySingleDocumentSegmentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/segments/{TestConstants.TestSegmentId}",
                It.IsAny<DifyUpdateDocumentSegmentParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetDocumentSegmentAsync_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyDocumentSegmentResDto>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyDocumentSegmentResDto> { Success = true });

        var paramDto = new DifyGetDocumentSegmentParamDto
        {
            DatasetId = TestConstants.TestDatasetId,
            DocumentId = TestConstants.TestDocumentId,
            Keyword = "test",
            Status = "completed",
            Page = 1,
            Limit = 20
        };

        // Act
        await _sut.GetDocumentSegmentAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain($"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/segments");
        capturedUrl.Should().Contain("keyword=test");
        capturedUrl.Should().Contain("status=completed");
    }

    [Fact]
    public async Task DeleteDocumentSegmentAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpDelete<DifyDocumentSegmentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/segments/{TestConstants.TestSegmentId}",
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyDocumentSegmentResDto> { Success = true });

        // Act
        await _sut.DeleteDocumentSegmentAsync(
            TestConstants.TestDatasetId,
            TestConstants.TestDocumentId,
            TestConstants.TestSegmentId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpDelete<DifyDocumentSegmentResDto>(
                $"datasets/{TestConstants.TestDatasetId}/documents/{TestConstants.TestDocumentId}/segments/{TestConstants.TestSegmentId}",
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
