using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Extensions;

namespace BDHeroGUI.Components
{
    public abstract partial class TrackListView : UserControl
    {
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;

                listView.Items.Clear();

                if (_playlist == null) return;

                var items = Transform(_playlist.VideoTracks);
                listView.Items.AddRange(items);
                AutoSizeColumns();
            }
        }

        private Playlist _playlist;

        private static ListViewItem[] Transform(IList<Track> videoTracks)
        {
            return videoTracks.Select(delegate(Track track, int i)
                {
                    var item = new ListViewItem(track.Codec.CommonName)
                        {
                            Checked = track.Keep,
                            Tag = track
                        };
                    item.SubItems.AddRange(new[]
                        {
                            new ListViewItem.ListViewSubItem(item, track.VideoFormatDisplayable),
                            new ListViewItem.ListViewSubItem(item, track.FrameRateDisplayable),
                            new ListViewItem.ListViewSubItem(item, track.AspectRatioDisplayable),
                            new ListViewItem.ListViewSubItem(item, track.IndexOfType.ToString("D")) { Tag = track.IndexOfType }
                        });
                    return item;
                }).ToArray();
        }

        protected TrackListView()
        {
            InitializeComponent();
            Load += OnLoad;
            listView.ItemChecked += ListViewOnItemChecked;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            SortByFirstDisplayedColumn();
        }

        private void SortByFirstDisplayedColumn()
        {
            listView.SetSortColumn(listView.FirstDisplayedColumn.Index);
        }

        private static void ListViewOnItemChecked(object sender, ItemCheckedEventArgs args)
        {
            var track = args.Item.Tag as Track;
            if (track != null)
            {
                track.Keep = args.Item.Checked;
            }
        }

        public void AutoSizeColumns()
        {
            listView.AutoSizeColumns();
        }

        protected abstract ICollection<ListViewCell> GetListItem(Track track);
    }
}
