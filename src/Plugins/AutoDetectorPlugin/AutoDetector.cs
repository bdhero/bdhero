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
using System.Drawing;
using System.Linq;
using System.Threading;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using I18N;

namespace BDHero.Plugin.AutoDetector
{
    [UsedImplicitly]
    public class AutoDetector : IAutoDetectorPlugin
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "BDHero Detective"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return null; } }

        public int RunOrder { get { return 0; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void AutoDetect(CancellationToken cancellationToken, Job job)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 0.0, "Gathering data...");

            var disc = job.Disc;

            // Data gathering (round 1)
            FindDuplicatePlaylists(disc);
            TransformPlaylistQuality(disc);

            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 25.0, "Auto-detecting track types...");

            // Auto-configuration
            DetectPlaylistTypes(disc);
            DetectMainFeaturePlaylistTrackTypes(disc);
            DetectCommentaryPlaylistTrackTypes(disc);
            DetectSpecialFeaturePlaylistTrackTypes(disc);

            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 75.0, "Auto-selecting best playlists and tracks...");

            SelectBestPlaylist(job);
            SelectBestTracks(disc);

            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 100.0, "Finished auto detecting");
        }

        #region Data Gathering (round 1)

        private void FindDuplicatePlaylists(Disc disc)
        {
            var bdamPlaylistMap = disc.Playlists.ToDictionary(playlist => playlist.FileName);
            var bdinfoDuplicateMap = disc.Playlists.ToMultiValueDictionary(GetPlaylistDupKey);

            var dups = (from key in bdinfoDuplicateMap.Keys
                        where bdinfoDuplicateMap[key].Count > 1
                        select bdinfoDuplicateMap[key].OrderBy(GetDupSortValue))
                .SelectMany(SkipBestDup);

            foreach (var playlist in dups)
            {
                playlist.IsDuplicate = true;
                bdamPlaylistMap[playlist.FileName].IsDuplicate = true;
            }
        }

        private static int GetDupSortValue(Playlist playlist)
        {
            return (playlist.IsBogusFoSho ? 1 : 0) + playlist.Tracks.Count(track => track.IsHidden);
        }

        private static IEnumerable<Playlist> SkipBestDup(IEnumerable<Playlist> playlistsEnumerable)
        {
            var playlists = playlistsEnumerable.ToArray();
            var nonBogus = playlists.Where(playlist => !playlist.IsBogusFoSho).ToArray();
            if (nonBogus.Any())
                return nonBogus.Except(new[] { nonBogus.First() });
            return playlists.Skip(1);
        }

        private static string GetPlaylistDupKey(Playlist playlist)
        {
            var streamClips = string.Join(",", playlist.StreamClips.Select(GetStreamClipDupKey));
            var chapters = string.Join(",", playlist.Chapters.Select(GetChapterDupKey));
            var key = string.Format("{0}/{1}=[{2}]+[{3}]", playlist.Length, playlist.FileSize, streamClips, chapters);
            return key;
        }

        private static string GetStreamClipDupKey(StreamClip clip)
        {
            return string.Format("{0}/{1}/{2}", clip.FileName, clip.Length, clip.FileSize);
        }

        private static string GetChapterDupKey(Chapter chapter)
        {
            return string.Format("{0}/{1}", chapter.Number, chapter.StartTime);
        }

        private void TransformPlaylistQuality(Disc disc)
        {
            // TODO: Fix or remove quality detection

            // Only consider feature-length playlists when searching for highest quality tracks
            var maxLength = GetMaxPlaylistLength(disc);
            var allPlaylists = disc.Playlists;
            var featureLengthPlaylists = allPlaylists.Where(playlist => playlist.IsFeatureLength(maxLength)).ToArray();

            var bestAudioPlaylist = featureLengthPlaylists.OrderByDescending(p => p.MaxAudioChannels).FirstOrDefault();
            if (bestAudioPlaylist == null) return;

            var bestVideoPlaylist = featureLengthPlaylists.OrderByDescending(p => p.MaxAvailableVideoResolution).FirstOrDefault();
            if (bestVideoPlaylist == null) return;

            var maxChannels = bestAudioPlaylist.MaxAudioChannels;
            var maxHeight = bestVideoPlaylist.MaxAvailableVideoResolution;

            var maxQualityPlaylists = allPlaylists.Where(playlist => IsMaxQualityPlaylist(playlist, maxChannels, maxHeight)).ToArray();

            foreach (var playlist in maxQualityPlaylists)
            {
                playlist.IsMaxQuality = true;
            }
        }

        private static bool IsMaxQualityPlaylist(Playlist playlist, double maxChannels, int maxHeight)
        {
            return playlist.MaxAudioChannels >= maxChannels && playlist.MaxAvailableVideoResolution == maxHeight;
        }

        #endregion

        private static TimeSpan GetMaxPlaylistLength(Disc disc)
        {
            return disc.Playlists
                       .Where(playlist => playlist.IsPossibleMainFeature && !playlist.IsBogus)
                       .Select(playlist => playlist.Length)
                       .OrderByDescending(length => length)
                       .FirstOrDefault();
        }

        #region Auto Detection

        private static void DetectPlaylistTypes(Disc disc)
        {
            var maxLength = GetMaxPlaylistLength(disc);

            foreach (var playlist in disc.Playlists)
            {
                if (playlist.IsMainFeaturePlaylist(maxLength))
                    playlist.Type = TrackType.MainFeature;

                if (playlist.IsSpecialFeaturePlaylist(maxLength))
                    playlist.Type = TrackType.SpecialFeature;
            }
        }

        private static void DetectMainFeaturePlaylistTrackTypes(Disc disc)
        {
            foreach (var playlist in disc.Playlists.Where(playlist => playlist.IsMainFeature))
            {
                // Additional video tracks are usually PiP commentary
                foreach (var videoTrack in playlist.VideoTracks.Skip(1))
                {
                    videoTrack.Type = TrackType.Commentary;
                }

                var audioLanguages = new HashSet<Language>(playlist.AudioTracks.Select(track => track.Language)).ToArray();
                var subtitleLanguages = new HashSet<Language>(playlist.SubtitleTracks.Select(track => track.Language)).ToArray();

                // Detect type of audio tracks (per-language)
                foreach (var audioLanguage in audioLanguages)
                {
                    var lang = audioLanguage;
                    var audioTracksWithLang = playlist.AudioTracks.Where(track => track.Language == lang).ToArray();

                    // Detect type of audio tracks
                    for (var i = 0; i < audioTracksWithLang.Length; i++)
                    {
                        var audioTrack = audioTracksWithLang[i];

                        if (IsMainFeatureAudioTrack(audioTrack, i))
                            audioTrack.Type = TrackType.MainFeature;

                        if (IsCommentaryAudioTrack(audioTrack, i))
                            audioTrack.Type = TrackType.Commentary;
                    }
                }

                // Detect type of audio tracks (per-language)
                // Assume the first subtitle track of every language is the Main Feature,
                // and all other subtitle tracks of that language are Commentary
                foreach (var subtitleLanguage in subtitleLanguages)
                {
                    var lang = subtitleLanguage;
                    var subtitleTracksWithLang = playlist.SubtitleTracks.Where(track => track.Language == lang).ToArray();

                    // Detect type of subtitle tracks
                    for (var i = 0; i < subtitleTracksWithLang.Length; i++)
                    {
                        var subtitleTrack = subtitleTracksWithLang[i];
                        subtitleTrack.Type = i == 0 ? TrackType.MainFeature : TrackType.Commentary;

                        var codec = subtitleTrack.Codec;
                        if (!codec.IsKnown || !codec.IsMuxable)
                        {
                            subtitleTrack.Type = TrackType.Misc;
                        }
                    }
                }
            }
        }

        private static bool IsMainFeatureAudioTrack(Track track, int indexOfTypeWithSameLanguage)
        {
            return indexOfTypeWithSameLanguage == 0 ||
                   indexOfTypeWithSameLanguage >= 1 && track.ChannelCount > 2;
        }

        private static bool IsCommentaryAudioTrack(Track track, int indexOfTypeWithSameLanguage)
        {
            return indexOfTypeWithSameLanguage >= 1 && track.ChannelCount <= 2;
        }

        private static void DetectCommentaryPlaylistTrackTypes(Disc disc)
        {
            var tracks = disc.Playlists.Where(playlist => playlist.IsVideoCommentary).SelectMany(playlist => playlist.Tracks);
            foreach (var track in tracks)
            {
                track.Type = TrackType.Commentary;
            }
        }

        private static void DetectSpecialFeaturePlaylistTrackTypes(Disc disc)
        {
            var tracks = disc.Playlists.Where(playlist => playlist.IsSpecialFeature).SelectMany(playlist => playlist.Tracks);
            foreach (var track in tracks)
            {
                track.Type = TrackType.SpecialFeature;
            }
        }

        #endregion

        #region Auto Selection

        private static void SelectBestPlaylist(Job job)
        {
            var bestPlaylists = job.Disc.ValidMainFeaturePlaylists;
            var bestPlaylist = bestPlaylists.FirstOrDefault();

            if (bestPlaylist == null) return;

            bestPlaylists.ForEach(playlist => playlist.IsBestGuess = true);
            job.SelectedPlaylistIndex = job.Disc.Playlists.IndexOf(bestPlaylist);
        }

        private static void SelectBestTracks(Disc disc)
        {
            foreach (var playlist in disc.Playlists)
            {
                // Video

                var firstVideoTrack = playlist.VideoTracks.FirstOrDefault();
                if (firstVideoTrack == null)
                {
                    Logger.WarnFormat("Playlist {0} has no video tracks - skipping");
                    continue;
                }

                SelectTrack(firstVideoTrack);

                // Audio

                var mainFeatureAudioTracks = playlist.AudioTracks.Where(track => track.IsMainFeature).ToList();
                var primaryLanguageAudioTracks = mainFeatureAudioTracks.Where(track => track.Language == disc.PrimaryLanguage).ToList();
                var firstAudioTrack = playlist.AudioTracks.FirstOrDefault();

                if (primaryLanguageAudioTracks.Any())
                    SelectTracks(primaryLanguageAudioTracks);
                else if (mainFeatureAudioTracks.Any())
                    SelectTrack(mainFeatureAudioTracks.First());
                else if (firstAudioTrack != null)
                    SelectTrack(firstAudioTrack);

                // Subtitles

                var mainFeatureSubtitleTracks = playlist.SubtitleTracks.Where(track => track.IsMainFeature).ToList();
                var primaryLanguageSubtitleTracks = mainFeatureSubtitleTracks.Where(track => track.Language == disc.PrimaryLanguage).ToList();
                var firstSubtitleTrack = playlist.SubtitleTracks.FirstOrDefault();

                if (primaryLanguageSubtitleTracks.Any())
                    SelectTracks(primaryLanguageSubtitleTracks);
                else if (mainFeatureSubtitleTracks.Any())
                    SelectTrack(mainFeatureSubtitleTracks.First());
                else if (firstSubtitleTrack != null)
                    SelectTrack(firstSubtitleTrack);
            }
        }

        private static void SelectTracks(IEnumerable<Track> tracks)
        {
            foreach (var track in tracks)
            {
                SelectTrack(track);
            }
        }

        private static void SelectTrack([CanBeNull] Track track)
        {
            if (track == null)
                return;

            track.IsBestGuess = true;
            track.Keep = true;
        }

        #endregion

    }
}
