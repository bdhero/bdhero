using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace BDHero.ErrorReporting.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("{Number}: {Title}")]
    public class NewIssueResponse
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public int Number { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public User User { get; set; }
        public User Assignee { get; set; }
        public List<Label> Labels { get; set; }
        public string LabelsUrl { get; set; }
        public int Comments { get; set; }
        public string CommentsUrl { get; set; }
        public string EventsUrl { get; set; }
        public Milestone Milestone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public User ClosedBy { get; set; }
    }
}