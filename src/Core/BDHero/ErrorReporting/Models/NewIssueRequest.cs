using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetUtils;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace BDHero.ErrorReporting.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("{Title}")]
    internal class NewIssueRequest
    {
        /// <summary>
        ///     Indentation to create a code block in Markdown syntax.
        /// </summary>
        private const string CodeIndent = "    ";

        /// <summary>
        ///     Gets or sets a brief summary of the issue.
        /// </summary>
        [JsonProperty("title")]
        public string Title;
        
        /// <summary>
        ///     Gets or sets a detailed description of the issue.
        /// </summary>
        [JsonProperty("body")]
        public string Body;
        
        /// <summary>
        ///     Gets or sets the login name (username) of the GitHub user to whom the issue is assigned.
        /// </summary>
        [JsonProperty("assignee")]
        public string Assignee;

        /// <summary>
        ///     Gets or sets a list of labels to tag the issue with.
        /// </summary>
        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        public NewIssueRequest(Exception exception)
        {
            var stackTrace = string.Join("\n", exception.ToString().Split('\n').Select(line => CodeIndent + line));
            Title = string.Format("Exception: {0} ({1} v{2})", exception.Message, AppUtils.AppName, AppUtils.AppVersion);
            Body = string.Format("{0} v{1}:\n\n{2}", AppUtils.AppName, AppUtils.AppVersion, stackTrace);
            Labels = new List<string> { "report" };
        }
    }
}