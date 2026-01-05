using Newtonsoft.Json;

namespace DifyAi.Dto.ApiParamDto;

public class DifyApiCreateDocumentByTextParam : DifyBaseRequestParamDto
{
    /// <summary>
    ///     Document name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     text
    /// </summary>
    public string Text { get; set; }

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
    public DifyDatasetProcessRule ProcessRule { get; set; }
}