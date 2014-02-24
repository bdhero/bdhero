using System;
using System.Diagnostics;
using System.Linq;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Net;

namespace GitHub.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("Body = {Body}")]
    internal class CreateIssueCommentRequest : IGitHubRequest<CreateIssueCommentResponse>
    {
        /// <summary>
        ///     Indentation to create a code block in Markdown syntax.
        /// </summary>
        private const string CodeIndent = "    ";

        /// <summary>
        ///     Gets or sets a detailed description of the issue.
        /// </summary>
        [UsedImplicitly]
        public string Body;

        public CreateIssueCommentRequest(string repo, int issueNumber, Exception exception)
        {
            var stackTrace = string.Join("\n", exception.ToString().Split('\n').Select(line => CodeIndent + line));

            Body = string.Format("{0} v{1}:\n\n{2}", AppUtils.AppName, AppUtils.AppVersion, stackTrace);
            Url = string.Format("https://api.github.com/repos/{0}/issues/{1}/comments", repo, issueNumber);
        }

        public HttpRequestMethod Method
        {
            get { return HttpRequestMethod.Post; }
        }

        public string Url { get; private set; }
    }
}
