using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class DifyGetConversationListResDto : DifyBaseRequestResDto<List<DifyGetConversationListResDto_DataItem>>
{
}

public class DifyGetConversationListResDto_DataItem
{
    public string Id { get; set; }

    /// <summary>
    ///     Conversation name, by default, is a snippet of the first question asked by the user in the conversation.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     User input parameters.
    /// </summary>
    public Dictionary<string, string> Inputs { get; set; } = new ();

    /// <summary>
    ///     Introduction
    /// </summary>
    public string Introduction { get; set; }


    /// <summary>
    ///     Creation time
    /// </summary>
    [JsonProperty("created_at")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? CreatedAt { get; set; }
}