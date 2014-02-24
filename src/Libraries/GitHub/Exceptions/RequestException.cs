using System;
using System.Diagnostics;
using GitHub.Models;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace GitHub.Exceptions
{
    /// <summary>
    ///     Represents one or more errors returned by the GitHub API in response to an HTTP request.
    /// </summary>
    [DebuggerDisplay("ErrorResponse = {ErrorResponse}")]
    public class RequestException : Exception
    {
        /// <summary>
        ///     Gets the error details returned by the GitHub API.
        /// </summary>
        public ErrorResponse ErrorResponse { get; private set; }

        /// <summary>
        ///     Constructs a new <see cref="RequestException"/> instance with the given parameters.
        /// </summary>
        /// <param name="innerException">
        ///     The inner exception that caused this <c>RequestException</c>.
        /// </param>
        /// <param name="errorResponse">
        ///     Response from the GitHub API.
        /// </param>
        public RequestException(Exception innerException, ErrorResponse errorResponse)
            : base(errorResponse.ToString(), innerException)
        {
            ErrorResponse = errorResponse;
        }

        /// <summary>
        ///     Constructs a new <see cref="RequestException"/> instance with the given parameters.
        /// </summary>
        /// <param name="message">
        ///     Custom error message.
        /// </param>
        /// <param name="innerException">
        ///     The inner exception that caused this <c>RequestException</c>.
        /// </param>
        /// <param name="errorResponse">
        ///     Response from the GitHub API.
        /// </param>
        public RequestException(string message, Exception innerException, ErrorResponse errorResponse)
            : base(message, innerException)
        {
            ErrorResponse = errorResponse;
        }
    }
}
