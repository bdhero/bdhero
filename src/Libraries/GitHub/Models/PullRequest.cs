using System.Diagnostics;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Global
namespace GitHub.Models
{
    /// <summary>
    ///     Represents a pull request on GitHub.
    /// </summary>
    /// <remarks>
    ///     Every pull request is an issue, but not every issue is a pull request.
    /// </remarks>
    [UsedImplicitly]
    [DebuggerDisplay("HtmlUrl = {HtmlUrl}")]
    public class PullRequest
    {
        /// <summary>
        ///     URL of a human-friendly summary of the pull request.
        /// </summary>
        public string HtmlUrl;

        /// <summary>
        ///     URL of the diff for the pull request.
        /// </summary>
        public string DiffUrl;

        /// <summary>
        ///     URL of the patch for the pull request.
        /// </summary>
        public string PatchUrl;
    }
}