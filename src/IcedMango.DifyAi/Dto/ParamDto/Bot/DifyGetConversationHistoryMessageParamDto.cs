using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class DifyGetConversationHistoryMessageParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     User identifier, used to define the identity of the end-user for retrieval and statistics.
    ///     Should be uniquely defined by the developer within the application.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///     Conversation ID
    /// </summary>
    [Required]
    [JsonProperty("conversation_id")]
    public string ConversationId { get; set; }

    /// <summary>
    ///     The ID of the first chat record on the current page, default is null.
    /// </summary>
    [JsonIgnore]
    public string FirstId { get; set; }

    /// <summary>
    ///     How many chat history messages to return in one request, default is 20.
    /// </summary>
    public int Limit { get; set; } = 20;
}