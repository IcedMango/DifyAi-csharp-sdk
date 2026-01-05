using System.Net;
using DifyAi.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DifyAi.ServiceExtension;

/// <summary>
/// Dify AI service register - inspired by Senparc.Weixin design pattern
/// </summary>
public class DifyAiRegister
{
    private readonly IServiceCollection _services;
    private readonly List<DifyAiInstanceConfig> _botConfigs = new List<DifyAiInstanceConfig>();
    private readonly List<DifyAiInstanceConfig> _datasetConfigs = new List<DifyAiInstanceConfig>();

    internal DifyAiRegister(IServiceCollection services)
    {
        _services = services;
    }

    #region RegisterBot

    /// <summary>
    /// Register Bot instance - quick registration
    /// </summary>
    /// <param name="name">Instance name</param>
    /// <param name="apiKey">API Key</param>
    /// <param name="baseUrl">Base URL (default: https://api.dify.ai/v1/)</param>
    /// <returns>Register instance (supports method chaining)</returns>
    public DifyAiRegister RegisterBot(
        string name,
        string apiKey,
        string baseUrl = "https://api.dify.ai/v1/")
    {
        var config = new DifyAiInstanceConfig
        {
            Name = name,
            ApiKey = apiKey,
            BaseUrl = baseUrl
        };

        config.Validate();
        _botConfigs.Add(config);

        return this;
    }

    /// <summary>
    /// Register Bot instance - advanced registration (full config object)
    /// </summary>
    /// <param name="config">Full configuration object</param>
    /// <returns>Register instance (supports method chaining)</returns>
    public DifyAiRegister RegisterBot(DifyAiInstanceConfig config)
    {
        config.Validate();
        _botConfigs.Add(config);

        return this;
    }

    #endregion

    #region RegisterDataset

    /// <summary>
    /// Register Dataset instance - quick registration
    /// </summary>
    /// <param name="name">Instance name</param>
    /// <param name="apiKey">API Key</param>
    /// <param name="baseUrl">Base URL (default: https://api.dify.ai/v1/)</param>
    /// <returns>Register instance (supports method chaining)</returns>
    public DifyAiRegister RegisterDataset(
        string name,
        string apiKey,
        string baseUrl = "https://api.dify.ai/v1/")
    {
        var config = new DifyAiInstanceConfig
        {
            Name = name,
            ApiKey = apiKey,
            BaseUrl = baseUrl
        };

        config.Validate();
        _datasetConfigs.Add(config);

        return this;
    }

    /// <summary>
    /// Register Dataset instance - advanced registration (full config object)
    /// </summary>
    /// <param name="config">Full configuration object</param>
    /// <returns>Register instance (supports method chaining)</returns>
    public DifyAiRegister RegisterDataset(DifyAiInstanceConfig config)
    {
        config.Validate();
        _datasetConfigs.Add(config);

        return this;
    }

    #endregion

    #region Build

    /// <summary>
    /// Complete registration and build services
    /// </summary>
    internal void Build()
    {
        // Register configuration store (Singleton)
        _services.AddSingleton(new DifyAiConfigurationStore
        {
            BotConfigs = _botConfigs.ToDictionary(c => c.Name),
            DatasetConfigs = _datasetConfigs.ToDictionary(c => c.Name)
        });

        // Register Named HttpClient for each Bot instance
        foreach (var config in _botConfigs)
        {
            var clientName = $"DifyAi.Bot.{config.Name}";
            var formattedApiKey = config.ApiKey.FormatApiKey();

            _services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new Uri(config.BaseUrl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {formattedApiKey}");
                client.DefaultRequestHeaders.Add("User-Agent", "IcedMango/DifyAiSdk");
            })
            .ConfigurePrimaryHttpMessageHandler(() => CreateHandler(config));
        }

        // Register Named HttpClient for each Dataset instance
        foreach (var config in _datasetConfigs)
        {
            var clientName = $"DifyAi.Dataset.{config.Name}";
            var formattedApiKey = config.ApiKey.FormatApiKey();

            _services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new Uri(config.BaseUrl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {formattedApiKey}");
                client.DefaultRequestHeaders.Add("User-Agent", "IcedMango/DifyAiSdk");
            })
            .ConfigurePrimaryHttpMessageHandler(() => CreateHandler(config));
        }

        // Register unified service factory (Singleton - because service instances are cached internally)
        _services.AddSingleton<IDifyAiServicesFactory, DifyAiServicesFactory>();
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
    /// Create HttpClientHandler
    /// </summary>
    private static HttpClientHandler CreateHandler(DifyAiInstanceConfig config)
    {
        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        // Configure proxy
        if (!string.IsNullOrWhiteSpace(config.ProxyUrl))
        {
            handler.Proxy = new WebProxy(config.ProxyUrl)
            {
                UseDefaultCredentials = false
            };
        }

        // Configure SSL certificate validation
        if (config.IgnoreSslErrors)
        {
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }

        return handler;
    }

    #endregion
}

/// <summary>
/// Dify AI configuration store - internal use only
/// </summary>
internal class DifyAiConfigurationStore
{
    public Dictionary<string, DifyAiInstanceConfig> BotConfigs { get; set; } = new Dictionary<string, DifyAiInstanceConfig>();
    public Dictionary<string, DifyAiInstanceConfig> DatasetConfigs { get; set; } = new Dictionary<string, DifyAiInstanceConfig>();
}
