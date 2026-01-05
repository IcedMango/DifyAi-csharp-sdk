namespace DifyAi.Configuration;

/// <summary>
///     DifyAi instance configuration
/// </summary>
public class DifyAiInstanceConfig
{
    /// <summary>
    ///     Instance name (required)
    /// </summary>
    /// <example>CustomerService</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     API Key (required)
    /// </summary>
    /// <example>app-xxxxxxxx or dataset-xxxxxxxx</example>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    ///     DifyAi API base URL (optional, defaults to official address)
    /// </summary>
    /// <example>https://api.dify.ai/v1/</example>
    public string BaseUrl { get; set; } = "https://api.dify.ai/v1/";

    /// <summary>
    ///     Proxy configuration URL (optional)
    /// </summary>
    /// <example>socks5://127.0.0.1:1080</example>
    public string ProxyUrl { get; set; }

    /// <summary>
    ///     Ignore SSL certificate errors (default: false)
    ///     WARNING: Setting this to true is a security risk, only use in development/testing environments
    /// </summary>
    public bool IgnoreSslErrors { get; set; } = false;

    /// <summary>
    ///     Validate configuration
    /// </summary>
    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new DifyConfigurationException("Instance name cannot be empty")
            {
                PropertyName = nameof(Name)
            };
        }

        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new DifyConfigurationException("API Key cannot be empty")
            {
                PropertyName = nameof(ApiKey)
            };
        }

        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            throw new DifyConfigurationException("BaseUrl cannot be empty")
            {
                PropertyName = nameof(BaseUrl)
            };
        }

        // Ensure BaseUrl ends with /
        if (!BaseUrl.EndsWith("/"))
        {
            BaseUrl += "/";
        }
    }
}