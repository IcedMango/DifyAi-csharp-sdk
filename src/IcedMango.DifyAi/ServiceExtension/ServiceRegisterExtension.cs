using Microsoft.Extensions.DependencyInjection;

namespace DifyAi.ServiceExtension;

/// <summary>
/// Dify AI service extension methods
/// </summary>
public static class ServiceRegisterExtension
{
    /// <summary>
    /// Add and configure Dify AI services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configure">Configuration action</param>
    /// <returns>Service collection (supports method chaining)</returns>
    /// <example>
    /// <code>
    /// services.AddDifyAi(register =>
    /// {
    ///     // Quick registration
    ///     register.RegisterBot("CustomerService", "app-xxx");
    ///
    ///     // Advanced registration
    ///     register.RegisterBot(new DifyAiInstanceConfig
    ///     {
    ///         Name = "ProxyBot",
    ///         ApiKey = "app-yyy",
    ///         BaseUrl = "https://api.dify.ai/v1/",
    ///         ProxyUrl = "socks5://127.0.0.1:8889",
    ///         IgnoreSslErrors = true
    ///     });
    ///
    ///     register.RegisterDataset("ProductDocs", "dataset-xxx");
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddDifyAi(
        this IServiceCollection services,
        Action<DifyAiRegister> configure)
    {
        var register = new DifyAiRegister(services);
        configure(register);
        register.Build();

        return services;
    }
}
