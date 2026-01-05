namespace IcedMango.DifyAi.IntegrationTests.Fixtures;

/// <summary>
/// Configuration helper for integration tests.
/// Loads configuration from appsettings.json and environment variables.
/// </summary>
public class TestConfiguration
{
    private readonly IConfiguration _configuration;

    public TestConfiguration()
    {
        var basePath = AppContext.BaseDirectory;

        _configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
    }

    /// <summary>
    /// Bot API Key from configuration or environment variable DIFY_BOT_API_KEY
    /// </summary>
    public string BotApiKey =>
        Environment.GetEnvironmentVariable("DIFY_BOT_API_KEY")
        ?? _configuration["DifyAi:Bot:ApiKey"]
        ?? string.Empty;

    /// <summary>
    /// Bot instance name
    /// </summary>
    public string BotName =>
        _configuration["DifyAi:Bot:Name"]
        ?? "IntegrationTestBot";

    /// <summary>
    /// Bot base URL
    /// </summary>
    public string BotBaseUrl =>
        Environment.GetEnvironmentVariable("DIFY_BOT_BASE_URL")
        ?? _configuration["DifyAi:Bot:BaseUrl"]
        ?? "https://api.dify.ai/v1/";

    /// <summary>
    /// Dataset API Key from configuration or environment variable DIFY_DATASET_API_KEY
    /// </summary>
    public string DatasetApiKey =>
        Environment.GetEnvironmentVariable("DIFY_DATASET_API_KEY")
        ?? _configuration["DifyAi:Dataset:ApiKey"]
        ?? string.Empty;

    /// <summary>
    /// Dataset instance name
    /// </summary>
    public string DatasetName =>
        _configuration["DifyAi:Dataset:Name"]
        ?? "IntegrationTestDataset";

    /// <summary>
    /// Dataset base URL
    /// </summary>
    public string DatasetBaseUrl =>
        Environment.GetEnvironmentVariable("DIFY_DATASET_BASE_URL")
        ?? _configuration["DifyAi:Dataset:BaseUrl"]
        ?? "https://api.dify.ai/v1/";

    /// <summary>
    /// Check if Bot API key is configured
    /// </summary>
    public bool IsBotConfigured => !string.IsNullOrWhiteSpace(BotApiKey) && !BotApiKey.Contains("your-real-api-key");

    /// <summary>
    /// Check if Dataset API key is configured
    /// </summary>
    public bool IsDatasetConfigured => !string.IsNullOrWhiteSpace(DatasetApiKey) && !DatasetApiKey.Contains("your-real-api-key");

    /// <summary>
    /// Get skip reason if Bot is not configured
    /// </summary>
    public string BotSkipReason => IsBotConfigured
        ? string.Empty
        : "Bot API key not configured. Set DIFY_BOT_API_KEY environment variable or update appsettings.json";

    /// <summary>
    /// Get skip reason if Dataset is not configured
    /// </summary>
    public string DatasetSkipReason => IsDatasetConfigured
        ? string.Empty
        : "Dataset API key not configured. Set DIFY_DATASET_API_KEY environment variable or update appsettings.json";
}
