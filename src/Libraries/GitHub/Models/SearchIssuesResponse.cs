using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitHub.Models
{
    public class SearchIssuesResponse
    {
        [JsonProperty("total_count")]
        public int ResultCount;

        [JsonProperty("items")]
        public List<SearchIssuesResult> Results;
    }
}
