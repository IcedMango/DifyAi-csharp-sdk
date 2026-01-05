using Xunit.Abstractions;

namespace IcedMango.DifyAi.IntegrationTests.ChatApi;

/// <summary>
/// Integration tests for Chat API using real Dify API.
/// These tests require valid API keys configured in appsettings.json or environment variables.
/// </summary>
[Collection("IntegrationTests")]
public class ChatCompletionIntegrationTests : IDisposable
{
    private readonly IntegrationTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public ChatCompletionIntegrationTests(IntegrationTestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        // Reset service provider to use the current test's output helper
        _fixture.ResetServiceProvider();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void TestConfiguration_ShouldLoadCorrectly()
    {
        // This test always runs to verify test infrastructure
        _fixture.Configuration.Should().NotBeNull();
        _fixture.Configuration.BotName.Should().NotBeNullOrEmpty();
    }

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task CreateChatCompletionBlockModeAsync_WithValidRequest_ShouldReturnResponse()
    {
        // Skip if not configured
        Skip.If(!_fixture.Configuration.IsBotConfigured, _fixture.Configuration.BotSkipReason);

        // Arrange
        var chatService = _fixture.GetBotService(_output);
        var param = new DifyCreateChatCompletionParamDto
        {
            Query = "Hello, what is 1+1?",
            User = "integration-test-user"
        };

        // Act
        var result = await chatService.CreateChatCompletionBlockModeAsync(param);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Answer.Should().NotBeNullOrEmpty();
        result.Data.MessageId.Should().NotBeNullOrEmpty();
    }

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task GetApplicationInfoAsync_ShouldReturnAppInfo()
    {
        // Skip if not configured
        Skip.If(!_fixture.Configuration.IsBotConfigured, _fixture.Configuration.BotSkipReason);

        // Arrange
        var chatService = _fixture.GetBotService(_output);

        // Act
        var result = await chatService.GetApplicationInfoAsync("integration-test-user");

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task GetApplicationMetaAsync_ShouldReturnMetaInfo()
    {
        // Skip if not configured
        Skip.If(!_fixture.Configuration.IsBotConfigured, _fixture.Configuration.BotSkipReason);

        // Arrange
        var chatService = _fixture.GetBotService(_output);

        // Act
        var result = await chatService.GetApplicationMetaAsync("integration-test-user");

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task GetConversationListAsync_ShouldReturnList()
    {
        // Skip if not configured
        Skip.If(!_fixture.Configuration.IsBotConfigured, _fixture.Configuration.BotSkipReason);

        // Arrange
        var chatService = _fixture.GetBotService(_output);
        var param = new DifyGetConversationListParamDto
        {
            User = "integration-test-user",
            Limit = 10
        };

        // Act
        var result = await chatService.GetConversationListAsync(param);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
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
            register.RegisterBot("InvalidBot", "app-invalid-key-12345", _fixture.Configuration.BotBaseUrl);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();
        var chatService = factory.GetBotService("InvalidBot");

        var param = new DifyCreateChatCompletionParamDto
        {
            Query = "Test",
            User = "test-user"
        };

        // Act
        DifyApiResult<DifyCreateChatCompletionResDto>? result = null;
        try
        {
            result = await chatService.CreateChatCompletionBlockModeAsync(param);
        }
        catch (HttpRequestException ex)
        {
            Skip.If(true, $"Network error occurred: {ex.Message}");
        }

        // Assert - Should return error result (unauthorized or similar)
        result.Should().NotBeNull();
        result!.Success.Should().BeFalse();
    }

    public void Dispose()
    {
        // Cleanup if needed
        GC.SuppressFinalize(this);
    }
}
