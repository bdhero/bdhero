using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UpdateLib
{
    public class UpdateResponse
    {
        [JsonProperty(PropertyName = "version")]
        [JsonConverter(typeof(VersionConverter))]
        public Version Version { get; set; }

        /// <summary>
        /// ISO 8601 date format (e.g. <c>2008-04-12T12:53Z</c>).  See <see cref="IsoDateTimeConverter"/>.
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "mirrors")]
        public List<string> Mirrors { get; set; }

        [JsonProperty(PropertyName = "platforms")]
        public PlatformList Platforms { get; set; }

        [JsonProperty(PropertyName = "releaseNotes")]
        public string ReleaseNotes { get; set; }

        public UpdateResponse()
        {
            Version = new Version();
            Date = DateTime.Now;
            Mirrors = new List<string>();
            Platforms = new PlatformList();
            ReleaseNotes = "";
        }
    }

    public class PlatformList
    {
        [JsonProperty(PropertyName = "windows")]
        public Platform Windows { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "mac")]
        public Platform Mac { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "linux")]
        public Platform Linux { get; set; }

        public PlatformList()
        {
            Windows = new Platform();
            Mac = new Platform();
            Linux = new Platform();
        }
    }

    public class Platform
    {
        [JsonProperty(PropertyName = "packages")]
        public PackageList Packages { get; set; }

        public Platform()
        {
            Packages = new PackageList();
        }
    }

    public class PackageList
    {
        [JsonProperty(PropertyName = "setup")]
        public Package Setup { get; set; }

        [JsonProperty(PropertyName = "sfx")]
        public Package Sfx { get; set; }

        [JsonProperty(PropertyName = "sevenZip")]
        public Package SevenZip { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public Package Zip { get; set; }
    }

    public class Package
    {
        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "sha1")]
        public string SHA1 { get; set; }

        [JsonProperty(PropertyName = "size")]
        public long Size { get; set; }
    }
}
