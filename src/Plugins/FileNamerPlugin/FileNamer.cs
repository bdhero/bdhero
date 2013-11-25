using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.FS;

namespace BDHero.Plugin.FileNamer
{
    internal class FileNamer : IReleaseMediumVisitor
    {
        private readonly Job _job;
        private readonly Preferences _prefs;

        private string _directory;
        private string _fileName;

        public FileNamer(Job job, Preferences prefs)
        {
            _job = job;
            _prefs = prefs;
        }

        public FileNamerPath GetPath()
        {
            Debug.Assert(_job != null);

            var medium = GetOrCreateReleaseMedium();

            medium.Accept(this);

            _directory = ReplaceCommonPlaceholders(_directory, medium);
            _fileName = ReplaceCommonPlaceholders(_fileName, medium);

            ReplaceSpaces();

            return new FileNamerPath(_directory, _fileName + ".mkv");
        }

        private void ReplaceSpaces()
        {
            if (!_prefs.ReplaceSpaces)
                return;

            var replaceWith = FileUtils.SanitizeFileName(_prefs.ReplaceSpacesWith ?? "");

            _fileName = _fileName.Replace(" ", replaceWith);
        }

        private ReleaseMedium GetOrCreateReleaseMedium()
        {
            if (_job.SelectedReleaseMedium != null)
                return _job.SelectedReleaseMedium;
            if (_job.ReleaseMediumType == ReleaseMediumType.TVShow)
                return new TVShow { Title = _job.SearchQuery.Title };
            return new Movie { Title = _job.SearchQuery.Title };
        }

        #region Movies

        public void Visit(Movie movie)
        {
            _directory = _prefs.Movies.Directory;
            _fileName = _prefs.Movies.FileName;

            _directory = ReplaceMoviePlaceholders(_directory, movie);
            _fileName = ReplaceMoviePlaceholders(_fileName, movie);
        }

        private string ReplaceMoviePlaceholders(string fsPart, Movie movie)
        {
            Replace(ref fsPart, "year", movie.ReleaseYearDisplayable);
            return fsPart;
        }

        #endregion

        #region TV Shows

        public void Visit(TVShow tvShow)
        {
            _directory = _prefs.TVShows.Directory;
            _fileName = _prefs.TVShows.FileName;

            _directory = ReplaceTVShowPlaceholders(_directory, tvShow);
            _fileName = ReplaceTVShowPlaceholders(_fileName, tvShow);
        }

        private string ReplaceTVShowPlaceholders(string fsPart, TVShow tvShow)
        {
            var episode = tvShow.SelectedEpisode;
            if (episode == null)
                return fsPart;

            Replace(ref fsPart, "season", episode.SeasonNumber.ToString(_prefs.TVShows.SeasonNumberFormat));
            Replace(ref fsPart, "episode", episode.EpisodeNumber.ToString(_prefs.TVShows.EpisodeNumberFormat));
            Replace(ref fsPart, "date", episode.ReleaseDate.ToString(_prefs.TVShows.ReleaseDateFormat));
            Replace(ref fsPart, "episodetitle", episode.Title);

            return fsPart;
        }

        #endregion

        #region Common

        private string ReplaceCommonPlaceholders(string fsPart, ReleaseMedium releaseMedium)
        {
            Replace(ref fsPart, "volume",   _job.Disc.Metadata.Derived.VolumeLabel);
            Replace(ref fsPart, "title",    releaseMedium.Title);
            Replace(ref fsPart, "res",      GetVideoResolution());
            Replace(ref fsPart, "vcodec",   GetVideoCodec());
            Replace(ref fsPart, "acodec",   GetAudioCodec());
            Replace(ref fsPart, "channels", GetChannelCount());
            Replace(ref fsPart, "cut",      GetCut());
            Replace(ref fsPart, "vlang",    GetVideoLanguage());
            Replace(ref fsPart, "alang",    GetAudioLanguage());

            fsPart = Environment.ExpandEnvironmentVariables(fsPart);

            return fsPart;
        }

        private void Replace(ref string fsPart, string varName, string value)
        {
            fsPart = Regex.Replace(fsPart, string.Format("%{0}%", varName), S(value), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Sanitize variable for use in a file or folder name.
        /// </summary>
        /// <param name="fsPart"></param>
        /// <returns></returns>
        private string S(string fsPart)
        {
            fsPart = fsPart ?? string.Empty;
            return FileUtils.SanitizeFileName(fsPart);
        }

        private string GetVideoResolution()
        {
            var playlist = _job.SelectedPlaylist;
            if (playlist == null)
                return null;
            return playlist.MaxSelectedVideoResolutionDisplayable;
        }

        private string GetVideoCodec()
        {
            var playlist = _job.SelectedPlaylist;
            if (playlist == null)
                return null;
            var videoTracks = playlist.VideoTracks.Where(track => track.Keep).ToArray();
            return videoTracks.Any() ? _prefs.GetCodecName(videoTracks.First().Codec) : null;
        }

        private string GetAudioCodec()
        {
            var playlist = _job.SelectedPlaylist;
            if (playlist == null)
                return null;
            var audioTracks = playlist.AudioTracks.Where(track => track.Keep).OrderByDescending(GetAudioTrackQuality).ToArray();
            return audioTracks.Any() ? _prefs.GetCodecName(audioTracks.First().Codec) : null;
        }

        private double GetAudioTrackQuality(Track track)
        {
            var codec = track.Codec as AudioCodec;
            Debug.Assert(codec != null, "codec != null");
            var multiplier = codec.Lossless ? 2 : 1;
            return track.ChannelCount * multiplier;
        }

        private string GetChannelCount()
        {
            var playlist = _job.SelectedPlaylist;
            if (playlist == null)
                return null;
            var audioTracks = playlist.AudioTracks.Where(track => track.Keep).OrderByDescending(track => track.ChannelCount).ToArray();
            return audioTracks.Any() ? audioTracks.First().ChannelCount.ToString("F1") : null;
        }

        private string GetCut()
        {
            var playlist = _job.SelectedPlaylist;
            if (playlist == null)
                return null;
            return playlist.Cut.ToString();
        }

        private string GetVideoLanguage()
        {
            var playlist = _job.SelectedPlaylist;
            if (playlist == null)
                return null;
            var videoTracks = playlist.VideoTracks.Where(track => track.Keep).ToArray();
            return videoTracks.Any() ? videoTracks.First().Language.ISO_639_2 : null;
        }

        private string GetAudioLanguage()
        {
            var playlist = _job.SelectedPlaylist;
            if (playlist == null)
                return null;
            var audioTracks = playlist.AudioTracks.Where(track => track.Keep).ToArray();
            return audioTracks.Any() ? audioTracks.First().Language.ISO_639_2 : null;
        }

        #endregion
    }
}
