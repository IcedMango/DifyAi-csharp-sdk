namespace DifyAi.Dto.ResDto;

/// <summary>
///     Response DTO for conversation variables
/// </summary>
public class DifyGetConversationVariablesResDto
{
    public List<DifyConversationVariable> Data { get; set; }
    public bool HasMore { get; set; }
}

/// <summary>
///     Conversation variable item
/// </summary>
public class DifyConversationVariable
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public object Value { get; set; }
    public long CreatedAt { get; set; }
}