using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace DifyAi.Services;

/// <summary>
/// Unified Dify AI service factory implementation - provides on-demand retrieval of Bot and Dataset services
/// Service instances use Singleton pattern with permanent caching after first creation
/// </summary>
public class DifyAiServicesFactory : IDifyAiServicesFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RequestExtension> _logger;

    // Service instance cache - uses Lazy<T> to ensure thread-safe lazy initialization
    private readonly ConcurrentDictionary<string, Lazy<IDifyAiChatServices>> _botServiceCache = new();
    private readonly ConcurrentDictionary<string, Lazy<IDifyAiDatasetServices>> _datasetServiceCache = new();

    public DifyAiServicesFactory(
        IHttpClientFactory httpClientFactory,
        ILogger<RequestExtension> logger = null)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger;
    }

    /// <summary>
    /// Get Bot (Chat) service instance by name
    /// </summary>
    public IDifyAiChatServices GetBotService(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        // GetOrAdd ensures concurrency safety, Lazy ensures initialization only once
        var lazyService = _botServiceCache.GetOrAdd(name, key => new Lazy<IDifyAiChatServices>(() =>
        {
            var clientName = $"DifyAi.Bot.{key}";

            try
            {
                var httpClient = _httpClientFactory.CreateClient(clientName);
                var requestExtension = new RequestExtension(httpClient, _logger);
                return new DifyAiChatServices(requestExtension);
            }
            catch (InvalidOperationException)
            {
                throw new DifyInstanceNotFoundException(key, "Bot");
            }
        }));

        return lazyService.Value;
    }

    /// <summary>
    /// Get Dataset service instance by name
    /// </summary>
    public IDifyAiDatasetServices GetDatasetService(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        var lazyService = _datasetServiceCache.GetOrAdd(name, key => new Lazy<IDifyAiDatasetServices>(() =>
        {
            var clientName = $"DifyAi.Dataset.{key}";

            try
            {
                var httpClient = _httpClientFactory.CreateClient(clientName);

                // Debug: Log HttpClient state after creation
                _logger?.LogDebug(
                    "[DifyAiServicesFactory] Created HttpClient for: {ClientName}, BaseAddress: {BaseAddress}, HasAuthorization: {HasAuth}",
                    clientName,
                    httpClient.BaseAddress?.ToString() ?? "[NULL]",
                    httpClient.DefaultRequestHeaders.Contains("Authorization"));

                var requestExtension = new RequestExtension(httpClient, _logger);
                return new DifyAiDatasetServices(requestExtension);
            }
            catch (InvalidOperationException)
            {
                throw new DifyInstanceNotFoundException(key, "Dataset");
            }
        }));

        return lazyService.Value;
    }
}
