using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetUtils;
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
        ///     Indentation to create a code block in Markdown syntax.
        /// </summary>
        private const string CodeIndent = "    ";

        /// <summary>
        ///     Gets or sets a brief summary of the issue.
        /// </summary>
        public string Title;
        
        /// <summary>
        ///     Gets or sets a detailed description of the issue.
        /// </summary>
        public string Body;
        
        /// <summary>
        ///     Gets or sets the login name (username) of the GitHub user to whom the issue is assigned.
        /// </summary>
        public string Assignee;

        /// <summary>
        ///     Gets or sets a list of labels to tag the issue with.
        /// </summary>
        public List<string> Labels { get; set; }

        public CreateIssueRequest(string repo, Exception exception)
        {
            var stackTrace = string.Join("\n", exception.ToString().Split('\n').Select(line => CodeIndent + line));

            Title = string.Format("{0}: {1} ({2} v{3})", exception.GetType().FullName, exception.Message, AppUtils.AppName, AppUtils.AppVersion);
            Body = string.Format("{0} v{1}:\n\n{2}", AppUtils.AppName, AppUtils.AppVersion, stackTrace);
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