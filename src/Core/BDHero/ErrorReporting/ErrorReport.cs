using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BDHero.Logging;
using BDHero.Plugin;
using BDHero.Startup;
using DotNetUtils;
using OSUtils.Info;

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

        public ErrorReport(Exception exception, IPluginRepository pluginRepository, IDirectoryLocator directoryLocator)
        {
            exception = GetBaseException(exception);

            ExceptionMessageRaw = exception.Message;
            ExceptionMessageRedacted = Redact(ExceptionMessageRaw);

            ExceptionDetailRaw = exception.ToString();
            ExceptionDetailRedacted = Redact(ExceptionDetailRaw);

            var plugins = pluginRepository.PluginsByType.Select(ToString).ToList();
            AddPluginHeaderRows(plugins);

            var logMessages = BoundedMemoryAppender.RecentEvents.Select(ToString).ToArray();
            var logEvents = string.Join("\n", logMessages);

            Title = string.Format("{0}: {1} ({2} v{3})", exception.GetType().FullName, ExceptionMessageRedacted, AppUtils.AppName, AppUtils.AppVersion);
            Body = string.Format(@"
{0} v{1}{2} (built on {3:u})

Stack Trace
-----------

{4}

Log Events
----------

{5}

Plugins
-------

{6}

System Info
-----------

{7}
".TrimStart(),
                AppUtils.AppName,
                AppUtils.AppVersion,
                directoryLocator.IsPortable ? " portable" : "",
                AppUtils.BuildDate,
                FormatAsMarkdownCode(ExceptionDetailRedacted),
                FormatAsMarkdownCode(logEvents),
                FormatAsMarkdownTable(plugins.ToArray()),
                FormatAsMarkdownCode(SystemInfo.Instance.ToString()));
        }

        private static string ToString(FormattedLoggingEvent @event)
        {
            return Redact(@event.ToString(true));
        }

        private static string FormatAsMarkdownTable(string[][] rows)
        {
            var numRows = rows.Count();
            var numCols = rows.First().Count();
            var widths = new List<int>(numCols);

            for (var colIdx = 0; colIdx < numCols; colIdx++)
            {
                // GitHub requires a minimum of 3 characters per cell
                var width = 3;
                for (var rowIdx = 0; rowIdx < numRows; rowIdx++)
                {
                    var row = rows[rowIdx];
                    var col = row[colIdx];
                    if (col.Length > width)
                        width = col.Length;
                }
                widths.Add(width);
            }

            var lines = new List<string>(numRows);

            for (var rowIdx = 0; rowIdx < numRows; rowIdx++)
            {
                var cells = new List<string>(numCols);
                for (var colIdx = 0; colIdx < numCols; colIdx++)
                {
                    var row = rows[rowIdx];
                    var col = row[colIdx];
                    var width = widths[colIdx];

                    if (col == "-")
                        cells.Add(new string('-', width));
                    else
                        cells.Add(string.Format("{0,-" + width + "}", col));
                }
                lines.Add(string.Format("{0} {1} {0}", "|", string.Join(" | ", cells)));
            }

            return string.Join(Environment.NewLine, lines);
        }

        private static string FormatAsMarkdownList(IEnumerable<string> plugins)
        {
            return string.Join(Environment.NewLine, plugins.Select((item, i) => string.Format("{0:D}. {1}", i + 1, item)));
        }

        private static void AddPluginHeaderRows(IList<string[]> plugins)
        {
            plugins.Insert(0, new[] { "R/O", "Plugin Name", "Version", "Build Date", "E/D" });
            plugins.Insert(1, new[] { "-", "-", "-", "-", "-" });
        }

        private static string[] ToString(IPlugin plugin)
        {
            return new[]
                   {
                       plugin.RunOrder.ToString("D"),
                       plugin.Name,
                       plugin.AssemblyInfo.Version.ToString(),
                       plugin.AssemblyInfo.BuildDate.ToString("u"),
                       plugin.Enabled ? " " : "DIS"
                   };
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
            var sanitized = raw;
            sanitized = WindowsPathQuoted.Replace(sanitized, Redacted);
            sanitized = WindowsPathUnquoted.Replace(sanitized, Redacted);
            sanitized = UnixPathQuoted.Replace(sanitized, Redacted);
            sanitized = UnixPathUnquoted.Replace(sanitized, Redacted);
            return sanitized;
        }
    }
}
