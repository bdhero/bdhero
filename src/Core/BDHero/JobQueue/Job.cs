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
