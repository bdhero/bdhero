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

using System.Windows.Forms;
using BDHero.BDROM;
using BDHeroGUI.Forms;
using I18N;

namespace BDHeroGUI.Components
{
    public partial class TracksPanel : UserControl
    {
        private Playlist _playlist;
        private Language[] _allLanguages = new Language[0];

        public event PlaylistReconfiguredEventHandler PlaylistReconfigured;

        /// <summary>
        /// Gets or sets whether all playlists are shown, regardless of the user's filter settings.
        /// </summary>
        public bool ShowAll
        {
            get { return _showAllTracks; }
            set
            {
                _showAllTracks = value;
                RefreshPlaylist();
            }
        }

        private readonly TrackFilter _filter = new TrackFilter();

        private bool _showAllTracks;

        public TracksPanel()
        {
            InitializeComponent();

            videoTrackListView.Filter = ShowTrack;
            audioTrackListView.Filter = ShowTrack;
            subtitleTrackListView.Filter = ShowTrack;

            videoTrackListView.PlaylistReconfigured += HelperOnPlaylistReconfigured;
            audioTrackListView.PlaylistReconfigured += HelperOnPlaylistReconfigured;
            subtitleTrackListView.PlaylistReconfigured += HelperOnPlaylistReconfigured;
        }

        public void ShowFilterWindow()
        {
            var result = new FormTrackFilter(_filter).ShowDialog(this);

            if (result == DialogResult.OK)
            {
                RefreshPlaylist();
            }
        }

        public void SetPlaylist(Playlist playlist, Language[] allLanguages)
        {
            _playlist = playlist;
            _allLanguages = allLanguages;
            videoTrackListView.SetPlaylist(playlist, allLanguages);
            audioTrackListView.SetPlaylist(playlist, allLanguages);
            subtitleTrackListView.SetPlaylist(playlist, allLanguages);
        }

        private bool ShowTrack(Track track)
        {
            return _filter.Show(track) || _showAllTracks;
        }

        private void RefreshPlaylist()
        {
            SetPlaylist(_playlist, _allLanguages);
        }

        private void HelperOnPlaylistReconfigured(Playlist playlist)
        {
            if (PlaylistReconfigured != null)
                PlaylistReconfigured(playlist);
        }
    }
}
