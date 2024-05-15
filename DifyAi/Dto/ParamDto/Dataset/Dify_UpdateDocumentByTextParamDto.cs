using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_UpdateDocumentByTextParamDto : Dify_BaseRequestParamDto
{
    public string DatasetId { get; set; }

    public string DocumentId { get; set; }

    /// <summary>
    /// Document name (optional)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Document content (optional)
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Preprocessing rules
    /// </summary>
    [JsonProperty("process_rule")]
    public List<Dify_Dataset_ProcessRule> ProcessRule { get; set; }
}