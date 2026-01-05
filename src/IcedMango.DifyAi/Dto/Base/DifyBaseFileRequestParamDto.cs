using System.ComponentModel.DataAnnotations;

namespace DifyAi.Dto.Base;

public abstract class DifyBaseFileRequestParamDto
{
    /// <summary>
    ///     File path (local)
    /// </summary>
    [Required]
    public string FilePath { get; set; }
}