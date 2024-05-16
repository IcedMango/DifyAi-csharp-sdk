using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_CreateDocumentByFileParamDto : Dify_BaseFileRequestParamDto
{
    /// <summary>
    ///     Knowledge ID
    /// </summary>
    [JsonIgnore]
    public string DatasetId { get; set; }

    /// <summary>
    ///     Document name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Source document ID (optional)
    ///     Used to re-upload the document or modify the document cleaning and segmentation configuration. The missing information is copied from the source document
    ///     The source document cannot be an archived document
    ///     When original_document_id is passed in, the update operation is performed on behalf of the document. process_rule is a fillable item. If not filled in, the segmentation method of the source document will be used by defaul
    ///     When original_document_id is not passed in, the new operation is performed on behalf of the document, and process_rule is required
    /// </summary>
    [JsonProperty("original_document_id")]
    public string OriginalDocumentId { get; set; }

   
    /// <summary>
    ///     segmentation mode(true : automatic, false : custom)
    /// </summary>
    public bool? IsAutomaticProcess { get; set; }

    /// <summary>
    ///     Index mode: true: high_quality, false: economy
    ///     (AutomaticProcess will ignore this field)
    /// </summary>
    /// <returns></returns>
    public bool? EnableHighQualityIndex { get; set; } = true;

    /// <summary>
    ///     Replace consecutive spaces, newlines, tabs
    ///     (AutomaticProcess will ignore this field)
    /// </summary>
    public bool? RemoveExtraSpaces { get; set; }

    /// <summary>
    ///     Delete URL, email address
    ///     (AutomaticProcess will ignore this field)
    /// </summary>
    public bool? RemoveUrlsEmails { get; set; }

    /// <summary>
    ///     Custom segment identifier, currently only allows one delimiter to be set. Default is \n
    ///     (AutomaticProcess will ignore this field)
    /// </summary>
    public string Separator { get; set; } = "\n";

    /// <summary>
    ///     Maximum length (token) defaults to 1000
    ///     (AutomaticProcess will ignore this field)
    /// </summary>
    public int? MaxTokens { get; set; } = 1000;
}