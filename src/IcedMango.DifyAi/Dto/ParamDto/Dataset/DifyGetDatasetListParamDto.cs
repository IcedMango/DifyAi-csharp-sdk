using Newtonsoft.Json;

namespace DifyAi.Dto.ParamDto;

/// <summary>
///     Parameters for getting dataset list
/// </summary>
public class DifyGetDatasetListParamDto
{
    /// <summary>
    ///     Page number, default 1
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    ///     Number of items per page, default 20, max 100
    /// </summary>
    public int Limit { get; set; } = 20;

    /// <summary>
    ///     Search keyword (optional)
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    ///     Tag IDs for filtering (optional)
    /// </summary>
    [JsonProperty("tag_ids")]
    public List<string> TagIds { get; set; }

    /// <summary>
    ///     Whether to include all datasets, default false
    /// </summary>
    [JsonProperty("include_all")]
    public bool IncludeAll { get; set; }
}