using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_GetDocumentEmbeddingResDto
{
    public string Id { get; set; }

    [JsonProperty("indexing_status")] public string IndexingStatus { get; set; }

    [JsonProperty("processing_started_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? ProcessingStartedAt { get; set; }

    [JsonProperty("parsing_completed_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? ParsingCompletedAt { get; set; }

    [JsonProperty("cleaning_completed_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? CleaningCompletedAt { get; set; }

    [JsonProperty("splitting_completed_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? SplittingCompletedAt { get; set; }

    [JsonProperty("completed_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? CompletedAt { get; set; }

    [JsonProperty("paused_at")] [JsonConverter(typeof(UnixTimestampConverter))] public DateTime? PausedAt { get; set; }

    [JsonProperty("error")] public string Error { get; set; }

    [JsonProperty("stopped_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? StoppedAt { get; set; }

    [JsonProperty("completed_segments")] public int? CompletedSegments { get; set; }

    [JsonProperty("total_segments")] public int? TotalSegments { get; set; }
}