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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using I18N;

namespace BDHeroGUI.Components
{
    public partial class AudioTrackListView : UserControl
    {
        public event PlaylistReconfiguredEventHandler PlaylistReconfigured;

        private readonly TrackListViewHelper _helper;

        public Func<Track, bool> Filter = track => true;

        public AudioTrackListView()
        {
            InitializeComponent();
            _helper = new TrackListViewHelper(listViewAudioTracks, track => track.IsAudio && Filter(track), GetListItem);
            Load += _helper.OnLoad;
            _helper.PlaylistReconfigured += HelperOnPlaylistReconfigured;

            // Set initial sort order for columns that should default to descending instead of ascending
            columnHeaderChannels.Tag = SortOrder.Descending;
        }

        public void SetPlaylist(Playlist playlist, Language[] allLanguages)
        {
            _helper.SetPlaylist(playlist, allLanguages);
        }

        private void HelperOnPlaylistReconfigured(Playlist playlist)
        {
            if (PlaylistReconfigured != null)
                PlaylistReconfigured(playlist);
        }

        private static ICollection<ListViewCell> GetListItem(Track track)
        {
            return new[]
                {
                    new ListViewCell { Text = track.Codec.DisplayName },
                    new ListViewCell { Text = track.ChannelCount.ToString("F1"), Tag = track.ChannelCount },
                    new ListViewCell { Text = track.Language.Name, Tag = track.Language },
                    new ListViewCell { Text = track.Type.ToString(), Tag = track.Type },
                    new ListViewCell { Text = (track.IndexOfType + 1).ToString("D"), Tag = track.IndexOfType }
                };
        }
    }
}
