namespace IcedMango.DifyAi.Tests.Services;

/// <summary>
/// Unit tests for DifyAiChatServices
/// </summary>
public class DifyAiChatServicesTests
{
    private readonly Mock<IRequestExtension> _mockRequestExtension;
    private readonly DifyAiChatServices _sut;

    public DifyAiChatServicesTests()
    {
        _mockRequestExtension = new Mock<IRequestExtension>();
        _sut = new DifyAiChatServices(_mockRequestExtension.Object);
    }

    #region CreateChatCompletionBlockModeAsync

    [Fact]
    public async Task CreateChatCompletionBlockModeAsync_ShouldSetResponseModeToBlocking()
    {
        // Arrange
        var expectedResult = new DifyApiResult<DifyCreateChatCompletionResDto>
        {
            Success = true,
            Data = new DifyCreateChatCompletionResDto
            {
                MessageId = TestConstants.TestMessageId,
                Answer = "Hello!"
            }
        };

        DifyBaseRequestParamDto capturedParam = null;
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateChatCompletionResDto>(
                It.IsAny<string>(),
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, DifyBaseRequestParamDto, CancellationToken>((url, param, ct) =>
            {
                capturedParam = param;
            })
            .ReturnsAsync(expectedResult);

        var paramDto = new DifyCreateChatCompletionParamDto
        {
            Query = "Hello",
            User = TestConstants.TestUserId
        };

        // Act
        var result = await _sut.CreateChatCompletionBlockModeAsync(paramDto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        capturedParam.Should().NotBeNull();
        var chatParam = capturedParam as DifyCreateChatCompletionParamDto;
        chatParam.Should().NotBeNull();
        chatParam!.ResponseMode.Should().Be("blocking");
    }

    [Fact]
    public async Task CreateChatCompletionBlockModeAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateChatCompletionResDto>(
                "chat-messages",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyCreateChatCompletionResDto> { Success = true });

        var paramDto = new DifyCreateChatCompletionParamDto
        {
            Query = "Test",
            User = TestConstants.TestUserId
        };

        // Act
        await _sut.CreateChatCompletionBlockModeAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyCreateChatCompletionResDto>(
                "chat-messages",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateChatCompletionBlockModeAsync_ShouldReturnResponse()
    {
        // Arrange
        var expectedAnswer = "The answer is 42";
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateChatCompletionResDto>(
                It.IsAny<string>(),
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyCreateChatCompletionResDto>
            {
                Success = true,
                Data = new DifyCreateChatCompletionResDto
                {
                    MessageId = TestConstants.TestMessageId,
                    ConversationId = TestConstants.TestConversationId,
                    Answer = expectedAnswer
                }
            });

        var paramDto = new DifyCreateChatCompletionParamDto
        {
            Query = "What is the meaning of life?",
            User = TestConstants.TestUserId
        };

        // Act
        var result = await _sut.CreateChatCompletionBlockModeAsync(paramDto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Answer.Should().Be(expectedAnswer);
        result.Data.MessageId.Should().Be(TestConstants.TestMessageId);
    }

    #endregion

    #region StopChatCompletionAsync

    [Fact]
    public async Task StopChatCompletionAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyStopChatCompletionResDto>(
                $"chat-messages/{TestConstants.TestTaskId}/stop",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyStopChatCompletionResDto> { Success = true });

        var paramDto = new DifyStopChatCompletionParamDto
        {
            TaskId = TestConstants.TestTaskId,
            User = TestConstants.TestUserId
        };

        // Act
        await _sut.StopChatCompletionAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyStopChatCompletionResDto>(
                $"chat-messages/{TestConstants.TestTaskId}/stop",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region CreateChatCompletionStreamModeAsync

    [Fact]
    public async Task CreateChatCompletionStreamModeAsync_ShouldThrowNotImplementedException()
    {
        // Arrange
        var paramDto = new DifyCreateChatCompletionParamDto
        {
            Query = "Test",
            User = TestConstants.TestUserId
        };

        // Act
        var act = async () => await _sut.CreateChatCompletionStreamModeAsync(paramDto);

        // Assert
        await act.Should().ThrowAsync<NotImplementedException>();
    }

    #endregion

    #region CreateFeedbackAsync

    [Fact]
    public async Task CreateFeedbackAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyCreateFeedbackResDto>(
                $"messages/{TestConstants.TestMessageId}/feedbacks",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyCreateFeedbackResDto> { Success = true });

        var paramDto = new DifyCreateFeedbackParamDto
        {
            MessageId = TestConstants.TestMessageId,
            Rating = "like",
            User = TestConstants.TestUserId
        };

        // Act
        await _sut.CreateFeedbackAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyCreateFeedbackResDto>(
                $"messages/{TestConstants.TestMessageId}/feedbacks",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetSuggestionsAsync

    [Fact]
    public async Task GetSuggestionsAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyBaseRequestResDto<List<string>>>(
                $"messages/{TestConstants.TestMessageId}/suggested",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestResDto<List<string>>>
            {
                Success = true,
                Data = new DifyBaseRequestResDto<List<string>>
                {
                    Data = new List<string> { "Question 1?", "Question 2?" }
                }
            });

        // Act
        var result = await _sut.GetSuggestionsAsync(TestConstants.TestMessageId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data?.Data.Should().HaveCount(2);
    }

    #endregion

    #region GetConversationListAsync

    [Fact]
    public async Task GetConversationListAsync_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetConversationListResDto>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyGetConversationListResDto> { Success = true });

        var paramDto = new DifyGetConversationListParamDto
        {
            User = TestConstants.TestUserId,
            Limit = 10
        };

        // Act
        await _sut.GetConversationListAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain($"user={TestConstants.TestUserId}");
        capturedUrl.Should().Contain("limit=10");
    }

    [Fact]
    public async Task GetConversationListAsync_WithOptionalParams_ShouldIncludeThemInUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetConversationListResDto>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyGetConversationListResDto> { Success = true });

        var paramDto = new DifyGetConversationListParamDto
        {
            User = TestConstants.TestUserId,
            Limit = 10,
            LastId = "last-conv-id",
            Pinned = true
        };

        // Act
        await _sut.GetConversationListAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain("last_id=last-conv-id");
        capturedUrl.Should().Contain("pinned=True");
    }

    #endregion

    #region GetConversationHistoryMessageAsync

    [Fact]
    public async Task GetConversationHistoryMessageAsync_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetConversationHistoryMessageResDto>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyGetConversationHistoryMessageResDto> { Success = true });

        var paramDto = new DifyGetConversationHistoryMessageParamDto
        {
            ConversationId = TestConstants.TestConversationId,
            User = TestConstants.TestUserId,
            Limit = 20
        };

        // Act
        await _sut.GetConversationHistoryMessageAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain($"conversation_id={TestConstants.TestConversationId}");
        capturedUrl.Should().Contain($"user={TestConstants.TestUserId}");
        capturedUrl.Should().Contain("limit=20");
    }

