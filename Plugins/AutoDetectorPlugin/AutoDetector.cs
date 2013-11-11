using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils;
using DotNetUtils.Extensions;
using I18N;

namespace BDHero.Plugin.AutoDetector
{
    public class AutoDetector : IAutoDetectorPlugin
    {
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
                        select bdinfoDuplicateMap[key].OrderBy(playlist => playlist.Tracks.Count(track => track.IsHidden)))
                .SelectMany(sortedFiles => sortedFiles.Skip(1));

            foreach (var playlist in dups)
            {
                playlist.IsDuplicate = true;
                bdamPlaylistMap[playlist.FileName].IsDuplicate = true;
            }
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
            // TODO: ONLY LOOK AT MAIN MOVIE PLAYLISTS

            var bestAudioPlaylist = disc.Playlists.OrderByDescending(p => p.MaxAudioChannels).FirstOrDefault();
            if (bestAudioPlaylist == null) return;

            var bestVideoPlaylist = disc.Playlists.OrderByDescending(p => p.MaxAvailableVideoResolution).FirstOrDefault();
            if (bestVideoPlaylist == null) return;

            var maxChannels = bestAudioPlaylist.MaxAudioChannels;
            var maxHeight = bestVideoPlaylist.MaxAvailableVideoResolution;

            var maxQualityPlaylists = disc.Playlists.Where(playlist => playlist.MaxAudioChannels == maxChannels && playlist.MaxAvailableVideoResolution == maxHeight);

            foreach (var playlist in maxQualityPlaylists)
            {
                playlist.IsMaxQuality = true;
            }
        }

        #endregion

        #region Auto Detection

        private static void DetectPlaylistTypes(Disc disc)
        {
            var maxLength =
                disc.Playlists
                    .OrderByDescending(p => p.Length)
                    .Where(playlist => !playlist.IsBogus && playlist.IsMaxQuality)
                    .Select(playlist => playlist.Length)
                    .FirstOrDefault();

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

                        if (IsMainFeatureAudioTrack(audioTrack))
                            audioTrack.Type = TrackType.MainFeature;

                        if (IsCommentaryAudioTrack(audioTrack))
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

        private static bool IsMainFeatureAudioTrack(Track track)
        {
            return track.Index == 0 ||
                   track.Index >= 1 && track.ChannelCount > 2;
        }

        private static bool IsCommentaryAudioTrack(Track track)
        {
            return track.Index >= 1 && track.ChannelCount <= 2;
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

                playlist.VideoTracks.First().IsBestGuess = true;
                playlist.VideoTracks.First().Keep = true;

                // Audio

                var mainFeatureAudioTracks = playlist.AudioTracks.Where(track => track.IsMainFeature).ToList();
                var primaryLanguageAudioTracks = mainFeatureAudioTracks.Where(track => track.Language == disc.PrimaryLanguage).ToList();
                var firstAudioTrack = playlist.AudioTracks.FirstOrDefault();

                if (primaryLanguageAudioTracks.Any())
                    SelectTracks(primaryLanguageAudioTracks);
                else if (mainFeatureAudioTracks.Any())
                    SelectTrack(primaryLanguageAudioTracks.First());
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

        private static void SelectTrack(Track track)
        {
            track.IsBestGuess = true;
            track.Keep = true;
        }

        #endregion

    }
}
