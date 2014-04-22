using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Plugin.FFmpegMuxer
{
    public enum FFmpegLogLevel
    {
        /// <summary>
        ///     Show nothing at all; be silent.
        /// </summary>
        [Argument("quiet")]
        Quiet,

        /// <summary>
        ///     Only show fatal errors which could lead the process to crash, such as and assert failure. This is not currently used for anything.
        /// </summary>
        [Argument("panic")]
        Panic,

        /// <summary>
        ///     Only show fatal errors. These are errors after which the process absolutely cannot continue after.
        /// </summary>
        [Argument("fatal")]
        Fatal,

        /// <summary>
        ///     Show all errors, including ones which can be recovered from.
        /// </summary>
        [Argument("error")]
        Error,

        /// <summary>
        ///     Show all warnings and errors. Any message related to possibly incorrect or unexpected events will be shown.
        /// </summary>
        [Argument("warning")]
        Warning,

        /// <summary>
        ///     Show informative messages during processing. This is in addition to warnings and errors. This is the default value.
        /// </summary>
        [Argument("info")]
        Info,

        /// <summary>
        ///     Same as <see cref="Info"/>, except more verbose.
        /// </summary>
        [Argument("verbose")]
        Verbose,

        /// <summary>
        ///     Show everything, including debugging information.
        /// </summary>
        [Argument("debug")]
        Debug,
    }
}