    [Fact]
    public async Task GetConversationHistoryMessageAsync_WithFirstId_ShouldIncludeInUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetConversationHistoryMessageResDto>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyGetConversationHistoryMessageResDto> { Success = true });

        var paramDto = new DifyGetConversationHistoryMessageParamDto
        {
            ConversationId = TestConstants.TestConversationId,
            User = TestConstants.TestUserId,
            Limit = 20,
            FirstId = "first-msg-id"
        };

        // Act
        await _sut.GetConversationHistoryMessageAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain("first_id=first-msg-id");
    }

    #endregion

    #region DeleteConversationAsync

    [Fact]
    public async Task DeleteConversationAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpDelete<DifyBaseRequestResDto>(
                $"conversations/{TestConstants.TestConversationId}",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyBaseRequestResDto> { Success = true });

        var paramDto = new DifyDeleteConversationParamDto
        {
            ConversationId = TestConstants.TestConversationId,
            User = TestConstants.TestUserId
        };

        // Act
        await _sut.DeleteConversationAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpDelete<DifyBaseRequestResDto>(
                $"conversations/{TestConstants.TestConversationId}",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region RenameConversationAsync

    [Fact]
    public async Task RenameConversationAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyRenameConversationResDto>(
                $"conversations/{TestConstants.TestConversationId}/name",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyRenameConversationResDto> { Success = true });

        var paramDto = new DifyRenameConversationParamDto
        {
            ConversationId = TestConstants.TestConversationId,
            Name = "New Name",
            User = TestConstants.TestUserId
        };

        // Act
        await _sut.RenameConversationAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyRenameConversationResDto>(
                $"conversations/{TestConstants.TestConversationId}/name",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region AudioToTextAsync

    [Fact]
    public async Task AudioToTextAsync_WithSupportedFormat_ShouldCallEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.PostUploadFileAsync<DifyAudioToTextResDto>(
                "audio-to-text",
                It.IsAny<DifyBaseFileRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyAudioToTextResDto>
            {
                Success = true,
                Data = new DifyAudioToTextResDto { Text = "Transcribed text" }
            });

        var paramDto = new DifyAudioToTextParamDto
        {
            FilePath = "/path/to/audio.mp3",
            User = TestConstants.TestUserId
        };

        // Act
        var result = await _sut.AudioToTextAsync(paramDto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data?.Text.Should().Be("Transcribed text");
    }

    [Theory]
    [InlineData("mp3")]
    [InlineData("mp4")]
    [InlineData("mpeg")]
    [InlineData("mpga")]
    [InlineData("m4a")]
    [InlineData("wav")]
    [InlineData("webm")]
    public async Task AudioToTextAsync_WithValidFormats_ShouldNotThrow(string format)
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.PostUploadFileAsync<DifyAudioToTextResDto>(
                It.IsAny<string>(),
                It.IsAny<DifyBaseFileRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyAudioToTextResDto> { Success = true });

        var paramDto = new DifyAudioToTextParamDto
        {
            FilePath = $"/path/to/audio.{format}",
            User = TestConstants.TestUserId
        };

        // Act
        var act = async () => await _sut.AudioToTextAsync(paramDto);

        // Assert
        await act.Should().NotThrowAsync<DifyInvalidFileException>();
    }

    [Fact]
    public async Task AudioToTextAsync_WithUnsupportedFormat_ShouldThrowDifyInvalidFileException()
    {
        // Arrange
        var paramDto = new DifyAudioToTextParamDto
        {
            FilePath = "/path/to/audio.xyz",
            User = TestConstants.TestUserId
        };

        // Act
        var act = async () => await _sut.AudioToTextAsync(paramDto);

        // Assert
        var exception = await act.Should().ThrowAsync<DifyInvalidFileException>();
        exception.Which.FilePath.Should().Be("/path/to/audio.xyz");
        exception.Which.SupportedFormats.Should().Contain("mp3");
    }

    #endregion

    #region GetApplicationInfoAsync

    [Fact]
    public async Task GetApplicationInfoAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetApplicationInfoResDto>(
                $"parameters?user={TestConstants.TestUserId}",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyGetApplicationInfoResDto> { Success = true });

        // Act
        await _sut.GetApplicationInfoAsync(TestConstants.TestUserId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpGet<DifyGetApplicationInfoResDto>(
                $"parameters?user={TestConstants.TestUserId}",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetApplicationMetaAsync

    [Fact]
    public async Task GetApplicationMetaAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetApplicationMetaResDto>(
                $"meta?user={TestConstants.TestUserId}",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyGetApplicationMetaResDto> { Success = true });

        // Act
        await _sut.GetApplicationMetaAsync(TestConstants.TestUserId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpGet<DifyGetApplicationMetaResDto>(
                $"meta?user={TestConstants.TestUserId}",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region FileUploadAsync

    [Fact]
    public async Task FileUploadAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.PostUploadFileAsync<DifyFileUploadResDto>(
                "files/upload",
                It.IsAny<DifyBaseFileRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyFileUploadResDto>
            {
                Success = true,
                Data = new DifyFileUploadResDto { Id = "file-123" }
            });

        var paramDto = new DifyFileUploadParamDto
        {
            FilePath = "/path/to/file.txt",
            User = TestConstants.TestUserId
        };

        // Act
        var result = await _sut.FileUploadAsync(paramDto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data?.Id.Should().Be("file-123");
    }

    #endregion

    #region GetConversationVariablesAsync

    [Fact]
    public async Task GetConversationVariablesAsync_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetConversationVariablesResDto>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyGetConversationVariablesResDto> { Success = true });

        var paramDto = new DifyGetConversationVariablesParamDto
        {
            ConversationId = TestConstants.TestConversationId,
            User = TestConstants.TestUserId,
            Limit = 20
        };

        // Act
        await _sut.GetConversationVariablesAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain($"conversations/{TestConstants.TestConversationId}/variables");
        capturedUrl.Should().Contain($"user={TestConstants.TestUserId}");
    }

    #endregion

    #region GetAppFeedbacksAsync

    [Fact]
    public async Task GetAppFeedbacksAsync_ShouldBuildCorrectUrl()
    {
        // Arrange
        string capturedUrl = null;
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetAppFeedbacksResDto>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, ct) => capturedUrl = url)
            .ReturnsAsync(new DifyApiResult<DifyGetAppFeedbacksResDto> { Success = true });

        var paramDto = new DifyGetAppFeedbacksParamDto
        {
            User = TestConstants.TestUserId,
            Page = 1,
            Limit = 20,
            Rating = "like",
            Keyword = "test"
        };

        // Act
        await _sut.GetAppFeedbacksAsync(paramDto);

        // Assert
        capturedUrl.Should().Contain("app/feedbacks");
        capturedUrl.Should().Contain("rating=like");
        capturedUrl.Should().Contain("keyword=test");
    }

    #endregion

    #region GetFilePreviewAsync

    [Fact]
    public async Task GetFilePreviewAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var fileId = "file-123";
        _mockRequestExtension
            .Setup(x => x.HttpGet<DifyGetFilePreviewResDto>(
                $"files/{fileId}/preview?user={TestConstants.TestUserId}",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyGetFilePreviewResDto> { Success = true });

        // Act
        await _sut.GetFilePreviewAsync(fileId, TestConstants.TestUserId);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpGet<DifyGetFilePreviewResDto>(
                $"files/{fileId}/preview?user={TestConstants.TestUserId}",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region TextToAudioAsync

    [Fact]
    public async Task TextToAudioAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        _mockRequestExtension
            .Setup(x => x.HttpPost<DifyTextToAudioResDto>(
                "text-to-audio",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DifyApiResult<DifyTextToAudioResDto> { Success = true });

        var paramDto = new DifyTextToAudioParamDto
        {
            Text = "Hello world",
            User = TestConstants.TestUserId
        };

        // Act
        await _sut.TextToAudioAsync(paramDto);

        // Assert
        _mockRequestExtension.Verify(
            x => x.HttpPost<DifyTextToAudioResDto>(
                "text-to-audio",
                It.IsAny<DifyBaseRequestParamDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
