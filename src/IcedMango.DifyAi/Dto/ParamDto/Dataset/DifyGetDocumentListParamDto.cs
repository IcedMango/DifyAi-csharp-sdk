namespace DifyAi.Dto.ParamDto;

public class DifyGetDocumentListParamDto
{
    /// <summary>
    ///     Knowledge ID
    /// </summary>
    public string DatasetId { get; set; }

    /// <summary>
    ///     Search keywords, currently only search document names(optional)
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    ///     Page number(optional)
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    ///     Number of items returned, default 20, range 1-100(optional)
    /// </summary>
    public int? Limit { get; set; } = 20;
}