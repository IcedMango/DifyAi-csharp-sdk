namespace DifyAi.Request;

public interface IRequestExtension
{
    /// <summary>
    ///     Http get
    /// </summary>
    /// <param name="url"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="httpClientName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<DifyApiResult<T>> HttpGet<T>(string url, string overrideApiKey, CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot");

    /// <summary>
    ///     Http Delete
    /// </summary>
    /// <param name="url"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="httpClientName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<DifyApiResult<T>> HttpDelete<T>(string url, Dify_BaseRequestParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot");

    /// <summary>
    ///     Http post
    /// </summary>
    /// <param name="url"></param>
    /// <param name="paramDto"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="httpClientName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<DifyApiResult<T>> HttpPost<T>(string url, Dify_BaseRequestParamDto paramDto, string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot");

    Task<DifyApiResult<T>> PostUploadFileAsync<T>(string url, Dify_BaseFileRequestParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot");

    Task<DifyApiResult<T>> PostUploadDocumentAsync<T>(string url, Dify_BaseFileRequestParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot");
}