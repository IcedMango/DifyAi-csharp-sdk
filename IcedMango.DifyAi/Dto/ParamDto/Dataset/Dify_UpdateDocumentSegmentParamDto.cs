namespace DifyAi.Dto.ParamDto;

public class Dify_UpdateDocumentSegmentParamDto : Dify_BaseRequestParamDto
{
    public string DatasetId { get; set; }

    public string DocumentId { get; set; }

    public string SegmentId { get; set; }

    public List<Dify_UpdateDocumentSegment_Segment> Segment { get; set; }
}

public class Dify_UpdateDocumentSegment_Segment : Dify_AddDocumentSegment_Segment
{
    public bool? Enabled { get; set; }
}