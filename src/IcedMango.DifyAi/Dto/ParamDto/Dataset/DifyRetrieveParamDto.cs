namespace DifyAi.Dto.ParamDto;

/// <summary>
///     Parameters for knowledge base retrieval
/// </summary>
public class DifyRetrieveParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     Dataset ID
    /// </summary>
    public string DatasetId { get; set; }

    /// <summary>
    ///     Query text
    /// </summary>
    public string Query { get; set; }

    /// <summary>
    ///     Retrieval mode: "multiple", "single" or "hybrid"
    /// </summary>
    public string RetrievalMode { get; set; } = "multiple";

    /// <summary>
    ///     Top K results to return (default: 3)
    /// </summary>
    public int? TopK { get; set; } = 3;

    /// <summary>
    ///     Score threshold (0.0 - 1.0)
    /// </summary>
    public double? ScoreThreshold { get; set; }
}