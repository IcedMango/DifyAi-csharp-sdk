namespace DifyAi.InternalException;

/// <summary>
///     Dify SDK business exception for API error responses
/// </summary>
public class DifySDKException : Exception
{
    public DifySDKException(string message) : base(message)
    {
    }

    public DifySDKException(string message, Exception inner) : base(message, inner)
    {
    }

    public DifySDKException(string message, int? statusCode, string errorCode, string rawResponse = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        RawResponse = rawResponse;
    }

    public DifySDKException(string message, int? statusCode, string errorCode, string rawResponse, Exception inner)
        : base(message, inner)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        RawResponse = rawResponse;
    }

    /// <summary>
    ///     HTTP status code from API response
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    ///     Error code from Dify API (e.g., "invalid_param", "file_too_large")
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    ///     Raw error response body (JSON)
    /// </summary>
    public string RawResponse { get; }
}