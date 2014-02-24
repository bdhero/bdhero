using System.Diagnostics;

namespace GitHub.Models
{
    [DebuggerDisplay("HtmlUrl = {HtmlUrl}")]
    public class PullRequest
    {
        public string HtmlUrl;
        public string DiffUrl;
        public string PatchUrl;
    }
}