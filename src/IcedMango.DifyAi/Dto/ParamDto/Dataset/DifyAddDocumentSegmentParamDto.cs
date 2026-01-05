using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class DifyAddDocumentSegmentParamDto : DifyBaseRequestParamDto
{
    public string DatasetId { get; set; }

    public string DocumentId { get; set; }

    [JsonProperty("segments")]
    public List<DifyAddDocumentSegment_Segment> Segments { get; set; }
}

public class DifyAddDocumentSegment_Segment
{
    /// <summary>
    ///     Text content/question content, required
    /// </summary>
    [JsonProperty("content")]
    public string Content { get; set; }

    /// <summary>
    ///     Answer content, if the mode of the Knowledge is qa mode, pass the value(optional)
    /// </summary>
    [JsonProperty("answer")]
    public string Answer { get; set; }

    /// <summary>
    ///     Keywords(optional)
    /// </summary>
    [JsonProperty("keywords")]
    public List<string> Keywords { get; set; }
}