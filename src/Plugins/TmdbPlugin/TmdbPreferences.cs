// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

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

