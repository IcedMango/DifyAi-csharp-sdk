namespace IcedMango.DifyAi.IntegrationTests.DependencyInjection;

/// <summary>
/// Integration tests for DI service registration
/// Tests that all services are correctly registered and can be resolved
/// </summary>
public class ServiceRegistrationTests
{
    [Fact]
    public void AddDifyAi_ShouldRegisterIDifyAiServicesFactory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", "app-test-key");
        });

        var provider = services.BuildServiceProvider();

        // Act
        var factory = provider.GetService<IDifyAiServicesFactory>();

        // Assert
        factory.Should().NotBeNull();
    }

    [Fact]
    public void AddDifyAi_FactoryShouldBeSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", "app-test-key");
        });

        var provider = services.BuildServiceProvider();

        // Act
        var factory1 = provider.GetRequiredService<IDifyAiServicesFactory>();
        var factory2 = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Assert
        factory1.Should().BeSameAs(factory2);
    }

    [Fact]
    public void GetBotService_ShouldReturnChatService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", "app-test-key");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var chatService = factory.GetBotService("TestBot");

        // Assert
        chatService.Should().NotBeNull();
        chatService.Should().BeAssignableTo<IDifyAiChatServices>();
    }

    [Fact]
    public void GetDatasetService_ShouldReturnDatasetService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterDataset("TestDataset", "dataset-test-key");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var datasetService = factory.GetDatasetService("TestDataset");

        // Assert
        datasetService.Should().NotBeNull();
        datasetService.Should().BeAssignableTo<IDifyAiDatasetServices>();
    }

    [Fact]
    public void GetBotService_CalledMultipleTimes_ShouldReturnSameInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", "app-test-key");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var service1 = factory.GetBotService("TestBot");
        var service2 = factory.GetBotService("TestBot");

        // Assert - Singleton pattern
        service1.Should().BeSameAs(service2);
    }

    [Fact]
    public void MultipleBotInstances_ShouldReturnDifferentServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterBot("Bot1", "app-key-1");
            register.RegisterBot("Bot2", "app-key-2");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var bot1 = factory.GetBotService("Bot1");
        var bot2 = factory.GetBotService("Bot2");

        // Assert
        bot1.Should().NotBeSameAs(bot2);
    }

    [Fact]
    public void IHttpClientFactory_ShouldBeRegistered()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", "app-test-key");
        });

        var provider = services.BuildServiceProvider();

        // Act
        var httpClientFactory = provider.GetService<IHttpClientFactory>();

        // Assert
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void HttpClient_ShouldHaveCorrectConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        var apiKey = "app-test-key-12345";
        var baseUrl = "https://custom.dify.ai/v1/";

        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", apiKey, baseUrl);
        });

        var provider = services.BuildServiceProvider();
        var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();

        // Act
        var client = httpClientFactory.CreateClient("DifyAi.Bot.TestBot");

        // Assert
        client.Should().NotBeNull();
        client.BaseAddress.Should().NotBeNull();
        client.BaseAddress!.ToString().Should().Be(baseUrl);
    }

    [Fact]
    public void GetBotService_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterBot("TestBot", "app-test-key");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var act = () => factory.GetBotService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetDatasetService_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddDifyAi(register =>
        {
            register.RegisterDataset("TestDataset", "dataset-test-key");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var act = () => factory.GetDatasetService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
