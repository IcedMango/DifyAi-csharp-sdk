using Newtonsoft.Json;

namespace DifyAi.Dto.ApiParamDto;

public class DifyApiUpdateDocumentByTextParam : DifyBaseRequestParamDto
{
    /// <summary>
    ///     Document name (optional)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Document content (optional)
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Preprocessing rules
    /// </summary>
    [JsonProperty("process_rule")]
    public DifyDatasetProcessRule ProcessRule { get; set; }
}