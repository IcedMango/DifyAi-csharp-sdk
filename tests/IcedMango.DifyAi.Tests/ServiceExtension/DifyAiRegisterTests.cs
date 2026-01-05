namespace IcedMango.DifyAi.Tests.ServiceExtension;

/// <summary>
/// Unit tests for DifyAiRegister
/// </summary>
public class DifyAiRegisterTests
{
    [Fact]
    public void RegisterBot_QuickMethod_ShouldValidateConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot("", TestConstants.ValidBotApiKey); // Empty name
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("Name");
    }

    [Fact]
    public void RegisterBot_QuickMethod_ShouldAcceptCustomBaseUrl()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(
                TestConstants.BotInstanceName,
                TestConstants.ValidBotApiKey,
                TestConstants.CustomBaseUrl);
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RegisterBot_FullConfig_ShouldValidateConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(new DifyAiInstanceConfig
            {
                Name = TestConstants.BotInstanceName,
                ApiKey = "", // Empty API key
                BaseUrl = TestConstants.DefaultBaseUrl
            });
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("ApiKey");
    }

    [Fact]
    public void RegisterBot_WithProxy_ShouldAcceptProxyConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(new DifyAiInstanceConfig
            {
                Name = TestConstants.BotInstanceName,
                ApiKey = TestConstants.ValidBotApiKey,
                BaseUrl = TestConstants.DefaultBaseUrl,
                ProxyUrl = "socks5://127.0.0.1:1080"
            });
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RegisterBot_WithIgnoreSsl_ShouldAcceptConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot(new DifyAiInstanceConfig
            {
                Name = TestConstants.BotInstanceName,
                ApiKey = TestConstants.ValidBotApiKey,
                IgnoreSslErrors = true
            });
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RegisterDataset_QuickMethod_ShouldValidateConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterDataset("", TestConstants.ValidDatasetApiKey); // Empty name
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("Name");
    }

    [Fact]
    public void RegisterDataset_FullConfig_ShouldValidateConfig()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterDataset(new DifyAiInstanceConfig
            {
                Name = TestConstants.DatasetInstanceName,
                ApiKey = "", // Empty API key
            });
        });

        // Assert
        act.Should().Throw<DifyConfigurationException>()
            .Which.PropertyName.Should().Be("ApiKey");
    }

    [Fact]
    public void Register_ShouldSupportMethodChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            register
                .RegisterBot("Bot1", "app-1")
                .RegisterBot("Bot2", "app-2")
                .RegisterDataset("Dataset1", "dataset-1")
                .RegisterDataset("Dataset2", "dataset-2");
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Build_ShouldRegisterNamedHttpClients()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", "app-xxx");
            register.RegisterDataset("TestDataset", "dataset-xxx");
        });

        // Assert - check that HttpClient services are registered
        var httpClientDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IHttpClientFactory));

        // IHttpClientFactory is registered indirectly via AddHttpClient
        services.Any(d => d.ServiceType.Name.Contains("HttpClient")).Should().BeTrue();
    }

    [Fact]
    public void Build_BotClientName_ShouldFollowNamingConvention()
    {
        // Arrange
        var services = new ServiceCollection();
        var botName = "CustomerService";

        // Act
        services.AddDifyAi(register =>
        {
            register.RegisterBot(botName, "app-xxx");
        });

        var provider = services.BuildServiceProvider();
        var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

        // Assert - should be able to create client with expected name
        var expectedClientName = $"DifyAi.Bot.{botName}";
        var act = () => httpClientFactory.CreateClient(expectedClientName);

        act.Should().NotThrow();
    }

    [Fact]
    public void Build_DatasetClientName_ShouldFollowNamingConvention()
    {
        // Arrange
        var services = new ServiceCollection();
        var datasetName = "ProductDocs";

        // Act
        services.AddDifyAi(register =>
        {
            register.RegisterDataset(datasetName, "dataset-xxx");
        });

        var provider = services.BuildServiceProvider();
        var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

        // Assert - should be able to create client with expected name
        var expectedClientName = $"DifyAi.Dataset.{datasetName}";
        var act = () => httpClientFactory.CreateClient(expectedClientName);

        act.Should().NotThrow();
    }

    [Fact]
    public void RegisterBot_WithDuplicateName_ShouldThrowException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act - registering same name twice should throw
        var act = () => services.AddDifyAi(register =>
        {
            register.RegisterBot("SameName", "app-1");
            register.RegisterBot("SameName", "app-2");
        });

        // Assert - duplicate keys throw ArgumentException
        act.Should().Throw<ArgumentException>();
    }
}
