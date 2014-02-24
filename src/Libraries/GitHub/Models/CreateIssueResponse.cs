using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Global
namespace GitHub.Models
{
    /// <summary>
    ///     Response received from GitHub after successfully creating a new issue.
    /// </summary>
    [UsedImplicitly]
    [DebuggerDisplay("{Number}: {Title}")]
    public class CreateIssueResponse
    {
        /// <summary>
        ///     The issue's unique ID number.
        /// </summary>
        public int Id;

        /// <summary>
        ///     Repo-specific issue number.
        /// </summary>
        public int Number;

        /// <summary>
        ///     The state of the issue (<c>"open"</c> or <c>"closed"</c>).
        /// </summary>
        public string State;

        /// <summary>
        ///     Brief summary of the issue.
        /// </summary>
        public string Title;

        /// <summary>
        ///     Detailed description of the issue.
        /// </summary>
        public string Body;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for the issue.
        /// </summary>
        public string Url;

        /// <summary>
        ///     URL of a human-friendly HTML version of the issue.
        /// </summary>
        public string HtmlUrl;

        /// <summary>
        ///     The user that created the issue.
        /// </summary>
        public User User;

        /// <summary>
        ///     The user assigned to this issue.
        /// </summary>
        public User Assignee;

        /// <summary>
        ///     A list of labels this issue is tagged with.
        /// </summary>
        public List<Label> Labels;

        /// <summary>
        ///     Parameterized GitHub API <c>GET</c> URL to retrieve the details of a specific label.
        /// </summary>
        public string LabelsUrl;

        /// <summary>
        ///     The number of comments.
        /// </summary>
        public int Comments;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of comments associated with this issue.
        /// </summary>
        public string CommentsUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of events associated with this issue.
        /// </summary>
        public string EventsUrl;

        /// <summary>
        ///     The milestone associated with this issue.
        /// </summary>
        public Milestone Milestone;

        /// <summary>
        ///     The date and time this issue was created.
        /// </summary>
        public DateTime CreatedAt;

        /// <summary>
        ///     The date and time this issue was created.
        /// </summary>
        public DateTime UpdatedAt;

        /// <summary>
        ///     The date and time this issue was closed.
        /// </summary>
        public DateTime? ClosedAt;

        /// <summary>
        ///     The user that closed this issue.
        /// </summary>
        [CanBeNull]
        public User ClosedBy;
    }
}