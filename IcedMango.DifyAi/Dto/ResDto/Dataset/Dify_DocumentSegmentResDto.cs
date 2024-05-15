using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_DocumentSegmentResDto
{
    [JsonProperty("doc_form")] public string DocForm { get; set; }
    public List<Dify_DocumentSegmentsItem> Data { get; set; }
}

public class Dify_DocumentSegmentsItem
{
    public string Id { get; set; }

    public int? Position { get; set; }

    [JsonProperty("document_id")] public string DocumentId { get; set; }

    [JsonProperty("content")] public string Content { get; set; }

    [JsonProperty("answer")] public string Answer { get; set; }

    [JsonProperty("word_count")] public int? WordCount { get; set; }

    public int? Tokens { get; set; }

    public List<string> Keywords { get; set; }

    [JsonProperty("index_node_id")] public string IndexNodeId { get; set; }

    [JsonProperty("index_node_hash")] public string IndexNodeHash { get; set; }

    [JsonProperty("hit_count")] public int? HitCount { get; set; }

    public bool? Enabled { get; set; }

    [JsonProperty("disabled_at")] [JsonConverter(typeof(UnixTimestampConverter))] public object DisabledAt { get; set; }

    [JsonProperty("disabled_by")] public object DisabledBy { get; set; }

    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("created_by")] public string CreatedBy { get; set; }

    [JsonProperty("created_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("indexing_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? IndexingAt { get; set; }

    [JsonProperty("completed_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? CompletedAt { get; set; }

    [JsonProperty("error")] public string Error { get; set; }

    [JsonProperty("stopped_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? StoppedAt { get; set; }
}