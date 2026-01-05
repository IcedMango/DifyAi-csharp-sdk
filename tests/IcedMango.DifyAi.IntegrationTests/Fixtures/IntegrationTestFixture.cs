using IcedMango.DifyAi.IntegrationTests.Logging;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace IcedMango.DifyAi.IntegrationTests.Fixtures;

/// <summary>
/// Integration test fixture that provides real Dify API services.
/// Uses real HTTP calls to Dify API endpoints.
/// </summary>
public class IntegrationTestFixture : IDisposable
{
    public TestConfiguration Configuration { get; }
    public ServiceProvider? ServiceProvider { get; private set; }

    public IntegrationTestFixture()
    {
        Configuration = new TestConfiguration();
    }

    /// <summary>
    /// Mask API key for logging (show first 8 and last 4 characters)
    /// </summary>
    private static string MaskApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return "[EMPTY]";

        if (apiKey.Length <= 12)
            return new string('*', apiKey.Length);

        return $"{apiKey[..8]}...{apiKey[^4..]}";
    }

    /// <summary>
    /// Build service provider with real DifyAi configuration
    /// </summary>
    public ServiceProvider BuildServiceProvider(ITestOutputHelper? output = null)
    {
        var services = new ServiceCollection();

        // Use xUnit output if available, otherwise fall back to console
        services.AddLogging(builder =>
        {
            // Set minimum level to Debug to ensure all logs are captured
            builder.SetMinimumLevel(LogLevel.Debug);

            if (output != null)
            {
                builder.AddXunit(output, LogLevel.Debug);
            }
            else
            {
                builder.AddConsole();
            }
        });

        services.AddDifyAi(register =>
        {
            // Register Bot if configured
            if (Configuration.IsBotConfigured)
            {
                register.RegisterBot(
                    Configuration.BotName,
                    Configuration.BotApiKey,
                    Configuration.BotBaseUrl);
            }

            // Register Dataset if configured
            if (Configuration.IsDatasetConfigured)
            {
                register.RegisterDataset(
                    Configuration.DatasetName,
                    Configuration.DatasetApiKey,
                    Configuration.DatasetBaseUrl);
            }
        });

        ServiceProvider = services.BuildServiceProvider();
        return ServiceProvider;
    }

    /// <summary>
    /// Get Bot service for integration testing
    /// </summary>
    public IDifyAiChatServices GetBotService(ITestOutputHelper? output = null)
    {
        if (!Configuration.IsBotConfigured)
        {
            throw new InvalidOperationException(Configuration.BotSkipReason);
        }

        var provider = ServiceProvider ?? BuildServiceProvider(output);
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();
        return factory.GetBotService(Configuration.BotName);
    }

    /// <summary>
    /// Get Dataset service for integration testing
    /// </summary>
    public IDifyAiDatasetServices GetDatasetService(ITestOutputHelper? output = null)
    {
        if (!Configuration.IsDatasetConfigured)
        {
            throw new InvalidOperationException(Configuration.DatasetSkipReason);
        }

        var provider = ServiceProvider ?? BuildServiceProvider(output);
        var factory = provider.GetRequiredService<IDifyAiServicesFactory>();
        return factory.GetDatasetService(Configuration.DatasetName);
    }

    /// <summary>
    /// Get the services factory
    /// </summary>
    public IDifyAiServicesFactory GetFactory(ITestOutputHelper? output = null)
    {
        var provider = ServiceProvider ?? BuildServiceProvider(output);
        return provider.GetRequiredService<IDifyAiServicesFactory>();
    }

    /// <summary>
    /// Reset the service provider (useful when switching between tests with different output helpers)
    /// </summary>
    public void ResetServiceProvider()
    {
        ServiceProvider?.Dispose();
        ServiceProvider = null;
    }

    public void Dispose()
    {
        ServiceProvider?.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Collection definition for integration tests that share the same fixture
/// </summary>
[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
}
