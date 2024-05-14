using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DifyAi.Utils;

internal static class ConvertHelper
{
    /// <summary>
    ///     Convert object to json string
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="ignoreNullProperty"></param>
    /// <returns></returns>
    public static string ToJson(this object obj, bool ignoreNullProperty = false)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }
            }
        };

        if (!ignoreNullProperty) return JsonConvert.SerializeObject(obj, settings);

        settings.DefaultValueHandling = DefaultValueHandling.Ignore;
        settings.NullValueHandling = NullValueHandling.Ignore;

        return JsonConvert.SerializeObject(obj, settings);
    }
}