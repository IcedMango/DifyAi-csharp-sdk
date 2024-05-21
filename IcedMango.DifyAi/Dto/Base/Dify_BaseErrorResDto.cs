using Newtonsoft.Json;

namespace DifyAi.Dto.Base;

public class Dify_BaseErrorResDto
{
    public string Code { get; set; }

    public string Message { get; set; }

    public int Status { get; set; }
}