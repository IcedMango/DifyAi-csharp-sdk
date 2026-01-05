using System.Text.Json.Serialization;

namespace DifyAi.Dto.ResDto;

public class DifyAudioToTextResDto
{
    /// <summary>
    /// </summary>
    [JsonPropertyName("test")]
    public string Text { get; set; }
}