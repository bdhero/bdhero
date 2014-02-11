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
using System.Text.RegularExpressions;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using I18N;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class DiscTransformer
    {
        public static Disc Transform(BDInfo.BDROM bdrom)
        {
            var tsPlaylistFiles = PlaylistTransformer.Transform(bdrom.PlaylistFiles);

            var disc =
                new Disc
                    {
                        PrimaryLanguage = bdrom.DiscLanguage,
                        Playlists = PlaylistTransformer.Transform(tsPlaylistFiles)
                    };

            DiscFileSystemTransformer.Transform(bdrom, disc);
            DiscFeaturesTransformer.Transform(disc);
            DiscMetadataTransformer.Transform(disc);

            // Data gathering
            TransformPrimaryLanguage(disc);
            TransformVideoLanguages(disc);
            TransformLanguageList(disc);
            TransformTitle(disc);

            return disc;
        }

        #region Data Gathering

        private static void TransformPrimaryLanguage(Disc disc)
        {
            if (disc.PrimaryLanguage != null) return;

            disc.PrimaryLanguage = disc.Playlists.SelectMany(playlist => playlist.AudioTracks)
                                       .Select(track => track.Language)
                                       .GroupBy(language => language)
                                       .OrderByDescending(grouping => grouping.Count())
                                       .Select(grouping => grouping.Key)
                                       .FirstOrDefault();
        }

        private static void TransformVideoLanguages(Disc disc)
        {
            if (disc.PrimaryLanguage == null) return;

            foreach (var videoTrack in disc.Playlists.SelectMany(playlist => playlist.VideoTracks))
            {
                videoTrack.Language = disc.PrimaryLanguage;
            }
        }

        private static void TransformLanguageList(Disc disc)
        {
            var playlists = disc.Playlists;
            var languages = disc.Languages;
            var primaryLanguage = disc.PrimaryLanguage;

            // Sort languages alphabetically
            var languagesWithDups =
                    playlists
                        .SelectMany(playlist => playlist.Tracks)
                        .Select(track => track.Language)
                        .Where(language => language != null && language != Language.Undetermined);

            languages.Clear();
            languages.AddRange(new HashSet<Language>(languagesWithDups).OrderBy(language => language.Name));

            if (primaryLanguage == null || primaryLanguage == Language.Undetermined) return;

            // Move primary language to the beginning of the list
            languages.Remove(primaryLanguage);
            languages.Insert(0, primaryLanguage);
        }

        private static void TransformTitle(Disc disc)
        {
            var derived = disc.Metadata.Derived;

            var validBdmtTitles = derived.ValidBdmtTitles;
            if (validBdmtTitles.ContainsKey(disc.PrimaryLanguage))
            {
                AddSearchableTitleIfNotEmpty(disc, validBdmtTitles[disc.PrimaryLanguage]);
            }

            AddSearchableTitleIfNotEmpty(disc, derived.DboxTitleSanitized);
            AddSearchableTitleIfNotEmpty(disc, derived.VolumeLabelSanitized);
        }

        private static void AddSearchableTitleIfNotEmpty(Disc disc, [CanBeNull] string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
                disc.Metadata.Derived.SearchQueries.Add(new SearchQuery { Title = query, Language = disc.PrimaryLanguage });
        }

        #endregion
    }

    
}
