using System.ComponentModel.DataAnnotations;

namespace DifyAi.Dto.Base;

public abstract class Dify_BaseFileRequestParamDto
{
    /// <summary>
    ///     File path (local)
    /// </summary>
    [Required]
    public string FilePath { get; set; }
}