using System.Collections.Generic;
using System.Diagnostics;
using DotNetUtils.Annotations;
using DotNetUtils.Net;

namespace GitHub.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("Title = {Title}")]
    internal class CreateIssueRequest : IGitHubRequest<CreateIssueResponse>
    {
        public const string DefaultLabel = "report";

        /// <summary>
        ///     Gets or sets a brief summary of the issue.
        /// </summary>
        [UsedImplicitly]
        public string Title;
        
        /// <summary>
        ///     Gets or sets a detailed description of the issue.
        /// </summary>
        [UsedImplicitly]
        public string Body;
        
        /// <summary>
        ///     Gets or sets the login name (username) of the GitHub user to whom the issue is assigned.
        /// </summary>
        [UsedImplicitly]
        public string Assignee;

        /// <summary>
        ///     Gets or sets a list of labels to tag the issue with.
        /// </summary>
        [UsedImplicitly]
        public List<string> Labels { get; set; }

        public CreateIssueRequest(string repo, string title, string body)
        {
            Title = title;
            Body = body;
            Labels = new List<string> { DefaultLabel };
            Url = string.Format("https://api.github.com/repos/{0}/issues", repo);
        }

        public HttpRequestMethod Method
        {
            get { return HttpRequestMethod.Post; }
        }

        public string Url { get; private set; }
    }
}