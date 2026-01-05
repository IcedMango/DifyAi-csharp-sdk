namespace DifyAi.Dto.ResDto;

/// <summary>
///     Knowledge base retrieval response
/// </summary>
public class DifyRetrieveResDto
{
    public string QueryId { get; set; }
    public List<DifyRetrieveRecord> Records { get; set; }
}

/// <summary>
///     Retrieval record item
/// </summary>
public class DifyRetrieveRecord
{
    public DifyRetrieveSegment Segment { get; set; }
    public double Score { get; set; }
}

/// <summary>
///     Retrieved segment information
/// </summary>
public class DifyRetrieveSegment
{
    public string Id { get; set; }
    public int Position { get; set; }
    public string DocumentId { get; set; }
    public string Content { get; set; }
    public int WordCount { get; set; }
    public int TokenCount { get; set; }
    public string Keywords { get; set; }
    public int IndexNodeId { get; set; }
    public string IndexNodeHash { get; set; }
    public int HitCount { get; set; }
    public bool Enabled { get; set; }
    public bool DisabledAt { get; set; }
    public string DisabledBy { get; set; }
    public string Status { get; set; }
    public long CreatedAt { get; set; }
}