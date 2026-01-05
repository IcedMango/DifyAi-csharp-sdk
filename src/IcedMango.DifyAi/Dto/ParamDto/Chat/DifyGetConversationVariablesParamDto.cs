namespace DifyAi.Dto.ParamDto;

/// <summary>
///     Parameters for getting conversation variables
/// </summary>
public class DifyGetConversationVariablesParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     Conversation ID
    /// </summary>
    public string ConversationId { get; set; }

    /// <summary>
    ///     User identifier
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///     Variable name to filter (optional)
    /// </summary>
    public string VariableName { get; set; }

    /// <summary>
    ///     Last ID for pagination (optional)
    /// </summary>
    public string LastId { get; set; }

    /// <summary>
    ///     Maximum number of items to return (range: 1-100, default: 20)
    /// </summary>
    public int Limit { get; set; } = 20;
}