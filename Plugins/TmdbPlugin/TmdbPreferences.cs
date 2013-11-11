using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TmdbPlugin
{   
    public class TmdbPreferences        
    {
        /// <summary>
        /// TMDb API key
        /// </summary>
        [JsonProperty(PropertyName = "apiKey")]
        public string ApiKey { get; set; }

        /// <summary>
        /// 2-digit ISO 639-1 language code (lowercase).
        /// </summary>
        /// <example>"en"</example>
        [JsonProperty(PropertyName = "defaultLanguage")]
        public string DefaultLanguage { get; set; }

        public TmdbPreferences()
        {
            ApiKey = "b59b366b0f0a457d58995537d847409a";
            DefaultLanguage = "en";
        }
    }
}

