using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class DifyCreateFeedbackParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     User identifier, used to define the identity of the end-user for retrieval and statistics.
    ///     Should be uniquely defined by the developer within the application.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///     Upvote as like,
    ///     downvote as dislike,
    ///     revoke upvote as null
    /// </summary>
    public string Rating { get; set; }


    /// <summary>
    ///     Message ID
    /// </summary>
    [JsonIgnore]
    public string MessageId { get; set; }
}