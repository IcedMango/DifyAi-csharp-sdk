namespace DifyAi.Dto.ResDto;

/// <summary>
///     Response DTO for app feedbacks
/// </summary>
public class DifyGetAppFeedbacksResDto
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public bool HasMore { get; set; }
    public List<DifyAppFeedback> Data { get; set; }
}

/// <summary>
///     App feedback item
/// </summary>
public class DifyAppFeedback
{
    public string Id { get; set; }
    public string MessageId { get; set; }
    public string ConversationId { get; set; }
    public string Rating { get; set; }
    public string Content { get; set; }
    public string FromSource { get; set; }
    public string FromEndUserId { get; set; }
    public long CreatedAt { get; set; }
}