using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_CreateDocumentByTextParamDto : Dify_BaseRequestParamDto
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
    ///     text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     segmentation mode(true : automatic, false : custom)
    /// </summary>
    public bool? IsAutomaticProcess { get; set; }

    /// <summary>
    ///     Index mode: true: high_quality, false: economy
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