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
    public partial class SubtitleTrackListView : UserControl
    {
        public event PlaylistReconfiguredEventHandler PlaylistReconfigured;

        private readonly TrackListViewHelper _helper;

        public Func<Track, bool> Filter = track => true;

        public SubtitleTrackListView()
        {
            InitializeComponent();
            _helper = new TrackListViewHelper(listViewAudioTracks, track => track.IsSubtitle && Filter(track), GetListItem);
            Load += _helper.OnLoad;
            _helper.PlaylistReconfigured += HelperOnPlaylistReconfigured;
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
                    new ListViewCell { Text = track.Codec.CommonName },
                    new ListViewCell { Text = track.Language.Name, Tag = track.Language },
                    new ListViewCell { Text = track.Type.ToString(), Tag = track.Type },
                    new ListViewCell { Text = (track.IndexOfType + 1).ToString("D"), Tag = track.IndexOfType }
                };
        }
    }
}
