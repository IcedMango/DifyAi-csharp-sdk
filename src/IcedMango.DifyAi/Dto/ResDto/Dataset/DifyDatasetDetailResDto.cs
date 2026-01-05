namespace DifyAi.Dto.ResDto;

/// <summary>
///     Dataset detail response
/// </summary>
public class DifyDatasetDetailResDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Permission { get; set; }
    public string DataSourceType { get; set; }
    public string IndexingTechnique { get; set; }
    public int DocumentCount { get; set; }
    public int WordCount { get; set; }
    public string CreatedBy { get; set; }
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
    public string EmbeddingModel { get; set; }
    public string EmbeddingModelProvider { get; set; }
}