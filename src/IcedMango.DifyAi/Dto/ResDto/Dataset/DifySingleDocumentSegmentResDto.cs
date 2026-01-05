using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

/// <summary>
/// Response DTO for single document segment operations (Update)
/// </summary>
public class DifySingleDocumentSegmentResDto
{
    [JsonProperty("data")]
    public DifyDocumentSegmentsItem Data { get; set; }
}
