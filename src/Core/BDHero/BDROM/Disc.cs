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

using System.Collections.Generic;
using System.Linq;
using I18N;
using Newtonsoft.Json;

namespace BDHero.BDROM
{
    /// <summary>
    /// Represents a BD-ROM disc.
    /// </summary>
    public class Disc
    {
        #region DB Fields

        public DiscMetadata Metadata;

        /// <summary>
        /// Primary release language of the disc.
        /// </summary>
        [JsonProperty(PropertyName = "primary_language")]
        public Language PrimaryLanguage = Language.Undetermined;

        /// <summary>
        /// List of all playlists in the order they appear on the disc.
        /// </summary>
        [JsonProperty(PropertyName = "playlists")]
        public List<Playlist> Playlists = new List<Playlist>();

        #endregion

        #region Constructor

        public Disc()
        {
            Languages = new List<Language>();
        }

        #endregion

        #region Non-DB Properties

        public DiscFileSystem FileSystem;

        public DiscFeatures Features;

        /// <summary>
        /// Returns a list of all languages found on the disc, with the primary disc language first if it can be automatically detected.
        /// </summary>
        [JsonIgnore]
        public IList<Language> Languages { get; private set; }

        [JsonIgnore]
        public IList<Playlist> ValidMainFeaturePlaylists
        {
            get { return Playlists.Where(playlist => playlist.IsMainFeature && !playlist.IsBogus).ToList(); }
        }

        #endregion
    }
}
