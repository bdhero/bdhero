using System;

namespace DotNetUtils.Exceptions
{
    /// <summary>
    ///     Represents errors that occur during application execution that may or may not be reportable via
    ///     bug trackers depending on how the errors were generated.
    /// </summary>
    public class ReportableException : Exception
    {
        /// <summary>
        ///     Gets or sets whether this exception can be reported to a bug tracker.
        ///     Default is <c>true</c>.
        /// </summary>
        public bool IsReportable = true;

        /// <summary>
        ///     Constructs a new instance of the <see cref="ReportableException"/> class.
        /// </summary>
        public ReportableException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportableException"/> class with a specified error message
        ///     <paramref name="message"/>.
        /// </summary>
        /// <param name="message">
        ///     Human-friendly message that describes the error.
        /// </param>
        public ReportableException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportableException"/> class with a specified error message
        ///     and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        ///     Human-friendly message that describes the error.
        /// </param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a <c>null</c> reference if no
        ///     inner exception is specified. 
        /// </param>
        public ReportableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
