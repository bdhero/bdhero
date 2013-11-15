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
    public partial class VideoTrackListView : UserControl
    {
        public event PlaylistReconfiguredEventHandler PlaylistReconfigured;

        private readonly TrackListViewHelper _helper;

        public Func<Track, bool> Filter = track => true;

        public VideoTrackListView()
        {
            InitializeComponent();
            _helper = new TrackListViewHelper(listViewVideoTracks, track => track.IsVideo && Filter(track), GetListItem);
            Load += _helper.OnLoad;
            _helper.PlaylistReconfigured += HelperOnPlaylistReconfigured;

            // Set initial sort order for columns that should default to descending instead of ascending
            columnHeaderResolution.Tag = SortOrder.Descending;
            columnHeaderFrameRate.Tag = SortOrder.Descending;
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
                    new ListViewCell { Text = track.VideoFormatDisplayable },
                    new ListViewCell { Text = track.FrameRateDisplayable },
                    new ListViewCell { Text = track.AspectRatioDisplayable },
                    new ListViewCell { Text = track.Type.ToString(), Tag = track.Type },
                    new ListViewCell { Text = (track.IndexOfType + 1).ToString("D"), Tag = track.IndexOfType }
                };
        }
    }
}
