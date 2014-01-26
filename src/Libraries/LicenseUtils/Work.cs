using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("authors")]
        public Author[] Authors;

        [JsonProperty("urls")]
        public Urls Urls;
    }
}
