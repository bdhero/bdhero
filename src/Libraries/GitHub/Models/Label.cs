using System.Diagnostics;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
namespace GitHub.Models
{
    /// <summary>
    ///     A short label associated with an issue.
    /// </summary>
    [UsedImplicitly]
    [DebuggerDisplay("{Name}")]
    public class Label
    {
        /// <summary>
        ///     GitHub API <c>GET</c> URL for this label.
        /// </summary>
        /// <example>
        ///     <code>"https://api.github.com/repos/bdhero/bdhero/labels/report"</code>
        /// </example>
        public string Url;

        /// <summary>
        ///     The human <b>and</b> machine-readable name of this label.
        /// </summary>
        /// <example>
        ///     <code>"report"</code>
        /// </example>
        public string Name;

        /// <summary>
        ///     The label's color in HEX format.
        /// </summary>
        /// <example>
        ///     <code>"eb6420"</code>
        /// </example>
        public string Color;
    }
}