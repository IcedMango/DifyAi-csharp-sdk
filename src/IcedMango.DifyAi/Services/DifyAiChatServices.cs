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
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyCreateChatCompletionResDto>> CreateChatCompletionBlockModeAsync(
        DifyCreateChatCompletionParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        paramDto.ResponseMode = "blocking";

        var res = await _requestExtension.HttpPost<DifyCreateChatCompletionResDto>(
            "chat-messages",
            paramDto,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Stop chat completion
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyStopChatCompletionResDto>> StopChatCompletionAsync(
        DifyStopChatCompletionParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifyStopChatCompletionResDto>(
            $"chat-messages/{paramDto.TaskId}/stop",
            paramDto,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Create chat completion(stream mode) - NOT IMPLEMENTED YET
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task<DifyCreateChatCompletionResDto> CreateChatCompletionStreamModeAsync(
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        DifyCreateChatCompletionParamDto paramDto,
        CancellationToken cancellationToken = default)
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
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyCreateFeedbackResDto>> CreateFeedbackAsync(
        DifyCreateFeedbackParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifyCreateFeedbackResDto>(
            $"messages/{paramDto.MessageId}/feedbacks",
            paramDto,
            cancellationToken);
        return res;
    }


    /// <summary>
    ///     Get next questions suggestions for the current message
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyBaseRequestResDto<List<string>>>> GetSuggestionsAsync(string messageId,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyBaseRequestResDto<List<string>>>(
            $"messages/{messageId}/suggested",
            cancellationToken);

        return res;
    }

    #region Conversation

    /// <summary>
    ///     Get conversation history message
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyGetConversationHistoryMessageResDto>> GetConversationHistoryMessageAsync(
        DifyGetConversationHistoryMessageParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var url = new StringBuilder(
            $"messages?conversation_id={paramDto.ConversationId}&user={paramDto.User}&limit={paramDto.Limit}");

        if (!string.IsNullOrWhiteSpace(paramDto.FirstId)) url.Append($"&first_id={paramDto.FirstId}");

        var res = await _requestExtension.HttpGet<DifyGetConversationHistoryMessageResDto>(
            url.ToString(),
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get conversation list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyGetConversationListResDto>> GetConversationListAsync(
        DifyGetConversationListParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var url = new StringBuilder($"conversations?user={paramDto.User}&limit={paramDto.Limit}");

        if (!string.IsNullOrWhiteSpace(paramDto.LastId)) url.Append($"&last_id={paramDto.LastId}");
        if (paramDto.Pinned != null) url.Append($"&pinned={paramDto.Pinned}");

        var res = await _requestExtension.HttpGet<DifyGetConversationListResDto>(
            url.ToString(),
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Delete a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyBaseRequestResDto>> DeleteConversationAsync(
        DifyDeleteConversationParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpDelete<DifyBaseRequestResDto>(
            $"conversations/{paramDto.ConversationId}",
            paramDto,
            cancellationToken);
        return res;
    }

    /// <summary>
    ///     Rename a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<DifyApiResult<DifyRenameConversationResDto>> RenameConversationAsync(
        DifyRenameConversationParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifyRenameConversationResDto>(
            $"conversations/{paramDto.ConversationId}/name",
            paramDto,
            cancellationToken);

        return res;
    }

    #endregion


    /// <summary>
    ///     Audio To Text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyAudioToTextResDto>> AudioToTextAsync(DifyAudioToTextParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        // Validate file format
        var supportedFormats = new[] { "mp3", "mp4", "mpeg", "mpga", "m4a", "wav", "webm" };
        var fileExtension = paramDto.FilePath.Split('.').Last().ToLowerInvariant();

        if (!supportedFormats.Contains(fileExtension))
        {
            throw new DifyInvalidFileException(
                $"File format '.{fileExtension}' is not supported for audio-to-text conversion",
                paramDto.FilePath,
                supportedFormats);
        }

        var res = await _requestExtension.PostUploadFileAsync<DifyAudioToTextResDto>(
            "audio-to-text",
            paramDto,
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get application info
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyGetApplicationInfoResDto>> GetApplicationInfoAsync(string user,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyGetApplicationInfoResDto>(
            $"parameters?user={user}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get application meta
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyGetApplicationMetaResDto>> GetApplicationMetaAsync(string user,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyGetApplicationMetaResDto>(
            $"meta?user={user}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     File upload
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyFileUploadResDto>> FileUploadAsync(DifyFileUploadParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.PostUploadFileAsync<DifyFileUploadResDto>(
            "files/upload",
            paramDto,
            cancellationToken);
        return res;
    }

    /// <summary>
    ///     Get conversation variables
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyGetConversationVariablesResDto>> GetConversationVariablesAsync(
        DifyGetConversationVariablesParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var url = new StringBuilder(
            $"conversations/{paramDto.ConversationId}/variables?user={paramDto.User}&limit={paramDto.Limit}");

        if (!string.IsNullOrWhiteSpace(paramDto.LastId))
        {
            url.Append($"&last_id={paramDto.LastId}");
        }

        if (!string.IsNullOrWhiteSpace(paramDto.VariableName))
        {
            url.Append($"&variable_name={paramDto.VariableName}");
        }

        var res = await _requestExtension.HttpGet<DifyGetConversationVariablesResDto>(
            url.ToString(),
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get app feedbacks
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyGetAppFeedbacksResDto>> GetAppFeedbacksAsync(
        DifyGetAppFeedbacksParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var url = new StringBuilder($"app/feedbacks?user={paramDto.User}&page={paramDto.Page}&limit={paramDto.Limit}");

        if (!string.IsNullOrWhiteSpace(paramDto.Rating))
        {
            url.Append($"&rating={paramDto.Rating}");
        }

        if (!string.IsNullOrWhiteSpace(paramDto.Keyword))
        {
            url.Append($"&keyword={paramDto.Keyword}");
        }

        var res = await _requestExtension.HttpGet<DifyGetAppFeedbacksResDto>(
            url.ToString(),
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Get file preview
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyGetFilePreviewResDto>> GetFilePreviewAsync(
        string fileId,
        string user,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpGet<DifyGetFilePreviewResDto>(
            $"files/{fileId}/preview?user={user}",
            cancellationToken);

        return res;
    }

    /// <summary>
    ///     Text to audio conversion
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<DifyApiResult<DifyTextToAudioResDto>> TextToAudioAsync(
        DifyTextToAudioParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var res = await _requestExtension.HttpPost<DifyTextToAudioResDto>(
            "text-to-audio",
            paramDto,
            cancellationToken);

        return res;
    }

    #endregion
}