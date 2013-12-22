using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using Newtonsoft.Json;

namespace BDHero.JobQueue
{
    /// <summary>
    /// Represents a muxing job in the queue.  Contains all information needed to properly mux a single Blu-ray playlist to MKV,
    /// including all user selections such as tracks, languages, chapter names, and cover art.
    /// </summary>
    /// <remarks>
    /// This class can (and should) be serialized to JSON and stored on disk to allow the user to recover their muxing queue if the app crashes.
    /// </remarks>
    public class Job
    {
        public readonly Disc Disc;

        /// <summary>
        /// Playlist selected by the user to mux.
        /// </summary>
        public int SelectedPlaylistIndex;

        /// <summary>
        /// Full absolute path to the muxed output file.
        /// </summary>
        public string OutputPath;

        /// <summary>
        /// Collection of movie search results returned from TMDb.
        /// </summary>
        public readonly IList<Movie> Movies = new List<Movie>();

        /// <summary>
        /// Collection of TV show search results returned from TVDB.
        /// </summary>
        public readonly IList<TVShow> TVShows = new List<TVShow>();

        /// <summary>
        /// Auto-detected or user-selected release medium (movie or TV).
        /// </summary>
        public ReleaseMediumType ReleaseMediumType;

        /// <summary>
        /// Gets or sets the playlist selected by the user.
        /// </summary>
        [JsonIgnore]
        public Playlist SelectedPlaylist
        {
            get { return SelectedPlaylistIndex > -1 ? Disc.Playlists[SelectedPlaylistIndex] : null; }
            set { SelectedPlaylistIndex = value != null ? Disc.Playlists.IndexOf(value) : -1; }
        }

        /// <summary>
        /// Gets the movie selected by the user.
        /// </summary>
        [JsonIgnore]
        public Movie SelectedMovie { get { return Movies.FirstOrDefault(movie => movie.IsSelected); } }

        /// <summary>
        /// Gets the playlist selected by the user.
        /// </summary>
        [JsonIgnore]
        public TVShow SelectedTVShow { get { return TVShows.FirstOrDefault(show => show.IsSelected); } }

        /// <summary>
        /// Gets the selected <see cref="Movie"/> or <see cref="TVShow"/>.
        /// </summary>
        [JsonIgnore]
        public ReleaseMedium SelectedReleaseMedium
        {
            get { return ReleaseMediumType == ReleaseMediumType.TVShow ? (ReleaseMedium) SelectedTVShow : SelectedMovie; }
        }

        [JsonIgnore]
        public SearchQuery SearchQuery
        {
            get { return _searchQuery ?? (Disc != null ? Disc.Metadata.Derived.BestSearchQuery : null); }
            set { _searchQuery = value; }
        }

        [JsonIgnore]
        public IList<SearchQuery> SearchQueries
        {
            get
            {
                if (Disc == null)
                {
                    return null;
                }

                var customQuery = SearchQuery;
                var derivedQueries = Disc.Metadata.Derived.SearchQueries;

                if (derivedQueries.Contains(customQuery))
                {
                    return derivedQueries;
                }

                var allQueries = new List<SearchQuery>();
                allQueries.Add(customQuery);
                allQueries.AddRange(derivedQueries);
                return allQueries;
            }
        }

        private SearchQuery _searchQuery;

        public Job(Disc disc)
        {
            Disc = disc;
        }
    }
}
