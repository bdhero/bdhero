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
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;

namespace BDHero.JobQueue
{
    /// <summary>
    /// Abstract base class representing a release medium (e.g., movie, TV, radio, newspaper).
    /// </summary>
    public abstract class ReleaseMedium
    {
        /// <summary>
        /// TMDb movie ID (e.g., 863) or TVDB show ID.
        /// </summary>
        public int Id;

        /// <summary>
        /// Name of the movie/TV show in its primary release language (e.g., "Toy Story 2" (movie) or "Scrubs" (TV show)).
        /// </summary>
        public string Title;

        /// <summary>
        /// URL of a website with more detailed information about this ReleaseMedium.
        /// </summary>
        public string Url;

        /// <summary>
        /// Gets or sets whether the BD-ROM contains this movie or TV show.
        /// </summary>
        public bool IsSelected;

        /// <summary>
        /// Poster image URLs from TMDb / TVDB.
        /// </summary>
        public readonly IList<ICoverArt> CoverArtImages = new List<ICoverArt>();

        public abstract void Accept(IReleaseMediumVisitor visitor);
    }

    public class Movie : ReleaseMedium
    {
        /// <summary>
        /// Year the movie was released.
        /// </summary>
        public int? ReleaseYear;

        public string ReleaseYearDisplayable
        {
            [CanBeNull]
            get { return ReleaseYear.HasValue && ReleaseYear.Value >= 1800 ? ReleaseYear.Value.ToString("D") : null; }
        }

        public override void Accept(IReleaseMediumVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return ReleaseYearDisplayable == null ? Title : string.Format("{0} ({1})", Title, ReleaseYearDisplayable);
        }

        public Movie Clone()
        {
            var clone = new Movie
                        {
                            Id = Id,
                            Title = Title,
                            Url = Url,
                            IsSelected = IsSelected,
                            ReleaseYear = ReleaseYear
                        };
            clone.CoverArtImages.AddRange(CoverArtImages);
            return clone;
        }
    }

    public class TVShow : ReleaseMedium
    {
        /// <summary>
        /// Collection of all episodes of this TV show ever released throughout all of its seasons.
        /// </summary>
        public readonly IList<Episode> Episodes = new List<Episode>();

        /// <summary>
        /// Gets or sets the index of the episode in <see cref="Episodes"/> selected by the user.
        /// </summary>
        public int SelectedEpisodeIndex;

        /// <summary>
        /// Gets the episode selected by the user.
        /// </summary>
        public Episode SelectedEpisode
        {
            get { return Episodes[SelectedEpisodeIndex]; }
        }

        public class Episode
        {
            public int Id;
            public string Title;
            public int SeasonNumber;
            public int EpisodeNumber;
            public DateTime ReleaseDate;
        }

        public override void Accept(IReleaseMediumVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Title;
        }
    }

    /// <summary>
    /// Enumeration of available <see cref="ReleaseMedium"/> sub-classes.
    /// </summary>
    public enum ReleaseMediumType
    {
        Movie = 1,
        TVShow = 2
    }

    public interface IReleaseMediumVisitor
    {
        void Visit(Movie movie);
        void Visit(TVShow tvShow);
    }
}
