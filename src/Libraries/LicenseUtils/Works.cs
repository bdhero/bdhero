using Newtonsoft.Json;

namespace LicenseUtils
{
    /// <summary>
    ///     Collection of <see cref="Work"/>s distributed with BDHero.
    /// </summary>
    public class Works
    {
        /// <summary>
        ///     Original works that were not derived from another source.
        /// </summary>
        /// <example>
        ///     BDInfo.
        /// </example>
        [JsonProperty("originals")]
        public Work[] Originals;

        /// <summary>
        ///     Works derived from <see cref="Originals"/>.
        /// </summary>
        /// <example>
        ///     BDHero.
        /// </example>
        [JsonProperty("derivatives")]
        public Work[] Derivatives;

        /// <summary>
        ///     Short code snippets or individual standalone source files.
        /// </summary>
        /// <example>
        ///     Plug-ins in C#, Detecting USB Drive Removal in a C# Program, JobObject.cs.
        /// </example>
        [JsonProperty("snippets")]
        public Work[] Snippets;

        /// <summary>
        ///     NuGet packages.
        /// </summary>
        /// <example>
        ///     Json.NET.
        /// </example>
        [JsonProperty("packages")]
        public Work[] Packages;

        /// <summary>
        ///     Compiled libraries (DLLs).
        /// </summary>
        /// <example>
        ///     Windows API Code Pack.
        /// </example>
        [JsonProperty("libraries")]
        public Work[] Libraries;

        /// <summary>
        ///     Executable binary software packages (EXEs).
        /// </summary>
        /// <example>
        ///     FFmpeg, MKVToolNix.
        /// </example>
        [JsonProperty("binaries")]
        public Work[] Binaries;
    }
}
