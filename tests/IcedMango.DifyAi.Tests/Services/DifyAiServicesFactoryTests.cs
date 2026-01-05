namespace IcedMango.DifyAi.Tests.Services;

/// <summary>
/// Unit tests for DifyAiServicesFactory
/// </summary>
public class DifyAiServicesFactoryTests
{
    [Fact]
    public void GetBotService_ExistingInstance_ShouldReturnService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var service = factory.GetBotService(TestConstants.BotInstanceName);

        // Assert
        service.Should().NotBeNull();
        service.Should().BeAssignableTo<IDifyAiChatServices>();
    }

    [Fact]
    public void GetBotService_NonExistingInstance_ShouldStillReturnService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act - Note: IHttpClientFactory returns a default client even for unregistered names
        // The service will be created but may not have proper configuration
        var service = factory.GetBotService(TestConstants.NonExistentInstanceName);

        // Assert - Due to IHttpClientFactory behavior, a service is returned
        // The actual API calls would fail due to missing authorization
        service.Should().NotBeNull();
    }

    [Fact]
    public void GetBotService_CalledTwice_ShouldReturnSameInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var service1 = factory.GetBotService(TestConstants.BotInstanceName);
        var service2 = factory.GetBotService(TestConstants.BotInstanceName);

        // Assert - Should return cached instance (Singleton pattern)
        service1.Should().BeSameAs(service2);
    }

    [Fact]
    public void GetDatasetService_ExistingInstance_ShouldReturnService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterDataset(TestConstants.DatasetInstanceName, TestConstants.ValidDatasetApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var service = factory.GetDatasetService(TestConstants.DatasetInstanceName);

        // Assert
        service.Should().NotBeNull();
        service.Should().BeAssignableTo<IDifyAiDatasetServices>();
    }

    [Fact]
    public void GetDatasetService_CalledTwice_ShouldReturnSameInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterDataset(TestConstants.DatasetInstanceName, TestConstants.ValidDatasetApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var service1 = factory.GetDatasetService(TestConstants.DatasetInstanceName);
        var service2 = factory.GetDatasetService(TestConstants.DatasetInstanceName);

        // Assert - Should return cached instance
        service1.Should().BeSameAs(service2);
    }

    [Fact]
    public void GetBotService_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var act = () => factory.GetBotService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetBotService_WithEmptyName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var act = () => factory.GetBotService("");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetDatasetService_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterDataset(TestConstants.DatasetInstanceName, TestConstants.ValidDatasetApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var act = () => factory.GetDatasetService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GetBotService_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act - Concurrent access from multiple threads
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => Task.Run(() => factory.GetBotService(TestConstants.BotInstanceName)))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        // Assert - All should return the same cached instance
        results.Should().AllBeEquivalentTo(results[0]);
        results.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public async Task GetDatasetService_ConcurrentAccess_ShouldBeThreadSafe()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterDataset(TestConstants.DatasetInstanceName, TestConstants.ValidDatasetApiKey);
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act - Concurrent access from multiple threads
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => Task.Run(() => factory.GetDatasetService(TestConstants.DatasetInstanceName)))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        // Assert - All should return the same cached instance
        results.Should().AllBeEquivalentTo(results[0]);
        results.Distinct().Should().HaveCount(1);
    }

    [Fact]
    public void GetBotService_DifferentNames_ShouldReturnDifferentInstances()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot("Bot1", "app-1");
            register.RegisterBot("Bot2", "app-2");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Act
        var bot1 = factory.GetBotService("Bot1");
        var bot2 = factory.GetBotService("Bot2");

        // Assert - Different names should return different instances
        bot1.Should().NotBeSameAs(bot2);
    }

    [Fact]
    public void FactoryIsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();

        // Act
        var factory1 = provider.GetRequiredService<IDifyAiServicesFactory>();
        var factory2 = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Assert
        factory1.Should().BeSameAs(factory2);
    }
}
