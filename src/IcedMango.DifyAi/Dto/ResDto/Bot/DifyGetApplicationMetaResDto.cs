using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class DifyGetApplicationMetaResDto
{
    public DifyAppMeta_ToolIcons ToolIcons { get; set; }
}

public class DifyAppMeta_ToolIcons
{
    public string Dalle2 { get; set; }

    [JsonProperty("api_tool")] public DifyAppMeta_ApiTool ApiTool { get; set; }
}

public class DifyAppMeta_ApiTool
{
    public string Background { get; set; }

    public string Content { get; set; }
}