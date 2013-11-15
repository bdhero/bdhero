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
