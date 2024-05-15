using System.ComponentModel.DataAnnotations;

namespace DifyAi.Dto.ParamDto;

public class Dify_FileUploadParamDto : Dify_BaseFileRequestParamDto
{
    /// <summary>
    ///     User identifier, used to define the identity of the end-user for retrieval and statistics.
    ///     Should be uniquely defined by the developer within the application.
    /// </summary>
    [Required]
    public string User { get; set; }
}