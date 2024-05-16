using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DifyAi.Request;

public class RequestExtension : IRequestExtension
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RequestExtension> _logger;

    public RequestExtension(IHttpClientFactory httpClientFactory,
        ILogger<RequestExtension> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    #region internal method

    /// <summary>
    ///     Format api key
    /// </summary>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    internal string FormatApiKey(string apiKey)
    {
        if (apiKey.StartsWith("Bearer ")) apiKey = apiKey.Replace("Bearer ", "");
        return apiKey;
    }

    /// <summary>
    ///     format response and throw exception if not success
    /// </summary>
    /// <param name="responseMessage"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="DifyApiRequestException"></exception>
    internal async Task<DifyApiResult<T>> HandelResponse<T>(HttpResponseMessage responseMessage,
        CancellationToken cancellationToken = default)
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            var respContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                var error = JsonConvert.DeserializeObject<Dify_BaseErrorResDto>(respContent);

                _logger.LogError(
                    $"DifyApi Request Failed! StatusCode: {error?.Code} Message: {error?.Message} \n response: {respContent}");

                return new DifyApiResult<T>()
                {
                    Code = error?.Code,
                    Success = false,
                    Message = error?.Message
                };
            }
            catch (Exception e)
            {
                if (e is JsonReaderException)
                {
                    return new DifyApiResult<T>()
                    {
                        Success = false,
                        Message = "DifyApi Request Failed! Response is not a valid json. Message: " + e.Message
                    };
                }

                throw;
            }
        }

        var resContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        var obj = JsonConvert.DeserializeObject<T>(resContent);

        return new DifyApiResult<T>()
        {
            Success = true,
            Data = obj
        };
    }

    #endregion


    /// <summary>
    ///     Http get
    /// </summary>
    /// <param name="url"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="httpClientName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<DifyApiResult<T>> HttpGet<T>(string url, string overrideApiKey,
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot")
    {
        using var client = _httpClientFactory.CreateClient(httpClientName);

        if (!string.IsNullOrEmpty(overrideApiKey))
        {
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {FormatApiKey(overrideApiKey)}");
        }

        var resp = await client.GetAsync(url, cancellationToken);
        return await HandelResponse<T>(resp, cancellationToken);
    }

    /// <summary>
    ///     Http delete
    /// </summary>
    /// <param name="url"></param>
    /// <param name="overrideApiKey"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="httpClientName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<DifyApiResult<T>> HttpDelete<T>(string url, Dify_BaseRequestParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot")
    {
        using var client = _httpClientFactory.CreateClient(httpClientName);

        if (!string.IsNullOrEmpty(overrideApiKey))
        {
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {FormatApiKey(overrideApiKey)}");
        }

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

        if (paramDto != null)
        {
            httpRequestMessage.Content = new StringContent(paramDto.ToJson(), Encoding.UTF8, "application/json");
        }

        var resp = await client.SendAsync(httpRequestMessage, cancellationToken);

        return await HandelResponse<T>(resp, cancellationToken);
    }

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
    public async Task<DifyApiResult<T>> HttpPost<T>(string url, Dify_BaseRequestParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot")
    {
        using var client = _httpClientFactory.CreateClient(httpClientName);

        if (!string.IsNullOrEmpty(overrideApiKey))
        {
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {FormatApiKey(overrideApiKey)}");
        }

        var content = new StringContent(paramDto.ToJson(), Encoding.UTF8, "application/json");

        var resp = await client.PostAsync(url, content, cancellationToken);
        return await HandelResponse<T>(resp, cancellationToken);
    }


    public async Task<DifyApiResult<T>> PostUploadFileAsync<T>(string url, Dify_BaseFileRequestParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot")
    {
        using var client = _httpClientFactory.CreateClient(httpClientName);

        if (!string.IsNullOrEmpty(overrideApiKey))
        {
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {FormatApiKey(overrideApiKey)}");
        }

        using var formData = new MultipartFormDataContent();

        foreach (var property in paramDto.GetType().GetProperties())
        {
            var value = property.GetValue(paramDto);
            if (value == null) continue;

            formData.Add(new StringContent(value.ToString()), property.Name);
        }

        // add file last
        if (!File.Exists(paramDto.FilePath))
        {
            throw new FileNotFoundException("File not found", paramDto.FilePath);
        }

        var fileType = MimeMapping.MimeUtility.GetMimeMapping(paramDto.FilePath);
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(paramDto.FilePath, cancellationToken));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileType);
        formData.Add(fileContent, "file", Path.GetFileName(paramDto.FilePath));


        var resp = await client.PostAsync(url, formData, cancellationToken);
        return await HandelResponse<T>(resp, cancellationToken);
    }

    public async Task<DifyApiResult<T>> PostUploadDocumentAsync<T>(string url, Dify_BaseFileRequestParamDto paramDto,
        string overrideApiKey = "",
        CancellationToken cancellationToken = default,
        string httpClientName = "DifyAi.Bot")
    {
        using var client = _httpClientFactory.CreateClient(httpClientName);

        if (!string.IsNullOrEmpty(overrideApiKey))
        {
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {FormatApiKey(overrideApiKey)}");
        }

        using var formData = new MultipartFormDataContent();


        formData.Add(new StringContent(paramDto.ToJson()), "data");


        // add file last
        if (!File.Exists(paramDto.FilePath))
        {
            throw new FileNotFoundException("File not found", paramDto.FilePath);
        }

        var fileType = MimeMapping.MimeUtility.GetMimeMapping(paramDto.FilePath);
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(paramDto.FilePath, cancellationToken));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileType);
        formData.Add(fileContent, "file", Path.GetFileName(paramDto.FilePath));


        var resp = await client.PostAsync(url, formData, cancellationToken);
        return await HandelResponse<T>(resp, cancellationToken);
    }
}