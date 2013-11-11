using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class PlaylistTransformer
    {
        /// <summary>
        /// Returns a List of TSPlaylistFile objects from the given Dictionary.
        /// </summary>
        public static List<TSPlaylistFile> Transform(IEnumerable<KeyValuePair<string, TSPlaylistFile>> tsPlaylistFiles)
        {
            return tsPlaylistFiles.Select(pair => pair.Value).ToList();
        }

        /// <summary>
        /// Transforms a List of TSPlaylistFile objects into a List of Playlist objects.
        /// </summary>
        /// <param name="playlistFiles"></param>
        /// <returns></returns>
        public static List<Playlist> Transform(IEnumerable<TSPlaylistFile> playlistFiles)
        {
            return playlistFiles.OrderBy(file => file.Name).Select(Transform).ToList();
        }

        public static Playlist Transform(TSPlaylistFile playlistFile)
        {
            return new Playlist
            {
                FileName = playlistFile.Name,
                FullPath = playlistFile.FullName,
                FileSize = playlistFile.FileSize,
                Length = TimeSpan.FromMilliseconds(playlistFile.TotalLength * 1000),
                StreamClips = StreamClipTransformer.Transform(playlistFile.StreamClips),
                Tracks = TrackTransformer.Transform(playlistFile.SortedStreams),
                Chapters = ChapterTransformer.Transform(playlistFile.Chapters),
                HasDuplicateStreamClips = playlistFile.HasDuplicateClips,
                HasLoops = playlistFile.HasLoops
            };
        }
    }
}
