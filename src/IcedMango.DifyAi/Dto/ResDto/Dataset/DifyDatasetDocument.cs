using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class DifyDatasetDocument
{
    public string Id { get; set; }

    public int Position { get; set; }

    [JsonProperty("data_source_type")] public string DataSourceType { get; set; }

    [JsonProperty("data_source_info")] public DifyDatasetDocument_DataSourceInfo DataSourceInfo { get; set; }

    [JsonProperty("dataset_process_rule_id")] public string DatasetProcessRuleId { get; set; }

    public string Name { get; set; }

    [JsonProperty("created_from")] public string CreatedFrom { get; set; }

    [JsonProperty("created_by")] public string CreatedBy { get; set; }

    [JsonProperty("created_at")] [JsonConverter(typeof(UnixTimestampConverter))] public DateTime? CreatedAt { get; set; }

    public int? Tokens { get; set; }

    [JsonProperty("indexing_status")] public string IndexingStatus { get; set; }

    public string Error { get; set; }

    public bool? Enabled { get; set; }

    [JsonProperty("disabled_at")] [JsonConverter(typeof(UnixTimestampConverter))] public DateTime? DisabledAt { get; set; }

    [JsonProperty("disabled_by")] public string DisabledBy { get; set; }

    public bool? Archived { get; set; }

    [JsonProperty("display_status")] public string DisplayStatus { get; set; }

    [JsonProperty("word_count")] public int? WordCount { get; set; }

    [JsonProperty("hit_count")] public int? HitCount { get; set; }

    [JsonProperty("doc_form")] public string DocForm { get; set; }
}

public class DifyDatasetDocument_DataSourceInfo
{
    [JsonProperty("upload_file_id")] public string UploadFileId { get; set; }
}