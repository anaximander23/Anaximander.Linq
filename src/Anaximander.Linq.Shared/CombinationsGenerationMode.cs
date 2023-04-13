namespace Anaximander.Linq
{
    /// <summary>
    /// How to handle duplication when generating combinations.
    /// </summary>
    public enum CombinationsGenerationMode
    {
        /// <summary>
        /// Use each source item only once, treating a different order of the same elements as a new combination. Note: Requires source collection size to be equal to or greater than combinationSize.
        /// </summary>
        DistinctOrderSensitive,

        /// <summary>
        /// Use each source item only once, treating a different order of the same elements as the same combination. Note: Requires source collection size to be equal to or greater than combinationSize.
        /// </summary>
        DistinctOrderInsensitive,

        /// <summary>
        /// Allow source items to be used multiple times, treating a different order of the same elements as a new combination.
        /// </summary>
        AllowDuplicatesOrderSensitive,

        /// <summary>
        /// Allow source items to be used multiple times, treating a different order of the same elements as the same combination.
        /// </summary>
        AllowDuplicatesOrderInsensitive,
    }
}