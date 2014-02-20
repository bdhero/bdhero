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
        public string Url { get; set; }
        public int Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User Creator { get; set; }
        public int OpenIssues { get; set; }
        public int ClosedIssues { get; set; }
        public string CreatedAt { get; set; }
        public DateTime? DueOn { get; set; }
    }
}