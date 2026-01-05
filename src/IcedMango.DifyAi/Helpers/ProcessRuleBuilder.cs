namespace DifyAi.Helpers;

/// <summary>
///     Helper class for building ProcessRule objects
/// </summary>
internal static class ProcessRuleBuilder
{
    /// <summary>
    ///     Builds a ProcessRule object with the specified parameters
    /// </summary>
    /// <param name="isAutomaticProcess">Whether to use automatic processing mode</param>
    /// <param name="removeExtraSpaces">Whether to remove extra spaces</param>
    /// <param name="removeUrlsEmails">Whether to remove URLs and emails</param>
    /// <param name="separator">Segment separator (default: \n)</param>
    /// <param name="maxTokens">Maximum tokens per segment (default: 1000)</param>
    /// <returns>Configured ProcessRule object</returns>
    public static DifyDatasetProcessRule Build(
        bool? isAutomaticProcess,
        bool? removeExtraSpaces,
        bool? removeUrlsEmails,
        string separator = "\n",
        int? maxTokens = 1000)
    {
        // When using automatic mode, provide default values for pre-processing rules
        // to avoid null values that Dify API rejects
        var effectiveRemoveExtraSpaces = removeExtraSpaces ?? (isAutomaticProcess == true ? true : false);
        var effectiveRemoveUrlsEmails = removeUrlsEmails ?? (isAutomaticProcess == true ? true : false);

        return new DifyDatasetProcessRule
        {
            Mode = isAutomaticProcess == true ? "automatic" : "custom",
            Rules = new DifyDatasetProcessRule_RuleItem
            {
                PreProcessingRules = new List<DifyDatasetProcessRule_PreProcessingRules>
                {
                    new ()
                    {
                        Id = "remove_extra_spaces",
                        Enabled = effectiveRemoveExtraSpaces
                    },
                    new ()
                    {
                        Id = "remove_urls_emails",
                        Enabled = effectiveRemoveUrlsEmails
                    }
                },
                Segmentation = new DifyDatasetProcessRule_Segmentation
                {
                    Separator = separator,
                    MaxTokens = maxTokens
                }
            }
        };
    }
}