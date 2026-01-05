namespace DifyAi.Request;

public interface IRequestExtension
{
    /// <summary>
    ///     Http get
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<DifyApiResult<T>> HttpGet<T>(string url, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Http Delete
    /// </summary>
    /// <param name="url"></param>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<DifyApiResult<T>> HttpDelete<T>(string url, DifyBaseRequestParamDto paramDto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Http post
    /// </summary>
    /// <param name="url"></param>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<DifyApiResult<T>> HttpPost<T>(string url, DifyBaseRequestParamDto paramDto, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Http patch
    /// </summary>
    /// <param name="url"></param>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<DifyApiResult<T>> HttpPatch<T>(string url, DifyBaseRequestParamDto paramDto, CancellationToken cancellationToken = default);

    Task<DifyApiResult<T>> PostUploadFileAsync<T>(string url, DifyBaseFileRequestParamDto paramDto, CancellationToken cancellationToken = default);

    Task<DifyApiResult<T>> PostUploadDocumentAsync<T>(string url, DifyBaseFileRequestParamDto paramDto, CancellationToken cancellationToken = default);
}