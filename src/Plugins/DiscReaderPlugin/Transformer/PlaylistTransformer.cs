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
