namespace DifyAi.InternalException;

/// <summary>
///     Exception thrown when configuration validation fails
/// </summary>
public class DifyConfigurationException : DifySDKException
{
    public DifyConfigurationException(string message) : base(message)
    {
    }

    public DifyConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    ///     Name of the invalid configuration property
    /// </summary>
    public string PropertyName { get; init; }
}
