namespace DifyAi.Dto.ParamDto;

public class DifyGetDocumentSegmentParamDto
{
    public string DatasetId { get; set; }

    public string DocumentId { get; set; }

    public string Keyword { get; set; }

    public string Status { get; set; } = "completed";

    /// <summary>
    ///     Page number, default 1
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    ///     Number of items per page, default 20, max 100
    /// </summary>
    public int Limit { get; set; } = 20;
}