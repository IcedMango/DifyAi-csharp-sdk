namespace DifyAi.Interface;

public interface IDifyAiChatServices
{
    /// <summary>
    ///     Create chat completion(block mode)
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyCreateChatCompletionResDto>> CreateChatCompletionBlockModeAsync(
        DifyCreateChatCompletionParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Stop chat completion
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyStopChatCompletionResDto>> StopChatCompletionAsync(
        DifyStopChatCompletionParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create chat completion(stream mode) - NOT IMPLEMENTED YET
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyCreateChatCompletionResDto> CreateChatCompletionStreamModeAsync(
        DifyCreateChatCompletionParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Message Feedback
    ///     End-users can provide feedback messages,
    ///     facilitating application developers to optimize expected outputs.
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyCreateFeedbackResDto>> CreateFeedbackAsync(
        DifyCreateFeedbackParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get next questions suggestions for the current message
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestResDto<List<string>>>> GetSuggestionsAsync(
        string messageId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get conversation history message
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyGetConversationHistoryMessageResDto>> GetConversationHistoryMessageAsync(
        DifyGetConversationHistoryMessageParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get conversation list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyGetConversationListResDto>> GetConversationListAsync(
        DifyGetConversationListParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyBaseRequestResDto>> DeleteConversationAsync(
        DifyDeleteConversationParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Rename a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<DifyRenameConversationResDto>> RenameConversationAsync(
        DifyRenameConversationParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Audio To Text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyAudioToTextResDto>> AudioToTextAsync(
        DifyAudioToTextParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get application info
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyGetApplicationInfoResDto>> GetApplicationInfoAsync(
        string user,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get application meta
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyGetApplicationMetaResDto>> GetApplicationMetaAsync(
        string user,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     File upload
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyFileUploadResDto>> FileUploadAsync(
        DifyFileUploadParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get conversation variables
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyGetConversationVariablesResDto>> GetConversationVariablesAsync(
        DifyGetConversationVariablesParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get app feedbacks
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyGetAppFeedbacksResDto>> GetAppFeedbacksAsync(
        DifyGetAppFeedbacksParamDto paramDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get file preview
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyGetFilePreviewResDto>> GetFilePreviewAsync(
        string fileId,
        string user,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Text to audio conversion
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<DifyTextToAudioResDto>> TextToAudioAsync(
        DifyTextToAudioParamDto paramDto,
        CancellationToken cancellationToken = default);
}