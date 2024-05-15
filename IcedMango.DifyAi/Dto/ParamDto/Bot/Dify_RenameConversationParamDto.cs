using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_RenameConversationParamDto : Dify_BaseRequestParamDto
{
    /// <summary>
    ///     Conversation ID
    /// </summary>
    [JsonIgnore]
    public string ConversationId { get; set; }

    /// <summary>
    ///     The name of the conversation. This parameter can be omitted if auto_generate is set to true.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Automatically generate the title, default is false
    /// </summary>
    [JsonProperty("auto_generate")]
    public bool? AutoGenerate { get; set; }

    /// <summary>
    ///     The user identifier, defined by the developer, must ensure uniqueness within the application.
    /// </summary>
    [Required]
    public string User { get; set; }
}