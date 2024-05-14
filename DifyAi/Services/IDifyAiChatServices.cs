namespace DifyAi.Services;

public interface IDifyAiChatServices
{
    /// <summary>
    ///     Create chat completion(block mode)
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey">pass this to override config api key</param>
    Task<DifyApiResult<Dify_CreateChatCompletionResDto>> CreateChatCompletionBlockModeAsync(
        Dify_CreateChatCompletionParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default);

    /// <summary>
    ///     Stop chat completion
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_StopChatCompletionResDto>> StopChatCompletionAsync(Dify_StopChatCompletionParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create chat completion(block mode)
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey">pass this to override config api key</param>
    Task<Dify_CreateChatCompletionResDto> CreateChatCompletionStreamModeAsync(
        Dify_CreateChatCompletionParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default);

    /// <summary>
    ///     Message Feedback
    ///     End-users can provide feedback messages,
    ///     facilitating application developers to optimize expected outputs.
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_CreateFeedbackResDto>> CreateFeedbackAsync(Dify_CreateFeedbackParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get next questions suggestions for the current message
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_BaseRequestResDto<List<string>>>> GetSuggestionsAsync(string messageId,
        string overrideApiKey = "", CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get conversation history message
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_GetConversationHistoryMessageResDto>> GetConversationHistoryMessageAsync(
        Dify_GetConversationHistoryMessageParamDto paramDto,
        string overrideApiKey = "", CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get conversation list
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_GetConversationListResDto>> GetConversationListAsync(
        Dify_GetConversationListParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task DeleteConversationAsync(
        Dify_DeleteConversationParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Rename a conversation
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    Task<DifyApiResult<Dify_RenameConversationResDto>> RenameConversationAsync(
        Dify_RenameConversationParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Audio To Text
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<Dify_AudioToTextResDto>> AudioToTextAsync(Dify_AudioToTextParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get application info
    /// </summary>
    /// <param name="user"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<Dify_GetApplicationInfoResDto>> GetApplicationInfoAsync(string user, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get application meta
    /// </summary>
    /// <param name="user"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<Dify_GetApplicationMetaResDto>> GetApplicationMetaAsync(string user, string overrideApiKey = "",
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     File upload
    /// </summary>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DifyApiResult<Dify_FileUploadResDto>> FileUploadAsync(Dify_FileUploadParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default);
}