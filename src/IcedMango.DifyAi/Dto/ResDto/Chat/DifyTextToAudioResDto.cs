namespace DifyAi.Dto.ResDto;

/// <summary>
///     Response DTO for text to audio conversion
/// </summary>
public class DifyTextToAudioResDto
{
    public string TaskId { get; set; }
    public string Status { get; set; }
    public string Audio { get; set; }
    public string Format { get; set; }
    public double Duration { get; set; }
}