using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using MimeMapping;
using Newtonsoft.Json;

namespace DifyAi.Request;

public class RequestExtension : IRequestExtension
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RequestExtension> _logger;

    /// <summary>
    ///     Constructor - using HttpClient instance directly
    /// </summary>
    /// <param name="httpClient">Pre-configured HttpClient instance from IHttpClientFactory</param>
    /// <param name="logger">Optional logger</param>
    public RequestExtension(HttpClient httpClient, ILogger<RequestExtension> logger = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger;
    }


    /// <summary>
    ///     Http get
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<DifyApiResult<T>> HttpGet<T>(string url, CancellationToken cancellationToken = default)
    {
        LogRequestStart("GET", url);
        var resp = await _httpClient.GetAsync(url, cancellationToken);
        return await HandelResponse<T>(resp, "GET", url, cancellationToken);
    }

    /// <summary>
    ///     Http delete
    /// </summary>
    /// <param name="url"></param>
    /// <param name="cancellationToken"></param>
    /// <param></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<DifyApiResult<T>> HttpDelete<T>(string url, DifyBaseRequestParamDto paramDto,
        CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
        string requestBody = null;

        if (paramDto != null)
        {
            requestBody = paramDto.ToJson();
            httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        }

        LogRequestStart("DELETE", url, requestBody);
        var resp = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await HandelResponse<T>(resp, "DELETE", url, cancellationToken);
    }

    /// <summary>
    ///     Http post
    /// </summary>
    /// <param name="url"></param>
    /// <param name="paramDto"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<DifyApiResult<T>> HttpPost<T>(string url, DifyBaseRequestParamDto paramDto, CancellationToken cancellationToken = default)
    {
        var requestBody = paramDto.ToJson();
        LogRequestStart("POST", url, requestBody);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var resp = await _httpClient.PostAsync(url, content, cancellationToken);
        return await HandelResponse<T>(resp, "POST", url, cancellationToken);
    }


    public async Task<DifyApiResult<T>> HttpPatch<T>(string url, DifyBaseRequestParamDto paramDto, CancellationToken cancellationToken = default)
    {
        var requestBody = paramDto.ToJson();
        LogRequestStart("PATCH", url, requestBody);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var resp = await _httpClient.PatchAsync(url, content, cancellationToken);
        return await HandelResponse<T>(resp, "PATCH", url, cancellationToken);
    }

    public async Task<DifyApiResult<T>> PostUploadFileAsync<T>(string url, DifyBaseFileRequestParamDto paramDto, CancellationToken cancellationToken = default)
    {
        using var formData = new MultipartFormDataContent();

        foreach (var property in paramDto.GetType().GetProperties())
        {
            var value = property.GetValue(paramDto);
            if (value == null) continue;

            formData.Add(new StringContent(value?.ToString() ?? string.Empty), property.Name);
        }

        // add file last
        if (!File.Exists(paramDto.FilePath))
        {
            throw new DifyInvalidFileException("File not found", paramDto.FilePath);
        }

        var fileType = MimeUtility.GetMimeMapping(paramDto.FilePath);
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(paramDto.FilePath, cancellationToken));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileType);
        formData.Add(fileContent, "file", Path.GetFileName(paramDto.FilePath));

        LogRequestStart("POST (multipart)", url, $"[File: {Path.GetFileName(paramDto.FilePath)}, Type: {fileType}]");
        var resp = await _httpClient.PostAsync(url, formData, cancellationToken);
        return await HandelResponse<T>(resp, "POST", url, cancellationToken);
    }

    public async Task<DifyApiResult<T>> PostUploadDocumentAsync<T>(string url, DifyBaseFileRequestParamDto paramDto, CancellationToken cancellationToken = default)
    {
        using var formData = new MultipartFormDataContent();
        var dataJson = paramDto.ToJson();
        formData.Add(new StringContent(dataJson), "data");


        // add file last
        if (!File.Exists(paramDto.FilePath))
        {
            throw new DifyInvalidFileException("File not found", paramDto.FilePath);
        }

        var fileType = MimeUtility.GetMimeMapping(paramDto.FilePath);
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(paramDto.FilePath, cancellationToken));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileType);
        formData.Add(fileContent, "file", Path.GetFileName(paramDto.FilePath));

        LogRequestStart("POST (document)", url, $"data={dataJson}, [File: {Path.GetFileName(paramDto.FilePath)}, Type: {fileType}]");
        var resp = await _httpClient.PostAsync(url, formData, cancellationToken);
        return await HandelResponse<T>(resp, "POST", url, cancellationToken);
    }

    #region internal method

    /// <summary>
    ///     format response and throw exception if not success
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="method">HTTP method for logging</param>
    /// <param name="url">Request URL for logging</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="DifyApiRequestException"></exception>
    internal async Task<DifyApiResult<T>> HandelResponse<T>(HttpResponseMessage responseMessage, string method, string url, CancellationToken cancellationToken = default)
    {
        var respContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (!responseMessage.IsSuccessStatusCode)
        {
            try
            {
                var error = JsonConvert.DeserializeObject<DifyBaseErrorResDto>(respContent);

                _logger?.LogError(
                    "[DifyApi] {Method} {Url} FAILED\n    Status: {HttpStatusCode}\n    ErrorCode: {ErrorCode}\n    Message: {ErrorMessage}\n    Response: {RawResponse}",
                    method,
                    url,
                    (int)responseMessage.StatusCode,
                    error?.Code,
                    error?.Message,
                    respContent);

                return new DifyApiResult<T>
                {
                    Code = error?.Code,
                    Success = false,
                    Message = error?.Message
                };
            }
            catch (JsonReaderException ex)
            {
                _logger?.LogError(ex,
                    "[DifyApi] {Method} {Url} FAILED (Invalid JSON)\n    Status: {HttpStatusCode}\n    Response: {RawResponse}",
                    method,
                    url,
                    (int)responseMessage.StatusCode,
                    respContent);

                return new DifyApiResult<T>
                {
                    Success = false,
                    Message = $"DifyApi request failed. Response is not valid JSON: {ex.Message}"
                };
            }
        }

        // Log successful response
        LogResponseSuccess(method, url, (int)responseMessage.StatusCode, respContent);

        // Handle 204 No Content or empty response body
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent ||
            string.IsNullOrWhiteSpace(respContent))
        {
            return new DifyApiResult<T>
            {
                Success = true,
                Data = default
            };
        }

        // Handle simple numeric responses (e.g., "204" for successful deletion)
        // These cannot be deserialized into complex DTOs
        if (int.TryParse(respContent.Trim(), out _))
        {
            return new DifyApiResult<T>
            {
                Success = true,
                Data = default
            };
        }

        var obj = JsonConvert.DeserializeObject<T>(respContent);

        return new DifyApiResult<T>
        {
            Success = true,
            Data = obj
        };
    }

    /// <summary>
    /// Log request start with optional body
    /// </summary>
    private void LogRequestStart(string method, string url, string requestBody = null)
    {
        if (string.IsNullOrEmpty(requestBody))
        {
            _logger?.LogDebug(
                "[DifyApi] >>> {Method} {Url}",
                method, url);
        }
        else
        {
            _logger?.LogDebug(
                "[DifyApi] >>> {Method} {Url}\n    Body: {RequestBody}",
                method, url, requestBody);
        }
    }

    /// <summary>
    /// Log successful response
    /// </summary>
    private void LogResponseSuccess(string method, string url, int statusCode, string responseBody)
    {
        // Truncate very long responses for readability
        var truncatedResponse = responseBody.Length > 2000
            ? responseBody[..2000] + "... [TRUNCATED]"
            : responseBody;

        _logger?.LogDebug(
            "[DifyApi] <<< {Method} {Url} ({StatusCode})\n    Response: {ResponseBody}",
            method, url, statusCode, truncatedResponse);
    }

    /// <summary>
    /// Mask authorization header for logging
    /// </summary>
    private static string MaskAuthHeader(string authHeader)
    {
        if (string.IsNullOrWhiteSpace(authHeader))
            return "[EMPTY]";

        // Format: "Bearer xxx..."
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) && authHeader.Length > 15)
        {
            var token = authHeader.Substring(7);
            if (token.Length > 12)
                return $"Bearer {token[..8]}...{token[^4..]}";
        }

        return authHeader.Length > 15 ? $"{authHeader[..12]}..." : authHeader;
    }

    #endregion
}