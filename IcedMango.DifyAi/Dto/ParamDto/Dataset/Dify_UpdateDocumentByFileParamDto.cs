using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_UpdateDocumentByFileParamDto : Dify_BaseFileRequestParamDto
{
    /// <summary>
    ///     Knowledge ID
    /// </summary>
    public string DatasetId { get; set; }

    /// <summary>
    ///     Document ID
    /// </summary>
    public string DocumentId { get; set; }
    
    /// <summary>
    ///     
    /// </summary>
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