using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_StopChatCompletionParamDto : Dify_BaseRequestParamDto
{
    /// <summary>
    ///     User identifier, used to define the identity of the end-user for retrieval and statistics.
    ///     Should be uniquely defined by the developer within the application.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///    Task ID, can be obtained from the streaming chunk return
    /// </summary>
    [JsonIgnore]
    public string TaskId { get; set; }
}