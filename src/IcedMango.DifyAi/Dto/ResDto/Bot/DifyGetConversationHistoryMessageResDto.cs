using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class DifyGetConversationHistoryMessageResDto :
    DifyBaseRequestResDto<List<DifyGetConversationHistoryMessageResDto_MessageItem>>
{
}

public class DifyGetConversationHistoryMessageResDto_MessageItem
{
    /// <summary>
    ///     Message ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    ///     Conversation ID
    /// </summary>
    [JsonProperty("conversation_id")]
    public string ConversationId { get; set; }

    /// <summary>
    ///     User input parameters.
    /// </summary>
    [JsonProperty("inputs")]
    public Dictionary<string, string> Inputs { get; set; }


    /// <summary>
    ///     User input / question content.
    /// </summary>
    public string Query { get; set; }

    /// <summary>
    ///     Response message content
    /// </summary>
    public string Answer { get; set; }

    /// <summary>
    ///     Message files
    /// </summary>
    [JsonProperty("message_files")]
    public List<DifyGetConversationHistoryMessageResDto_MessageFileItem> MessageFiles { get; set; }

    /// <summary>
    ///     Citation and Attribution List
    /// </summary>
    [JsonProperty("feedback")]
    public DifyGetConversationHistoryMessageResDto_FeedbackItem Feedback { get; set; }

    /// <summary>
    /// </summary>
    [JsonProperty("retriever_resources")]
    public List<DifyRetrieverResource> RetrieverResources { get; set; }

    [JsonProperty("created_at")] [JsonConverter(typeof(UnixTimestampConverter))] public DateTime? CreatedAt { get; set; }
}

public class DifyGetConversationHistoryMessageResDto_MessageFileItem
{
    /// <summary>
    ///     ID
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     File type, image for images
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///     Preview image URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    ///     belongs to，user or assistant
    /// </summary>
    [JsonProperty("belongs_to")]
    public string BelongsTo { get; set; }
}

public class DifyGetConversationHistoryMessageResDto_FeedbackItem
{
    /// <summary>
    ///     Upvote as like / Downvote as dislike
    /// </summary>
    public string Rating { get; set; }
}