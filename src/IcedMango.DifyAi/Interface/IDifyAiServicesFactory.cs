namespace DifyAi.Interface;

/// <summary>
/// Unified Dify AI service factory interface - retrieves Bot or Dataset services on demand
/// </summary>
public interface IDifyAiServicesFactory
{
    /// <summary>
    /// Get Bot (Chat) service instance by name
    /// </summary>
    /// <param name="name">Instance name used during registration</param>
    /// <returns>IDifyAiChatServices instance</returns>
    /// <exception cref="DifyInstanceNotFoundException">Thrown when instance is not registered</exception>
    IDifyAiChatServices GetBotService(string name);

    /// <summary>
    /// Get Dataset service instance by name
    /// </summary>
    /// <param name="name">Instance name used during registration</param>
    /// <returns>IDifyAiDatasetServices instance</returns>
    /// <exception cref="DifyInstanceNotFoundException">Thrown when instance is not registered</exception>
    IDifyAiDatasetServices GetDatasetService(string name);
}
