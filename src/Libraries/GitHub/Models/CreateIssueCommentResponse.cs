using System.Diagnostics;
using DotNetUtils.Annotations;

namespace GitHub.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("Id = {Id}, HtmlUrl = {HtmlUrl}")]
    public class CreateIssueCommentResponse
    {
        public int Id;
        public User User;
        public string Body;

        public string Url;
        public string HtmlUrl;
        public string IssueUrl;

        public string CreatedAt;
        public string UpdatedAt;
    }
}
