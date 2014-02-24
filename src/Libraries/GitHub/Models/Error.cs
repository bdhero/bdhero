using System.Diagnostics;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
namespace GitHub.Models
{
    /// <summary>
    ///     An individual error.
    /// </summary>
    [UsedImplicitly]
    [DebuggerDisplay("Message = {Message}, Field = {Field}")]
    public class Error
    {
        /// <summary>
        ///     A detailed description of the error.
        /// </summary>
        /// <example>
        ///     <code>"The search is longer than 256 characters."</code>
        /// </example>
        public string Message;

        /// <summary>
        ///     The name of the requested resource.
        /// </summary>
        /// <example>
        ///     <code>"search"</code>
        /// </example>
        public string Resource;

        /// <summary>
        ///     The name of the query string parameter or request body field that caused the error.
        /// </summary>
        /// <example>
        ///     <code>"q"</code>
        /// </example>
        public string Field;

        /// <summary>
        ///     A short error code.
        /// </summary>
        /// <example>
        ///     <code>"invalid"</code>
        /// </example>
        public string Code;
    }
}