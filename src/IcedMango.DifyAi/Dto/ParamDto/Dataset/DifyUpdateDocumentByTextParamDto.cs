namespace DifyAi.Dto.ParamDto;

public class DifyUpdateDocumentByTextParamDto
{
    public string DatasetId { get; set; }

    public string DocumentId { get; set; }

    /// <summary>
    ///     Document name (optional)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Document content (optional)
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     segmentation mode(true : automatic, false : custom)
    /// </summary>
    public bool? IsAutomaticProcess { get; set; }

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