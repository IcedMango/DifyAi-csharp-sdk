using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_RenameConversationResDto
{
    [JsonProperty("id")] public string Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("inputs")] public Dictionary<string, string> Inputs { get; set; }

    [JsonProperty("introduction")] public string Introduction { get; set; }

    [JsonProperty("created_at")] public long CreatedAt { get; set; }
}