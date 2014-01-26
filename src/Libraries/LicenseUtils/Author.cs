using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace LicenseUtils
{
    /// <summary>
    ///     A person or organization that contributed to a <see cref="Work"/>.
    /// </summary>
    [UsedImplicitly]
    public class Author
    {
        /// <summary>
        ///     Array of years that this author made copyrightable contributions to the <see cref="Work"/>.
        /// </summary>
        [JsonProperty("years")]
        public int[] Years;

        /// <summary>
        ///     The person's full name.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        ///     The name of the organization that commissioned the <see cref="Work"/>.
        /// </summary>
        [JsonProperty("organization")]
        public string Organization;

        /// <summary>
        ///     The author's primary contact email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email;

        /// <summary>
        ///     The author's homepage URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url;
    }
}
