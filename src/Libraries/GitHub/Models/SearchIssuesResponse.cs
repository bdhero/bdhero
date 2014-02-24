using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Global
namespace GitHub.Models
{
    /// <summary>
    ///     Response received from GitHub after an issue search was performed.
    /// </summary>
    public class SearchIssuesResponse
    {
        /// <summary>
        ///     Number of search results.
        /// </summary>
        [JsonProperty("total_count")]
        public int ResultCount;

        /// <summary>
        ///     List of search results.
        /// </summary>
        [JsonProperty("items")]
        public List<SearchIssuesResult> Results;
    }
}
