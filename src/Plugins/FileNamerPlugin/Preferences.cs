// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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

namespace BDHero.Plugin.FileNamer
{
    internal class Preferences
    {
        public MoviePreferences Movies = new MoviePreferences();
        public TVShowPreferences TVShows = new TVShowPreferences();

        public IDictionary<string, string> Codecs = new Dictionary<string, string>();

        public bool ReplaceSpaces;
        public string ReplaceSpacesWith = ".";

        public Preferences()
        {
            foreach (var codec in Codec.MuxableBDCodecs)
            {
                Codecs[codec.SerializableName] = codec.FileName;
            }
        }

        public string GetCodecName(Codec codec)
        {
            return Codecs.ContainsKey(codec.SerializableName) ? Codecs[codec.SerializableName] : null;
        }

        public Preferences CopyFrom(Preferences other)
        {
            Movies.CopyFrom(other.Movies);
            TVShows.CopyFrom(other.TVShows);

            Codecs.Clear();
            foreach (var k in other.Codecs.Keys)
            {
                Codecs[k] = other.Codecs[k];
            }

            ReplaceSpaces = other.ReplaceSpaces;
            ReplaceSpacesWith = other.ReplaceSpacesWith;

            return this;
        }

        public Preferences Clone()
        {
            return new Preferences().CopyFrom(this);
        }

        #region Equality members

        protected bool Equals(Preferences other)
        {
            return Equals(Movies, other.Movies) &&
                   Equals(TVShows, other.TVShows) &&
                   Codecs.SequenceEqual(other.Codecs) &&
                   ReplaceSpaces.Equals(other.ReplaceSpaces) &&
                   string.Equals(ReplaceSpacesWith, other.ReplaceSpacesWith);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Preferences) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Movies != null ? Movies.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TVShows != null ? TVShows.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Codecs != null ? Codecs.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ ReplaceSpaces.GetHashCode();
                hashCode = (hashCode*397) ^ (ReplaceSpacesWith != null ? ReplaceSpacesWith.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }

    internal abstract class ReleaseMediumPreferences
    {
        public string Directory = @"%temp%\%volume%";
        public string FileName = @"%title% [%res%]";

        public void CopyFrom(ReleaseMediumPreferences other)
        {
            Directory = other.Directory;
            FileName = other.FileName;
        }

        #region Equality members

        protected bool Equals(ReleaseMediumPreferences other)
        {
            return string.Equals(Directory, other.Directory) && string.Equals(FileName, other.FileName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ReleaseMediumPreferences) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Directory != null ? Directory.GetHashCode() : 0)*397) ^ (FileName != null ? FileName.GetHashCode() : 0);
            }
        }

        #endregion
    }

    internal class MoviePreferences : ReleaseMediumPreferences
    {
        public new string FileName = @"%title% (%year%) [%res%] [%vcodec%] [%acodec% %channels%]";

        public void CopyFrom(MoviePreferences other)
        {
            base.CopyFrom(other);
            FileName = other.FileName;
        }

        #region Equality members

        protected bool Equals(MoviePreferences other)
        {
            return base.Equals(other) && string.Equals(FileName, other.FileName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MoviePreferences) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (FileName != null ? FileName.GetHashCode() : 0);
            }
        }

        #endregion
    }

    internal class TVShowPreferences : ReleaseMediumPreferences
    {
        public new string Directory = @"%temp%\%title%\Season %season%";
        public new string FileName = @"s%season%e%episode% - %episodetitle% [%res%]";

        /// <summary>
        /// Format string for <see cref="int.ToString(string)"/>.
        /// </summary>
        public string SeasonNumberFormat = "D2";

        /// <summary>
        /// Format string for <see cref="int.ToString(string)"/>.
        /// </summary>
        public string EpisodeNumberFormat = "D2";

        /// <summary>
        /// Format string for <see cref="DateTime.ToString(string)"/>.
        /// </summary>
        public string ReleaseDateFormat = "yyyy-MM-dd";

        public void CopyFrom(TVShowPreferences other)
        {
            base.CopyFrom(other);
            Directory = other.Directory;
            FileName = other.FileName;
            SeasonNumberFormat = other.SeasonNumberFormat;
            EpisodeNumberFormat = other.EpisodeNumberFormat;
            ReleaseDateFormat = other.ReleaseDateFormat;
        }

        #region Equality members

        protected bool Equals(TVShowPreferences other)
        {
            return base.Equals(other) && string.Equals(Directory, other.Directory) &&
                   string.Equals(FileName, other.FileName) &&
                   string.Equals(SeasonNumberFormat, other.SeasonNumberFormat) &&
                   string.Equals(EpisodeNumberFormat, other.EpisodeNumberFormat) &&
                   string.Equals(ReleaseDateFormat, other.ReleaseDateFormat);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TVShowPreferences) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (Directory != null ? Directory.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FileName != null ? FileName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SeasonNumberFormat != null ? SeasonNumberFormat.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EpisodeNumberFormat != null ? EpisodeNumberFormat.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ReleaseDateFormat != null ? ReleaseDateFormat.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }
}
