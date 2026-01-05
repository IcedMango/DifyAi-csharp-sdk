namespace DifyAi.Dto.ParamDto;

/// <summary>
///     Parameters for text to audio conversion
/// </summary>
public class DifyTextToAudioParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     User identifier
    /// </summary>
    public string User { get; set; }

    /// <summary>
    ///     Text content to convert to audio
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Message ID (optional, for voice clone)
    /// </summary>
    public string MessageId { get; set; }
}