using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_GetApplicationMetaResDto
{
    public Dify_AppMeta_ToolIcons ToolIcons { get; set; }
}

public class Dify_AppMeta_ToolIcons
{
    public string Dalle2 { get; set; }

    [JsonProperty("api_tool")] public Dify_AppMeta_ApiTool ApiTool { get; set; }
}

public class Dify_AppMeta_ApiTool
{
    public string Background { get; set; }

    public string Content { get; set; }
}