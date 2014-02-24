using System;
using System.Diagnostics;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Global
namespace GitHub.Models
{
    /// <summary>
    ///     High-level project milestone associated with a repository's issue tracker.
    /// </summary>
    [UsedImplicitly]
    [DebuggerDisplay("{Number}: {Title}")]
    public class Milestone
    {
        /// <summary>
        ///     GitHub API <c>GET</c> URL for the milestone.
        /// </summary>
        /// <example>
        ///     <code>"https://api.github.com/repos/octocat/Hello-World/milestones/1"</code>
        /// </example>
        public string Url { get; set; }

        /// <summary>
        ///     Milestone number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     The state of the milestone (<c>"open"</c> or <c>"closed"</c>).
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     Brief summary title of the milestone.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Detailed description of the milestone.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The user that created this milestone.
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        ///     The number of open issues associated with this milestone.
        /// </summary>
        public int OpenIssues { get; set; }

        /// <summary>
        ///     The number of closed issues associated with this milestone.
        /// </summary>
        public int ClosedIssues { get; set; }

        /// <summary>
        ///     The date and time the milestone was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     The due date of this milestone.
        /// </summary>
        public DateTime? DueOn { get; set; }
    }
}