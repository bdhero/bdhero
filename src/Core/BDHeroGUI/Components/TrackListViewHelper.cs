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
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Extensions;
using I18N;
using UILib.WinForms.Controls;

namespace BDHeroGUI.Components
{
    class TrackListViewHelper
    {
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;

                _ignoreReconfiguration = true;

                _listView.Items.Clear();

                if (_playlist == null) return;

                var items = Transform(_playlist.Tracks);
                _listView.Items.AddRange(items);
                _listView.AutoSizeColumns();

                _ignoreReconfiguration = false;
            }
        }

        private Playlist _playlist;

        private bool _ignoreReconfiguration;

        public event PlaylistReconfiguredEventHandler PlaylistReconfigured;

        private readonly ListView2 _listView;
        private readonly Func<Track, bool> _filter;
        private readonly Func<Track, ICollection<ListViewCell>> _transform;

        private Language[] _allLanguages = new Language[0];

        public TrackListViewHelper(ListView2 listView, Func<Track, bool> filter, Func<Track, ICollection<ListViewCell>> transform)
        {
            _listView = listView;
            _filter = filter;
            _transform = transform;

            _listView.MultiSelect = true;
            _listView.ItemCheck += ListViewOnItemCheck;
            _listView.ItemChecked += ListViewOnItemChecked;

            InitContextMenu();
        }

        public void OnLoad(object sender = null, EventArgs eventArgs = null)
        {
            _listView.ToggleColumnSort(_listView.FirstDisplayedColumn.Index);
            _listView.AutoSizeColumns();
        }

        public void SetPlaylist(Playlist playlist, Language[] allLanguages)
        {
            Playlist = playlist;
            _allLanguages = allLanguages;
        }

        private void InitContextMenu()
        {
            _listView.MouseClick += ListViewOnMouseClick;
            _listView.KeyUp += ListViewOnKeyUp;
        }

        private void ListViewOnKeyUp(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Apps)
                return;

            if (_listView.SelectedIndices.Count == 0)
                return;

            var firstSelectedItem = _listView.SelectedItems[0];

            ShowContextMenu(firstSelectedItem.Position);
        }

        private void ListViewOnMouseClick(object sender, MouseEventArgs args)
        {
            if (args.Button != MouseButtons.Right)
                return;

            var pos = args.Location;
            var listViewItem = _listView.GetItemAt(pos.X, pos.Y);

            if (listViewItem == null)
                return;

            ShowContextMenu(pos);
        }

        private void ShowContextMenu(Point pos)
        {
            var selectedListViewItems = _listView.SelectedItems.OfType<ListViewItem>().ToArray();

            var menu = new ContextMenuStrip();

            AddLanguagesMenuItem(menu, selectedListViewItems);
            AddTrackTypesMenuItems(menu, selectedListViewItems);

            menu.Show(_listView, pos);
        }

        private void AddLanguagesMenuItem(ContextMenuStrip menu, ListViewItem[] listViewItems)
        {
            // Only allow users to change the language on video tracks
            var tracks = listViewItems.Select(item => item.Tag as Track).ToArray();
            var isVideo = tracks.All(track => track.IsVideo);
            if (!isVideo)
                return;

            var selectedLanguages = new HashSet<Language>();
            foreach (var track2 in tracks)
            {
                selectedLanguages.Add(track2.Language);
            }
            var selectedLanguage = selectedLanguages.Count == 1 ? selectedLanguages.FirstOrDefault() : null;

            var languagesMenuItem = new ToolStripMenuItem("Language");
            foreach (var language in _allLanguages.OrderBy(language => language.Name))
            {
                var lang = language;
                var langMenuItem = new ToolStripMenuItem(language.Name);

                langMenuItem.Click += (s, e) => LanguageMenuItemOnClick(listViewItems, lang);

                if (language.Equals(selectedLanguage))
                {
                    langMenuItem.Checked = true;
                    langMenuItem.Enabled = false;
                }

                languagesMenuItem.DropDownItems.Add(langMenuItem);
            }

            menu.Items.Add(languagesMenuItem);
            menu.Items.Add("-");
        }

        private void LanguageMenuItemOnClick(ListViewItem[] listViewItems, Language language)
        {
            foreach (var listViewItem in listViewItems)
            {
                LanguageMenuItemOnClick(language, listViewItem);
            }
            NotifyPlaylistReconfigured();
        }

        private static void LanguageMenuItemOnClick(Language language, ListViewItem listViewItem)
        {
            var track = listViewItem.Tag as Track;
            if (track != null) track.Language = language;
            var listViewSubItems = listViewItem.SubItems.OfType<ListViewItem.ListViewSubItem>().ToArray();
            var languageSubItems = listViewSubItems.Where(subItem => subItem.Tag is Language).ToArray();
            foreach (var subItem in languageSubItems)
            {
                subItem.Tag = language;
                subItem.Text = language.Name;
            }
        }

        private void AddTrackTypesMenuItems(ContextMenuStrip menu, ListViewItem[] selectedListViewItems)
        {
            var tracks = selectedListViewItems.Select(item => item.Tag as Track).ToArray();
            var allTrackTypes = Enum.GetValues(typeof(TrackType)).OfType<TrackType>().ToArray();

            var selectedTrackTypes = new HashSet<TrackType>();
            foreach (var track in tracks)
            {
                selectedTrackTypes.Add(track.Type);
            }
            var selectedTrackType = selectedTrackTypes.Count == 1 ? new TrackType?(selectedTrackTypes.First()) : null;

            foreach (var trackType in allTrackTypes)
            {
                var type = trackType;
                var menuItem = new ToolStripMenuItem(trackType.ToString());
                if (selectedTrackType.HasValue && selectedTrackType == trackType)
                {
                    menuItem.Checked = true;
                    menuItem.Enabled = false;
                }
                menuItem.Click += (s, e) => TrackTypeMenuItemOnClick(selectedListViewItems, type);
                menu.Items.Add(menuItem);
            }
        }

        private void TrackTypeMenuItemOnClick(ListViewItem[] listViewItems, TrackType trackType)
        {
            foreach (var listViewItem in listViewItems)
            {
                TrackTypeMenuItemOnClick(listViewItem, trackType);
            }
        }

        private void TrackTypeMenuItemOnClick(ListViewItem listViewItem, TrackType trackType)
        {
            var track = listViewItem.Tag as Track;
            if (track != null) track.Type = trackType;
            foreach (var subItem in listViewItem.SubItems.OfType<ListViewItem.ListViewSubItem>().Where(subItem => subItem.Tag is TrackType))
            {
                subItem.Tag = trackType;
                subItem.Text = trackType.ToString();
            }
            NotifyPlaylistReconfigured();
        }

        private void ListViewOnItemCheck(object sender, ItemCheckEventArgs e)
        {
            var track = _listView.Items[e.Index].Tag as Track;
            if (track == null) return;
            if (ShouldDisable(track))
            {
                e.NewValue = CheckState.Unchecked;
            }
        }

        private void ListViewOnItemChecked(object sender, ItemCheckedEventArgs args)
        {
            var track = args.Item.Tag as Track;
            if (track != null)
            {
                track.Keep = args.Item.Checked;
                NotifyPlaylistReconfigured();
            }
        }

        private void NotifyPlaylistReconfigured()
        {
            if (_ignoreReconfiguration)
                return;
            if (PlaylistReconfigured != null)
                PlaylistReconfigured(Playlist);
        }

        private static bool IsBestChoice(Track track)
        {
            return track.IsBestGuess;
        }

        private static bool ShouldDisable(Track track)
        {
            return !track.Codec.IsKnown || !track.Codec.IsMuxable;
        }

        private static bool ShouldMarkHidden(Track track)
        {
            return track.IsHidden;
        }

        private static void MarkBestChoice(ListViewItem item)
        {
            item.MarkBestChoice();
            item.AppendToolTip("Best choice based on your preferences");
        }

        private static void VisuallyDisable(ListViewItem item)
        {
            item.VisuallyDisable();
            item.AppendToolTip("Unsupported codec: cannot be muxed");
        }

        private static void MarkHidden(ListViewItem item)
        {
            item.MarkHidden();
            item.AppendToolTip("Hidden track");
        }

        private ListViewItem[] Transform(IEnumerable<Track> tracks)
        {
            return tracks.Where(_filter).Select(delegate(Track track)
                {
                    var cells = _transform(track);
                    var firstCell = cells.First();
                    var subCells = cells.Skip(1);

                    var item = new ListViewItem(firstCell.Text)
                        {
                            Checked = track.Keep,
                            Tag = track,
                            UseItemStyleForSubItems = true
                        };

                    item.SubItems.AddRange(subCells.Select(cell => CreateListViewSubItem(item, cell)).ToArray());

                    if (IsBestChoice(track))
                        MarkBestChoice(item);

                    if (ShouldDisable(track))
                        VisuallyDisable(item);

                    if (ShouldMarkHidden(track))
                        MarkHidden(item);

                    return item;
                }).ToArray();
        }

        private static ListViewItem.ListViewSubItem CreateListViewSubItem(ListViewItem item, ListViewCell cell)
        {
            return new ListViewItem.ListViewSubItem(item, cell.Text) {Tag = cell.Tag};
        }
    }

    public delegate void PlaylistReconfiguredEventHandler(Playlist playlist);
}
