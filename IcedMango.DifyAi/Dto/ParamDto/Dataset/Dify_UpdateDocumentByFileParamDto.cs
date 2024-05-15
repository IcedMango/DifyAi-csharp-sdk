using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_UpdateDocumentByFileParamDto : Dify_BaseFileRequestParamDto
{
    /// <summary>
    ///     Knowledge ID
    /// </summary>
    [JsonIgnore]
    public string DatasetId { get; set; }

    /// <summary>
    ///     Document ID
    /// </summary>
    [JsonIgnore]
    public string DocumentId { get; set; }

    /// <summary>
    ///     Document name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Index mode
    ///     high_quality : embedding using embedding model, built as vector database index
    ///     economy : Build using inverted index of Keyword Table Index
    /// </summary>
    /// <returns></returns>
    [JsonProperty("indexing_technique")]
    public string IndexingTechnique { get; set; }

    /// <summary>
    ///     Preprocessing rules
    /// </summary>
    [JsonProperty("process_rule")]
    public List<Dify_Dataset_ProcessRule> ProcessRule { get; set; }
}