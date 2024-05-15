namespace DifyAi.Dto.ParamDto;

public class Dify_AddDocumentSegmentParamDto : Dify_BaseRequestParamDto
{
    public string DatasetId { get; set; }

    public string DocumentId { get; set; }

    public List<Dify_AddDocumentSegment_Segment> Segment { get; set; }
}

public class Dify_AddDocumentSegment_Segment
{
    /// <summary>
    /// Text content/question content, required
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Answer content, if the mode of the Knowledge is qa mode, pass the value(optional)
    /// </summary>
    public string Answer { get; set; }

    /// <summary>
    ///     Keywords(optional)
    /// </summary>
    public List<string> Keywords { get; set; }
}