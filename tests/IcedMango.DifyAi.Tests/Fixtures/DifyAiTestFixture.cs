namespace IcedMango.DifyAi.Tests.Fixtures;

/// <summary>
/// Test fixture for DifyAi unit tests.
/// Provides common setup for mocking HttpClient and DI container.
/// </summary>
public class DifyAiTestFixture : IDisposable
{
    public ServiceProvider? ServiceProvider { get; private set; }
    public MockHttpMessageHandler MockHttp { get; private set; }

    public DifyAiTestFixture()
    {
        MockHttp = new MockHttpMessageHandler();
    }

    /// <summary>
    /// Reset the mock HTTP handler for a new test
    /// </summary>
    public void ResetMockHttp()
    {
        MockHttp = new MockHttpMessageHandler();
    }

    /// <summary>
    /// Create a service collection with DifyAi services registered
    /// </summary>
    public IServiceCollection CreateServiceCollection()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        return services;
    }

    /// <summary>
    /// Build service provider with Bot instance using mock HTTP
    /// </summary>
    public ServiceProvider BuildWithBotMock(
        string instanceName = TestConstants.BotInstanceName,
        string apiKey = TestConstants.ValidBotApiKey,
        string baseUrl = TestConstants.DefaultBaseUrl)
    {
        var services = CreateServiceCollection();

        // Register mock HttpClient
        services.AddHttpClient(TestConstants.HttpClientNames.Bot(instanceName), client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }).ConfigurePrimaryHttpMessageHandler(() => MockHttp);

        // Register factory
        services.AddSingleton<IDifyAiServicesFactory>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new DifyAiServicesFactory(httpClientFactory);
        });

        ServiceProvider = services.BuildServiceProvider();
        return ServiceProvider;
    }

    /// <summary>
    /// Build service provider with Dataset instance using mock HTTP
    /// </summary>
    public ServiceProvider BuildWithDatasetMock(
        string instanceName = TestConstants.DatasetInstanceName,
        string apiKey = TestConstants.ValidDatasetApiKey,
        string baseUrl = TestConstants.DefaultBaseUrl)
    {
        var services = CreateServiceCollection();

        // Register mock HttpClient
        services.AddHttpClient(TestConstants.HttpClientNames.Dataset(instanceName), client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }).ConfigurePrimaryHttpMessageHandler(() => MockHttp);

        // Register factory
        services.AddSingleton<IDifyAiServicesFactory>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new DifyAiServicesFactory(httpClientFactory);
        });

        ServiceProvider = services.BuildServiceProvider();
        return ServiceProvider;
    }

    /// <summary>
    /// Build service provider with full DifyAi registration
    /// </summary>
    public ServiceProvider BuildWithFullRegistration(
        Action<DifyAiRegister>? configure = null)
    {
        var services = CreateServiceCollection();

        services.AddDifyAi(register =>
        {
            if (configure != null)
            {
                configure(register);
            }
            else
            {
                // Default registration
                register.RegisterBot(TestConstants.BotInstanceName, TestConstants.ValidBotApiKey);
                register.RegisterDataset(TestConstants.DatasetInstanceName, TestConstants.ValidDatasetApiKey);
            }
        });

        ServiceProvider = services.BuildServiceProvider();
        return ServiceProvider;
    }

    /// <summary>
    /// Load mock response from test data file
    /// </summary>
    public static string LoadMockResponse(string relativePath)
    {
        var path = Path.Combine(AppContext.BaseDirectory, relativePath);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Mock response file not found: {path}");
        }
        return File.ReadAllText(path);
    }

    /// <summary>
    /// Setup mock HTTP response for a specific endpoint
    /// </summary>
    public void SetupMockResponse(
        string urlPattern,
        string responseContent,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string contentType = "application/json")
    {
        MockHttp.When(urlPattern)
            .Respond(statusCode, contentType, responseContent);
    }

    /// <summary>
    /// Setup mock HTTP response with POST method
    /// </summary>
    public void SetupPostMockResponse(
        string urlPattern,
        string responseContent,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string contentType = "application/json")
    {
        MockHttp.When(HttpMethod.Post, urlPattern)
            .Respond(statusCode, contentType, responseContent);
    }

    /// <summary>
    /// Setup mock HTTP error response
    /// </summary>
    public void SetupMockErrorResponse(
        string urlPattern,
        HttpStatusCode statusCode,
        string errorCode = "invalid_param",
        string message = "Test error message")
    {
        var errorResponse = $@"{{
            ""code"": ""{errorCode}"",
            ""message"": ""{message}"",
            ""status"": {(int)statusCode}
        }}";

        MockHttp.When(urlPattern)
            .Respond(statusCode, "application/json", errorResponse);
    }

    public void Dispose()
    {
        ServiceProvider?.Dispose();
        MockHttp?.Dispose();
        GC.SuppressFinalize(this);
    }
}
