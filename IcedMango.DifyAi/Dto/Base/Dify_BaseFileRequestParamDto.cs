using System.ComponentModel.DataAnnotations;

namespace DifyAi.Dto.Base;

public abstract class Dify_BaseFileRequestParamDto
{
    /// <summary>
    ///     文件路径
    /// </summary>
    [Required]
    public string FilePath { get; set; }
}