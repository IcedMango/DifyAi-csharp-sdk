namespace DifyAi.Dto.ParamDto;

public class DifyGetConversationListParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     User identifier, used to define the identity of the end-user for retrieval and statistics.
    ///     Should be uniquely defined by the developer within the application.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///     The ID of the last record on the current page, default is null.
    /// </summary>
    public string LastId { get; set; }

    /// <summary>
    ///     How many records to return in one request, default is the most recent 20 entries.
    /// </summary>
    public int Limit { get; set; } = 20;

    /// <summary>
    ///     Return only pinned conversations as true, only non-pinned as false
    /// </summary>
    public bool? Pinned { get; set; }
}