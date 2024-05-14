using System.Text;

namespace DifyAi.Services;

/// <summary>
///     DifyAi chat services
/// </summary>
public class DifyAiChatServices : IDifyAiChatServices
{
    private readonly IRequestExtension _requestExtension;

    public DifyAiChatServices(IRequestExtension requestExtension)
    {
        _requestExtension = requestExtension;
    }

    #region bot api

    #region completion

    /// <summary>
    ///     Create chat completion(block mode)
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey">pass this to override config api key</param>
    public async Task<DifyApiResult<Dify_CreateChatCompletionResDto>> CreateChatCompletionBlockModeAsync(
        Dify_CreateChatCompletionParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default)
    {
        paramDto.ResponseMode = "blocking";

        var res = await _requestExtension.HttpPost<Dify_CreateChatCompletionResDto>(
            "chat-messages",
            paramDto,
            overrideApiKey,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Stop chat completion
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_StopChatCompletionResDto>> StopChatCompletionAsync(
        Dify_StopChatCompletionParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<Dify_StopChatCompletionResDto>(
            $"chat-messages/{paramDto.TaskId}/stop",
            paramDto,
            overrideApiKey,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Create chat completion(block mode)
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey">pass this to override config api key</param>
    public async Task<Dify_CreateChatCompletionResDto> CreateChatCompletionStreamModeAsync(
        Dify_CreateChatCompletionParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default)
    {
        //TODO to be done
        throw new NotImplementedException();
    }

    #endregion


    /// <summary>
    ///     Message Feedback
    ///     End-users can provide feedback messages,
    ///     facilitating application developers to optimize expected outputs.
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_CreateFeedbackResDto>> CreateFeedbackAsync(
        Dify_CreateFeedbackParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<Dify_CreateFeedbackResDto>(
            $"messages/{paramDto.MessageId}/feedbacks",
            paramDto,
            overrideApiKey,
            cancellationToken);
        return res;
    }


    /// <summary>
    ///     Get next questions suggestions for the current message
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_BaseRequestResDto<List<string>>>> GetSuggestionsAsync(string messageId,
        string overrideApiKey = "", CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<Dify_BaseRequestResDto<List<string>>>(
            $"messages/{messageId}/suggested",
            overrideApiKey,
            cancellationToken);

        return res;
    }

    #region Conversation

    /// <summary>
    ///     Get conversation history message
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_GetConversationHistoryMessageResDto>> GetConversationHistoryMessageAsync(
        Dify_GetConversationHistoryMessageParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default)
    {
        var url = new StringBuilder(
            $"messages?conversation_id={paramDto.ConversationId}&user={paramDto.User}&limit={paramDto.Limit}");

        if (!string.IsNullOrWhiteSpace(paramDto.FirstId)) url.Append($"&first_id={paramDto.FirstId}");

        var res = await _requestExtension.HttpGet<Dify_GetConversationHistoryMessageResDto>(
            url.ToString(),
            overrideApiKey,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get conversation list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_GetConversationListResDto>> GetConversationListAsync(
        Dify_GetConversationListParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var url = new StringBuilder($"conversations?user={paramDto.User}&limit={paramDto.Limit}");

        if (!string.IsNullOrWhiteSpace(paramDto.LastId)) url.Append($"&last_id={paramDto.LastId}");
        if (paramDto.Pinned != null) url.Append($"&pinned={paramDto.Pinned}");

        var res = await _requestExtension.HttpGet<Dify_GetConversationListResDto>(
            url.ToString(),
            overrideApiKey,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Delete a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task DeleteConversationAsync(
        Dify_DeleteConversationParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        await _requestExtension.HttpDelete<Dify_GetConversationListResDto>(
            $"conversations/{paramDto.ConversationId}",
            paramDto,
            overrideApiKey,
            cancellationToken);
    }

    /// <summary>
    ///     Rename a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<Dify_RenameConversationResDto>> RenameConversationAsync(
        Dify_RenameConversationParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<Dify_RenameConversationResDto>(
            $"conversations/{paramDto.ConversationId}/name",
            paramDto,
            overrideApiKey,
            cancellationToken);

        return res;
    }

    #endregion


    /// <summary>
    ///     Audio To Text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<Dify_AudioToTextResDto>> AudioToTextAsync(Dify_AudioToTextParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        //validate file format
        var list = new List<string> { "mp3", "mp4", "mpeg", "mpga", "m4a", "wav", "webm" };

        if (!list.Contains(paramDto.FilePath.Split('.').Last()))
        {
            throw new Exception("File format not supported");
        }


        var res = await _requestExtension.PostFileAsync<Dify_AudioToTextResDto>(
            "audio-to-text",
            paramDto,
            overrideApiKey,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get application info
    /// </summary>
    /// <param name="user"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<Dify_GetApplicationInfoResDto>> GetApplicationInfoAsync(string user,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<Dify_GetApplicationInfoResDto>(
            $"parameters?user={user}",
            overrideApiKey,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get application meta
    /// </summary>
    /// <param name="user"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<Dify_GetApplicationMetaResDto>> GetApplicationMetaAsync(string user,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<Dify_GetApplicationMetaResDto>(
            $"meta?user={user}",
            overrideApiKey,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     File upload
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<Dify_FileUploadResDto>> FileUploadAsync(Dify_FileUploadParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.PostFileAsync<Dify_FileUploadResDto>(
            "files",
            paramDto,
            overrideApiKey,
            cancellationToken);
        return res;
    }

    #endregion
}