using System.Collections.Generic;

namespace GitHub.Models
{
    public class SearchIssuesResult
    {
        public int Id;
        public int Number;
        public string Title;
        public string Body;
        public double Score;

        public string Url;
        public string LabelsUrl;
        public string CommentsUrl;
        public string EventsUrl;
        public string HtmlUrl;

        public User User;
        public List<Label> Labels;
        public string State;
        public User Assignee;
        public Milestone Milestone;
        public int Comments;

        public string CreatedAt;
        public string UpdatedAt;
        public string ClosedAt;

        public PullRequest PullRequest;
    }
}