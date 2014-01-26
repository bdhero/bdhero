using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace LicenseUtils
{
    /// <summary>
    ///     A copyrightable piece of software.
    /// </summary>
    [UsedImplicitly]
    public class Work
    {
        /// <summary>
        ///     The full title of the software program.
        /// </summary>
        /// <example>
        ///     <c>"MKVToolNix"</c>
        /// </example>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        ///     Brief summary of the software program's key features.
        /// </summary>
        [JsonProperty("description")]
        public string Description;

        /// <summary>
        ///     List of primary authors who contributed to this work.
        /// </summary>
        [JsonProperty("authors")]
        public Author[] Authors;

        /// <summary>
        ///     Set of URLs to the software's source code, homepage, article, and NuGet package.
        /// </summary>
        [JsonProperty("urls")]
        public Urls Urls;
    }
}
