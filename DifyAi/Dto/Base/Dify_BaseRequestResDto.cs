using Newtonsoft.Json;

namespace DifyAi.Dto.Base;

public class Dify_BaseRequestResDto<T> : Dify_BaseRequestResDto where T : class
{
    public T Data { get; set; }
}

public class Dify_BaseRequestResDto
{
    public int Limit { get; set; }

    public string Result { get; set; }

    [JsonProperty("has_more")] public bool HasMore { get; set; }
}