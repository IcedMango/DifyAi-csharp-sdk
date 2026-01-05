using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class DifyDatasetProcessRule
{
    /// <summary>
    ///     Cleaning, segmentation mode, automatic / custom
    /// </summary>
    public string Mode { get; set; }


    /// <summary>
    ///     Custom rules (in automatic mode, this field is empty)
    /// </summary>
    public DifyDatasetProcessRule_RuleItem Rules { get; set; }
}

public class DifyDatasetProcessRule_RuleItem
{
    [JsonProperty("pre_processing_rules")] public List<DifyDatasetProcessRule_PreProcessingRules> PreProcessingRules { get; set; }

    public DifyDatasetProcessRule_Segmentation Segmentation { get; set; }
}

public class DifyDatasetProcessRule_Segmentation
{
    /// <summary>
    ///     Custom segment identifier, currently only allows one delimiter to be set. Default is \n
    /// </summary>
    public string Separator { get; set; } = "\n";

    /// <summary>
    ///     Maximum length (token) defaults to 1000
    /// </summary>
    [JsonProperty("max_tokens")]
    public int? MaxTokens { get; set; }
}

public class DifyDatasetProcessRule_PreProcessingRules
{
    /// <summary>
    ///     Unique identifier for the preprocessing rule
    ///     - remove_extra_spaces Replace consecutive spaces, newlines, tabs
    ///     - remove_urls_emails Delete URL, email address
    /// </summary>
    public string Id { get; set; }


    /// <summary>
    ///     Whether to select this rule or not.
    ///     If no document ID is passed in, it represents the default value.
    /// </summary>
    public bool? Enabled { get; set; }
}