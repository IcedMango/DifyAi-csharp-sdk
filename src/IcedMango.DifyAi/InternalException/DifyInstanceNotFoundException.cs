namespace DifyAi.InternalException;

/// <summary>
///     Exception thrown when a requested instance is not registered in the container
/// </summary>
public class DifyInstanceNotFoundException : DifySDKException
{
    public DifyInstanceNotFoundException(string instanceName, string instanceType)
        : base($"{instanceType} instance '{instanceName}' is not registered. Please call DifyAiContainer.Register{instanceType}(\"{instanceName}\", \"your-api-key\") first.")
    {
        InstanceName = instanceName;
        InstanceType = instanceType;
    }

    /// <summary>
    ///     Name of the instance that was not found
    /// </summary>
    public string InstanceName { get; }

    /// <summary>
    ///     Type of instance (Bot or Dataset)
    /// </summary>
    public string InstanceType { get; }
}
