using System;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetUtils;

namespace BDHero.ErrorReporting
{
    public class ErrorReport
    {
        /// <summary>
        ///     Indentation to create a code block in Markdown syntax.
        /// </summary>
        private const string CodeIndent = "    ";

        private const string Redacted = "[redacted]";

        /// <summary>
        ///     Matches Windows-style or UNC paths wrapped in single or double quotes.
        /// </summary>
        /// <remarks>
        ///     Adapted from the RegexBuddy 4 library: "Path: Windows or UNC"
        /// </remarks>
        private static readonly Regex WindowsPathQuoted = new Regex(
            @"
                (?<=(['""]))                                        # Opening quote
                (?:\b[a-z]:|\\\\[a-z0-9 %._-]+\\[a-z0-9 $%._-]+)\\  # Drive
                (?:[^\\/:*?""<>|\r\n]+\\)*                          # Folder
                [^\\/:*?""<>|\r\n]*                                 # File
                (?=\1)                                              # Matching closing quote
            ",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        /// <summary>
        ///     Matches Windows-style or UNC paths without regard for quotes.
        /// </summary>
        /// <remarks>
        ///     Adapted from the RegexBuddy 4 library: "Path: Windows or UNC"
        /// </remarks>
        private static readonly Regex WindowsPathUnquoted = new Regex(
            @"
                (?:\b[a-z]:|\\\\[a-z0-9 %._-]+\\[a-z0-9 $%._-]+)\\  # Drive
                (?:[^\\/:*?""<>|\r\n]+\\)*                          # Folder
                [^\\/:*?""<>|\r\n]*                                 # File
            ",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        private static readonly Regex UnixPathQuoted = new Regex(
            @"
                (?<=(['""]))             # Opening quote
                /                        # Root
                (?:[^/:*?""<>|\r\n]+/)*  # Folder
                [^/:*?""<>|\r\n]*        # File
                (?=\1)                   # Matching closing quote
            ",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        private static readonly Regex UnixPathUnquoted = new Regex(
            @"
                (?<=^|[\s])              # Opening quote
                /                        # Root
                (?:[^/:*?""<>|\r\n]+/)*  # Folder
                [^/:*?""<>|\r\n]*        # File
            ",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

        /// <summary>
        ///     Gets or sets a brief summary of the error.
        /// </summary>
        public string Title;

        /// <summary>
        ///     Gets or sets a detailed description of the error in Markdown format.
        /// </summary>
        public string Body;

        /// <summary>
        ///     Gets <see cref="System.Exception.Message"/> without removing potentially sensitive substrings
        ///     such as file paths.
        /// </summary>
        public readonly string ExceptionMessageRaw;

        /// <summary>
        ///     Gets a redacted version of <see cref="System.Exception.Message"/> that is safe to publish online
        ///     because potentially sensitive substrings (e.g., file paths) have been removed.
        /// </summary>
        public readonly string ExceptionMessageRedacted;

        /// <summary>
        ///     Gets <see cref="System.Exception.ToString"/> without removing potentially sensitive substrings
        ///     such as file paths.
        /// </summary>
        public readonly string ExceptionDetailRaw;

        /// <summary>
        ///     Gets a redacted version of <see cref="System.Exception.ToString"/> that is safe to publish online
        ///     because potentially sensitive substrings (e.g., file paths) have been removed.
        /// </summary>
        public readonly string ExceptionDetailRedacted;

        public ErrorReport(Exception exception)
        {
            exception = GetBaseException(exception);

            ExceptionMessageRaw = exception.Message;
            ExceptionMessageRedacted = Redact(ExceptionMessageRaw);

            ExceptionDetailRaw = exception.ToString();
            ExceptionDetailRedacted = Redact(ExceptionDetailRaw);

            Title = string.Format("{0}: {1} ({2} v{3})", exception.GetType().FullName, ExceptionMessageRedacted, AppUtils.AppName, AppUtils.AppVersion);
            Body = string.Format("{0} v{1}:\n\n{2}", AppUtils.AppName, AppUtils.AppVersion, FormatAsMarkdownCode(ExceptionDetailRedacted));
        }

        private static string FormatAsMarkdownCode(string str)
        {
            return string.Join("\n", str.Split('\n').Select(line => CodeIndent + line));
        }

        private static Exception GetBaseException(Exception exception)
        {
            var @base = exception;
            var aggregate = exception as AggregateException;
            if (aggregate != null)
            {
                @base = aggregate.GetBaseException();
            }
            return @base;
        }

        private static string Redact(string raw)
        {
            // TODO: Smarter sanitizing: replace letters in path with 'X' instead of completely removing them
            var sanitized = raw;
            sanitized = WindowsPathQuoted.Replace(sanitized, Redacted);
            sanitized = WindowsPathUnquoted.Replace(sanitized, Redacted);
            sanitized = UnixPathQuoted.Replace(sanitized, Redacted);
            sanitized = UnixPathUnquoted.Replace(sanitized, Redacted);
            return sanitized;
        }
    }
}
