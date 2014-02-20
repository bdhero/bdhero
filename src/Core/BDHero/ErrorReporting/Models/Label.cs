using System.Diagnostics;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace BDHero.ErrorReporting.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("{Name}")]
    public class Label
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("color")]
        public string Color;
    }
}