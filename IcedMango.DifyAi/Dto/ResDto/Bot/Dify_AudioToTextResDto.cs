using System.Text.Json.Serialization;

namespace DifyAi.Dto.ResDto;

public class Dify_AudioToTextResDto
{
    /// <summary>
    ///     
    /// </summary>
    [JsonPropertyName("test")]
    public string Text { get; set; }
}