using Newtonsoft.Json;

namespace DifyAi.Dto.ApiParamDto;

public class DifyApi_CreateDocumentByFileParam : Dify_BaseFileRequestParamDto
{
    /// <summary>
    ///     Source document ID (optional)
    ///
    ///     Used to re-upload the document or modify the document cleaning and segmentation configuration.
    ///     The missing information is copied from the source document
    ///     The source document cannot be an archived document
    ///     When original_document_id is passed in, the update operation is performed on behalf of the document.
    /// process_rule is a fillable item. If not filled in, the segmentation method of the source document will be used by defaul
    /// </summary>
    [JsonProperty("original_document_id")]
    public string OriginalDocumentId { get; set; }
    
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
    public Dify_Dataset_ProcessRule ProcessRule { get; set; }
}