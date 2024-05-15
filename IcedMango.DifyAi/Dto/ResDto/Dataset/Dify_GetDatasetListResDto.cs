using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_GetDatasetListResDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Permission { get; set; }

    [JsonProperty("data_source_type")]
    public string DataSourceType { get; set; }

    [JsonProperty("indexing_technique")]
    public string IndexingTechnique { get; set; }

    [JsonProperty("app_count")]
    public int AppCount { get; set; }

    [JsonProperty("document_count")]
    public int DocumentCount { get; set; }

    [JsonProperty("word_count")]
    public int WordCount { get; set; }

    [JsonProperty("created_by")]
    public string CreatedBy { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("updated_by")]
    public string UpdatedBy { get; set; }

    [JsonProperty("updated_at")]
    public string UpdatedAt { get; set; }
}