namespace IcedMango.DifyAi.Tests.ServiceExtension;

/// <summary>
/// Unit tests for ServiceRegisterExtension
/// </summary>
public class ServiceRegisterExtensionTests
{
    [Fact]
    public void AddDifyAi_WithValidConfig_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        var provider = services.BuildServiceProvider();

        // Assert
        var factory = provider.GetService<IDifyAiServicesFactory>();
        factory.Should().NotBeNull();
    }

    [Fact]
    public void AddDifyAi_ShouldRegisterIDifyAiServicesFactory()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        // Assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IDifyAiServicesFactory));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddDifyAi_MultipleBots_ShouldRegisterAllInstances()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDifyAi(register =>
        {
            register.RegisterBot("Bot1", "app-key-1");
            register.RegisterBot("Bot2", "app-key-2");
            register.RegisterBot("Bot3", "app-key-3");
        });

        var provider = services.BuildServiceProvider();

        // Assert
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Should be able to get all registered bots
        var bot1 = factory.GetBotService("Bot1");
        var bot2 = factory.GetBotService("Bot2");
        var bot3 = factory.GetBotService("Bot3");

        bot1.Should().NotBeNull();
        bot2.Should().NotBeNull();
        bot3.Should().NotBeNull();
    }

    [Fact]
    public void AddDifyAi_MultipleDatasets_ShouldRegisterAllInstances()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDifyAi(register =>
        {
            register.RegisterDataset("Dataset1", "dataset-key-1");
            register.RegisterDataset("Dataset2", "dataset-key-2");
        });

        var provider = services.BuildServiceProvider();

        // Assert
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        var dataset1 = factory.GetDatasetService("Dataset1");
        var dataset2 = factory.GetDatasetService("Dataset2");

        dataset1.Should().NotBeNull();
        dataset2.Should().NotBeNull();
    }

    [Fact]
    public void AddDifyAi_ShouldSupportMethodChaining()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddDifyAi(register =>
        {
            register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
        });

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddDifyAi_WithEmptyConfig_ShouldNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var act = () => services.AddDifyAi(register =>
        {
            // No registrations - intentional
        });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void AddDifyAi_WithMixedRegistrations_ShouldRegisterAll()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDifyAi(register =>
        {
            register
                .RegisterBot("CustomerService", "app-customer")
                .RegisterBot("TechSupport", "app-tech")
                .RegisterDataset("ProductDocs", "dataset-products")
                .RegisterDataset("FAQ", "dataset-faq");
        });

        var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();

        // Assert
        factory.GetBotService("CustomerService").Should().NotBeNull();
        factory.GetBotService("TechSupport").Should().NotBeNull();
        factory.GetDatasetService("ProductDocs").Should().NotBeNull();
        factory.GetDatasetService("FAQ").Should().NotBeNull();
    }
}
