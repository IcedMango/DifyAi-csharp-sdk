namespace DifyAi.InternalException;

/// <summary>
///     Exception thrown when an invalid file is provided
/// </summary>
public class DifyInvalidFileException : DifySDKException
{
    public DifyInvalidFileException(string message) : base(message)
    {
    }

    public DifyInvalidFileException(string message, string filePath)
        : base(message)
    {
        FilePath = filePath;
    }

    public DifyInvalidFileException(string message, string filePath, string[] supportedFormats)
        : base(message)
    {
        FilePath = filePath;
        SupportedFormats = supportedFormats;
    }

    /// <summary>
    ///     Path of the invalid file
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    ///     List of supported file formats
    /// </summary>
    public string[] SupportedFormats { get; }
}
