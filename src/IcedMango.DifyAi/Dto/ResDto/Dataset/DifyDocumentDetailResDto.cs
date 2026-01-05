namespace DifyAi.Dto.ResDto;

/// <summary>
///     Document detail response
/// </summary>
public class DifyDocumentDetailResDto
{
    public string Id { get; set; }
    public int Position { get; set; }
    public string DataSourceType { get; set; }
    public DifyDatasetDocument DataSourceInfo { get; set; }
    public string DatasetProcessRuleId { get; set; }
    public string Name { get; set; }
    public long CreatedAt { get; set; }
    public int WordCount { get; set; }
    public string IndexingStatus { get; set; }
    public string Error { get; set; }
    public bool Enabled { get; set; }
    public bool DisabledAt { get; set; }
    public string DisabledBy { get; set; }
    public bool Archived { get; set; }
    public int SegmentCount { get; set; }
    public int HitCount { get; set; }
    public string DisplayStatus { get; set; }
}