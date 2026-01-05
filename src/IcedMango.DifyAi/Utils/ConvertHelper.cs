using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DifyAi.Utils;

internal static class ConvertHelper
{
    // Static cached settings for better performance
    private static readonly JsonSerializerSettings DefaultSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new List<JsonConverter>
        {
            new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }
        }
    };

    private static readonly JsonSerializerSettings IgnoreNullSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new List<JsonConverter>
        {
            new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }
        },
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
    };

    /// <summary>
    ///     Convert object to json string
    /// </summary>
    /// <param name="obj">Object to serialize</param>
    /// <param name="ignoreNullProperty">Whether to ignore null properties (default: false)</param>
    /// <returns>JSON string</returns>
    public static string ToJson(this object obj, bool ignoreNullProperty = false)
    {
        return JsonConvert.SerializeObject(obj, ignoreNullProperty ? IgnoreNullSettings : DefaultSettings);
    }
    
    /// <summary>
    ///     Format API Key by removing "Bearer " prefix if present
    /// </summary>
    /// <param name="apiKey">API key string</param>
    /// <returns>Formatted API key without "Bearer " prefix</returns>
    public static string FormatApiKey(this string apiKey)
    {
        const string bearerPrefix = "Bearer ";

        if (string.IsNullOrWhiteSpace(apiKey))
            return apiKey;

        return apiKey.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
            ? apiKey.Substring(bearerPrefix.Length).Trim()
            : apiKey;
    }
}