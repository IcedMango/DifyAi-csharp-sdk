namespace DifyAi.Dto.ParamDto;

/// <summary>
///     Parameters for getting app feedbacks
/// </summary>
public class DifyGetAppFeedbacksParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     User identifier
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///     Rating filter: "like" or "dislike" (optional)
    /// </summary>
    public string Rating { get; set; }

    /// <summary>
    ///     Keyword search (optional)
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    ///     Page number for pagination (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    ///     Maximum number of items to return (range: 1-100, default: 20)
    /// </summary>
    public int Limit { get; set; } = 20;
}