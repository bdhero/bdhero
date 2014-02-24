using System;
using System.Diagnostics;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Global
namespace GitHub.Models
{
    /// <summary>
    ///     Response received from GitHub after successfully creating an issue comment.
    /// </summary>
    [UsedImplicitly]
    [DebuggerDisplay("Id = {Id}, HtmlUrl = {HtmlUrl}")]
    public class CreateIssueCommentResponse
    {
        /// <summary>
        ///     The newly-created comment's unique ID.
        /// </summary>
        public int Id;
        
        /// <summary>
        ///     The user that created the comment.
        /// </summary>
        public User User;

        /// <summary>
        ///     Comment body in Markdown format.
        /// </summary>
        public string Body;

        /// <summary>
        ///     The GitHub API <c>GET</c> URL to view the comment's properties in machine-readable format.
        /// </summary>
        public string Url;

        /// <summary>
        ///     URL of a human-friendly HTML version of the issue comment.
        /// </summary>
        public string HtmlUrl;

        /// <summary>
        ///     The GitHub API <c>GET</c> URL to view the comment's parent issue properties in machine-readable format.
        /// </summary>
        public string IssueUrl;

        /// <summary>
        ///     The date and time the comment was created.
        /// </summary>
        public DateTime CreatedAt;

        /// <summary>
        ///     The date and time the comment was last modified.
        /// </summary>
        public DateTime UpdatedAt;
    }
}
