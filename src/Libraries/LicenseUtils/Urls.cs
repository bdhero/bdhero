using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace LicenseUtils
{
    /// <summary>
    ///     Common URLs for a <see cref="Work"/>.
    /// </summary>
    [UsedImplicitly]
    public class Urls
    {
        /// <summary>
        ///     URL of the article, blog or forum post that linked to (or contained) the source code.
        /// </summary>
        /// <example>
        ///     <c>"http://www.codeproject.com/Articles/6334/Plug-ins-in-C"</c>
        /// </example>
        [JsonProperty("article")]
        public string Article;

        /// <summary>
        ///     NuGet package URL.
        /// </summary>
        /// <example>
        ///     <c>"http://www.nuget.org/packages/Ninject/"</c>
        /// </example>
        [JsonProperty("package")]
        public string Package;

        /// <summary>
        ///     Project homepage URL.
        /// </summary>
        /// <example>
        ///     <c>"http://www.ninject.org/"</c>
        /// </example>
        [JsonProperty("project")]
        public string Project;

        /// <summary>
        ///     URL where the source code can be obtained.
        /// </summary>
        /// <example>
        ///     <c>"https://github.com/ninject/ninject"</c>
        /// </example>
        [JsonProperty("source")]
        public string Source;
    }
}