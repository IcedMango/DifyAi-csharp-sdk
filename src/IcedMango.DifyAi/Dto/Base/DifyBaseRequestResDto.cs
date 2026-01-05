using Newtonsoft.Json;

namespace DifyAi.Dto.Base;

public class DifyBaseRequestResDto<T> : DifyBaseRequestResDto where T : class
{
    public T Data { get; set; }
}

public class DifyBaseRequestListResDto<T>
{
    public T Data { get; set; }
}

public class DifyBaseRequestResDto
{
    public int? Limit { get; set; }

    /// <summary>
    ///     Only dataset return this field
    /// </summary>
    public int? Total { get; set; }

    /// <summary>
    ///     Only dataset return this field
    /// </summary>
    public int? Page { get; set; }

    public string Result { get; set; }

    [JsonProperty("has_more")] public bool? HasMore { get; set; }
    [JsonProperty("doc_form")] public string DocForm { get; set; }
}