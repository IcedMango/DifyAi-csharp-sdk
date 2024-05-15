using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_FileUploadResDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public int Size { get; set; }

    public string Extension { get; set; }

    [JsonProperty("mime_type")] public string MimeType { get; set; }

    [JsonProperty("created_by")] public string CreatedBy { get; set; }

    [JsonProperty("created_at")] [JsonConverter(typeof(UnixTimestampConverter))] public long CreatedAt { get; set; }
}