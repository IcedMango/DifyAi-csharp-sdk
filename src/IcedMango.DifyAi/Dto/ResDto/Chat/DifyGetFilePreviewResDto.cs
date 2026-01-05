namespace DifyAi.Dto.ResDto;

/// <summary>
///     Response DTO for file preview
/// </summary>
public class DifyGetFilePreviewResDto
{
    public string FileId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Size { get; set; }
    public string Url { get; set; }
    public long ExpiresAt { get; set; }
}