using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

public class DifyUpdateDocumentSegmentParamDto : DifyBaseRequestParamDto
{
    public string DatasetId { get; set; }

    public string DocumentId { get; set; }

    public string SegmentId { get; set; }

    [JsonProperty("segment")]
    public DifyUpdateDocumentSegment_Segment Segment { get; set; }
}

public class DifyUpdateDocumentSegment_Segment : DifyAddDocumentSegment_Segment
{
    public bool? Enabled { get; set; }
}