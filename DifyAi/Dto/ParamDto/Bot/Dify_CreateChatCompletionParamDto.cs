using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class Dify_CreateChatCompletionParamDto : Dify_BaseRequestParamDto
{
    /// <summary>
    /// User Input/Question content
    /// </summary>
    [Required]
    public string Query { get; set; }

    /// <summary>
    ///     User identifier, used to define the identity of the end-user for retrieval and statistics.
    ///     Should be uniquely defined by the developer within the application.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///     Conversation ID, to continue the conversation based on previous chat records,
    ///     it is necessary to pass the previous message's conversation_id.
    /// </summary>
    [JsonProperty("conversation_id")]
    public string ConversationId { get; set; }

    /// <summary>
    /// Allows the entry of various variable values defined by the App
    /// </summary>
    public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// The mode of response return
    /// </summary>
    /// <see cref="DifyAiResponseModeEnum"/>
    [JsonProperty("response_mode")]
    public string ResponseMode { get; internal set; }

    /// <summary>
    ///     File list, suitable for inputting files (images) combined with text understanding and answering questions,
    ///     available only when the model supports Vision capability.
    /// </summary>
    public List<ChatCompletionFileItemParamDto> Files { get; set; }

    /// <summary>
    ///     (Optional) Auto-generate title, default true. If set to false,
    ///     you can asynchronously generate the title by calling the session
    ///     rename interface and setting auto_generate to true.
    /// </summary>
    [JsonProperty("auto_generate_name")]
    public bool? AutoGenerateName { get; set; } = true;
}

public class ChatCompletionFileItemParamDto
{
    /// <summary>
    ///     Supported type: image (currently only supports image type)
    /// </summary>
    public string Type { get; internal set; } = "image";

    /// <summary>
    ///     Transfer method, remote_url for image URL / local_file for file upload
    /// </summary>
    [JsonProperty("transfer_method ")]
    public string TransferMethod { get; set; }

    /// <summary>
    ///     Image URL (when the transfer method is remote_url)
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    ///     Uploaded file ID, which must be obtained by uploading through the File
    ///     Upload API in advance (when the transfer method is local_file)
    /// </summary>
    [JsonProperty("upload_file_id")]
    public string UploadFileId { get; set; }
}