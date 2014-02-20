using System;
using System.Diagnostics;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace BDHero.ErrorReporting.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("{Number}: {Title}")]
    public class Milestone
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("creator")]
        public User Creator { get; set; }

        [JsonProperty("open_issues")]
        public int OpenIssues { get; set; }

        [JsonProperty("closed_issues")]
        public int ClosedIssues { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("due_on")]
        public DateTime? DueOn { get; set; }
    }
}