namespace DifyAi.Dto.ParamDto;

public class Dify_GetDocumentSegmentParamDto
{
    public string DatasetId { get; set; }
    
    public string DocumentId { get; set; }
    
    public string Keyword { get; set; }
    
    public string Status { get; set; }
}