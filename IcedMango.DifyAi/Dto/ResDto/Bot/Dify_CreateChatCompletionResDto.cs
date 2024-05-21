using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_CreateChatCompletionResDto
{
    public string Event { get; set; }

    /// <summary>
    /// Unique message ID
    /// </summary>
    [JsonProperty("message_id")]
    public string MessageId { get; set; }

    /// <summary>
    /// Conversation ID
    /// </summary>
    [JsonProperty("conversation_id")]
    public string ConversationId { get; set; }

    /// <summary>
    ///      App mode, fixed as chat
    /// </summary>
    public string Mode { get; set; }

    /// <summary>
    /// Complete response content
    /// </summary>
    public string Answer { get; set; }

    public Metadata Metadata { get; set; }

    [JsonProperty("created_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? CreatedAt { get; set; }
}

public class Metadata
{
    /// <summary>
    ///     Model usage information
    /// </summary>
    public ModelUsage ModelUsage { get; set; }


    /// <summary>
    ///     Citation and Attribution List
    /// </summary>
    [JsonProperty("retriever_resources")]
    public List<Dify_RetrieverResource> RetrieverResources { get; set; }
}

public class ModelUsage
{
    [JsonProperty("prompt_tokens")] public int PromptTokens { get; set; }

    [JsonProperty("prompt_unit_price")] public string PromptUnitPrice { get; set; }

    [JsonProperty("prompt_price_unit")] public string PromptPriceUnit { get; set; }

    [JsonProperty("prompt_price")] public string PromptPrice { get; set; }

    [JsonProperty("completion_tokens")] public int CompletionTokens { get; set; }

    [JsonProperty("completion_unit_price")] public string CompletionUnitPrice { get; set; }

    [JsonProperty("completion_price_unit")] public string CompletionPriceUnit { get; set; }

    [JsonProperty("completion_price")] public string CompletionPrice { get; set; }

    [JsonProperty("total_tokens")] public int TotalTokens { get; set; }

    [JsonProperty("total_price")] public string TotalPrice { get; set; }

    public string Currency { get; set; }

    public double? Latency { get; set; }
}

public class Dify_RetrieverResource
{
    [JsonProperty("position")] public int Position { get; set; }

    [JsonProperty("dataset_id")] public string DatasetId { get; set; }

    [JsonProperty("dataset_name")] public string DatasetName { get; set; }

    [JsonProperty("document_id")] public string DocumentId { get; set; }

    [JsonProperty("document_name")] public string DocumentName { get; set; }

    [JsonProperty("segment_id")] public string SegmentId { get; set; }

    [JsonProperty("score")] public double Score { get; set; }

    [JsonProperty("content")] public string Content { get; set; }
}