using System.Diagnostics;
using DotNetUtils.Annotations;
using DotNetUtils.Net;

namespace GitHub.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("Body = {Body}")]
    internal class CreateIssueCommentRequest : IGitHubRequest<CreateIssueCommentResponse>
    {
        /// <summary>
        ///     Gets or sets a detailed description of the issue.
        /// </summary>
        [UsedImplicitly]
        public string Body;

        public CreateIssueCommentRequest(string repo, int issueNumber, string body)
        {
            Body = body;
            Url = string.Format("https://api.github.com/repos/{0}/issues/{1}/comments", repo, issueNumber);
        }

        public HttpRequestMethod Method
        {
            get { return HttpRequestMethod.Post; }
        }

        public string Url { get; private set; }
    }
}
