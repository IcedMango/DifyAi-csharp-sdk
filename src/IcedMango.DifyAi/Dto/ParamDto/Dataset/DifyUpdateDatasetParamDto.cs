namespace DifyAi.Dto.ParamDto;

/// <summary>
///     Parameters for updating dataset
/// </summary>
public class DifyUpdateDatasetParamDto : DifyBaseRequestParamDto
{
    /// <summary>
    ///     Dataset ID
    /// </summary>
    public string DatasetId { get; set; }

    /// <summary>
    ///     Dataset name (optional)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Dataset description (optional)
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     Indexing technique: "high_quality" or "economy" (optional)
    /// </summary>
    public string IndexingTechnique { get; set; }

    /// <summary>
    ///     Permission: "only_me" or "all_team_members" (optional)
    /// </summary>
    public string Permission { get; set; }
}