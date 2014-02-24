using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
namespace GitHub.Models
{
    /// <summary>
    ///     Response received when a GitHub API request fails.
    /// </summary>
    [UsedImplicitly]
    [DebuggerDisplay("Message = {Message}")]
    public class ErrorResponse
    {
        /// <summary>
        ///     A brief, high-level summary of what went wrong.
        /// </summary>
        /// <example>
        ///     <code>"Validation Failed"</code>
        /// </example>
        [UsedImplicitly]
        public string Message;

        /// <summary>
        ///     URL of human-friendly GitHub API documentation related to the error.
        /// </summary>
        /// <example>
        ///     <code>"http://developer.github.com/v3/search/"</code>
        /// </example>
        [UsedImplicitly]
        public string DocumentationUrl;

        /// <summary>
        ///     List of errors that prevented the request from succeeding.
        /// </summary>
        [CanBeNull]
        [UsedImplicitly]
        public List<Error> Errors;

        public override string ToString()
        {
            if (Errors != null && Errors.Any())
            {
                return Errors.Count == 1
                           ? string.Format("{0}: {1}", Message, Errors.First().Message)
                           : string.Format("{0}: [ {1} ]", Message,
                                           string.Join("; ", Errors.Select(error => error.Message)));
            }
            return Message;
        }
    }
}
