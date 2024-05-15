using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_DeleteConversationParamDto : Dify_BaseRequestParamDto
{
    /// <summary>
    ///     Conversation ID
    /// </summary>
    [JsonIgnore]
    public string ConversationId { get; set; }

    /// <summary>
    ///     User identifier, used to define the identity of the end-user for retrieval and statistics.
    ///     Should be uniquely defined by the developer within the application.
    /// </summary>
    [Required]
    public string User { get; set; }
}