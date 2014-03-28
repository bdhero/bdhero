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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHeroGUI.Forms;
using BDHeroGUI.Properties;
using DotNetUtils.Extensions;
using DotNetUtils.FS;
using UILib.Extensions;

namespace BDHeroGUI.Components
{
    public partial class PlaylistListView : UserControl
    {
        /// <summary>
        /// Gets or sets the list of playlists to display, maintaining the user's current selection if possible.
        /// </summary>
        public IList<Playlist> Playlists
        {
            get { return _playlists; }
            set
            {
                var selected = SelectedPlaylist;

                _playlists = value;
                listView.Items.Clear();

                if (_playlists == null || !_playlists.Any()) return;

                var items = Transform(_playlists);
                listView.Items.AddRange(items);
                listView.AutoSizeColumns();

                SelectedPlaylist = selected;
            }
        }

        public IList<Playlist> VisiblePlaylistsInSortOrder
        {
            get { return listView.Items.OfType<ListViewItem>().Select(item => item.Tag as Playlist).ToList(); }
        }

        /// <summary>
        /// Gets or sets the currently selected playlist.  A value of <c>null</c> indicates that no playlist is selected.
        /// </summary>
        public Playlist SelectedPlaylist
        {
            get { return listView.SelectedItems.Count > 0 ? listView.SelectedItems[0].Tag as Playlist : null; }
            set
            {
                if (value == null)
                {
                    listView.SelectNone();
                    return;
                }

                listView.SelectWhere(item => item.Tag == value);

                if (listView.SelectedItems.Count == 0 && listView.Items.Count > 0)
                {
                    listView.Items[0].Selected = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether all playlists are shown, regardless of the user's filter settings.
        /// </summary>
        public bool ShowAll
        {
            get { return checkBoxShowAllPlaylists.Checked; }
            set { checkBoxShowAllPlaylists.Checked = value; }
        }

        /// <summary>
        /// Triggered whenever the user selects a new playlist or deselects the current playlist.
        /// </summary>
        public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged;

        public event EventHandler ShowAllChanged;

        public event PlaylistReconfiguredEventHandler PlaylistReconfigured;

        private IList<Playlist> _playlists;

        private readonly PlaylistFilter _filter = new PlaylistFilter();

        private bool _showAllPlaylists;

        public PlaylistListView()
        {
            InitializeComponent();
            Load += OnLoad;

            listView.ItemSelectionChanged += delegate(object sender, ListViewItemSelectionChangedEventArgs args)
                {
                    if (ItemSelectionChanged != null)
                        ItemSelectionChanged(sender, args);
                };

            listView.MouseClick += ListViewOnMouseClick;
            listView.MouseDoubleClick += ListViewOnMouseDoubleClick;
            listView.KeyUp += ListViewOnKeyUp;

            // Set initial sort order for columns that should default to descending instead of ascending
            columnHeaderLength.Tag = SortOrder.Descending;
            columnHeaderChapterCount.Tag = SortOrder.Descending;
            columnHeaderFileSize.Tag = SortOrder.Descending;
        }

        private void ListViewOnKeyUp(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Apps)
                return;

            if (SelectedPlaylist == null)
                return;

            var selectedItem = listView.SelectedItems[0];

            ShowContextMenu(selectedItem.Position, SelectedPlaylist);
        }

        private void ListViewOnMouseClick(object sender, MouseEventArgs args)
        {
            if (args.Button != MouseButtons.Right)
                return;

            var item = listView.GetItemAt(args.Location.X, args.Location.Y);
            if (item == null)
                return;

            var playlist = item.Tag as Playlist;
            if (playlist == null)
                return;

            ShowContextMenu(args.Location, playlist);
        }

        private void ShowContextMenu(Point point, Playlist playlist)
        {
            var menu = new ContextMenuStrip();

            var assoc = new FileAssociation(playlist.FullPath);

            var playItem = new ToolStripMenuItem(assoc.HasAssociation ? string.Format("&Play with {0}", assoc.AppName) : "&Play with...");
            playItem.Click += (o, eventArgs) => FileUtils.OpenFile(playlist.FullPath, this);
            playItem.Font = new Font(playItem.Font, FontStyle.Bold);
            playItem.Enabled = assoc.CanBeOpened;
            if (assoc.HasAssociation)
                playItem.Image = assoc.GetProgramImage(16);

            var showFileItem = new ToolStripMenuItem("Show in &folder", Resources.folder_open);
            showFileItem.Click += (o, eventArgs) => FileUtils.ShowInFolder(playlist.FullPath, this);

            var copyPathItem = new ToolStripMenuItem("&Copy path to clipboard", Resources.clipboard_arrow);
            copyPathItem.Click += (o, eventArgs) => Clipboard.SetText(playlist.FullPath);

            if (!File.Exists(playlist.FullPath))
            {
                playItem.Enabled = false;
                showFileItem.Enabled = false;
                menu.Items.Add(new ToolStripMenuItem("File not found") {Enabled = false});
            }

            menu.Items.Add(playItem);
            menu.Items.Add("-");
            menu.Items.Add(showFileItem);
            menu.Items.Add(copyPathItem);
            menu.Items.Add("-");

            AddCutMenuItems(menu, playlist);

            menu.Show(listView, point);
        }

        private void AddCutMenuItems(ContextMenuStrip menu, Playlist playlist)
        {
            var cuts = Enum.GetValues(typeof (PlaylistCut)).OfType<PlaylistCut>().ToArray();
            foreach (var cut in cuts)
            {
                AddCutMenuItem(menu, playlist, cut);
            }
        }

        private void AddCutMenuItem(ContextMenuStrip menu, Playlist playlist, PlaylistCut cut)
        {
            var item = new ToolStripMenuItem(cut.ToString());
            item.Tag = cut;
            if (playlist.Cut == cut)
            {
                item.Checked = true;
                item.Enabled = false;
            }
            item.Click += (sender, args) => SetPlaylistCut(playlist, cut);
            menu.Items.Add(item);
        }

        private void SetPlaylistCut(Playlist playlist, PlaylistCut cut)
        {
            playlist.Cut = cut;
            var item = listView.Items.OfType<ListViewItem>().FirstOrDefault(viewItem => viewItem.Tag == playlist);
            if (item == null)
                return;
            var subitems = item.SubItems.OfType<ListViewItem.ListViewSubItem>().Where(subitem => subitem.Tag is PlaylistCut).ToArray();
            foreach (var subitem in subitems)
            {
                subitem.Tag = cut;
                subitem.Text = cut.ToString();
            }
            NotifyPlaylistReconfigured();
        }

        private void ListViewOnMouseDoubleClick(object sender, MouseEventArgs args)
        {
            var item = listView.GetItemAt(args.Location.X, args.Location.Y);
            if (item == null)
                return;

            var playlist = item.Tag as Playlist;
            if (playlist == null)
                return;

            if (File.Exists(playlist.FullPath))
                FileUtils.OpenFile(playlist.FullPath, this);
            else
                MessageBox.Show(this, string.Format("The file \"{0}\" could not be found.", playlist.FullPath), "File not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            listView.ToggleColumnSort(columnHeaderType.Index);
        }

        private ListViewItem[] Transform(IEnumerable<Playlist> playlists)
        {
            return playlists.Where(ShowPlaylist).Select(Transform).ToArray();
        }

        private bool ShowPlaylist(Playlist playlist)
        {
            return _filter.Show(playlist) || _showAllPlaylists;
        }

        private static ListViewItem Transform(Playlist playlist)
        {
            var item = new ListViewItem(playlist.FileName) { Tag = playlist };

            item.SubItems.AddRange(new[]
                {
                    new ListViewItem.ListViewSubItem(item, playlist.Length.ToStringMedium()) { Tag = playlist.Length },
                    new ListViewItem.ListViewSubItem(item, playlist.ChapterCount.ToString()) { Tag = playlist.ChapterCount },
                    new ListViewItem.ListViewSubItem(item, playlist.FileSize.ToString("n0")) { Tag = playlist.FileSize },
                    new ListViewItem.ListViewSubItem(item, playlist.Type.ToString()) { Tag = playlist.Type },
                    new ListViewItem.ListViewSubItem(item, playlist.Cut.ToString()) { Tag = playlist.Cut },
                    new ListViewItem.ListViewSubItem(item, playlist.FirstVideoLanguage.ToString()) { Tag = playlist.FirstVideoLanguage },
                    new ListViewItem.ListViewSubItem(item, playlist.Warnings) { Tag = playlist.Warnings },
                });

            if (IsBestChoice(playlist))
                MarkBestChoice(item);

            return item;
        }

        private static bool IsBestChoice(Playlist playlist)
        {
            return playlist.IsBestGuess;
        }

        private static void MarkBestChoice(ListViewItem item)
        {
            item.MarkBestChoice();
            item.AppendToolTip("Best choice based on your preferences");
        }

        public void ShowFilterWindow()
        {
            DialogResult result;

            using (var form = new FormPlaylistFilter(_filter))
            {
                result = form.ShowDialog(this);
            }

            if (result == DialogResult.OK)
            {
                RefreshPlaylists();
            }
        }

        public void SelectBestPlaylist()
        {
            if (!VisiblePlaylistsInSortOrder.Any())
                return;

            SelectedPlaylist = VisiblePlaylistsInSortOrder.FirstOrDefault(IsBestChoice) ??
                               VisiblePlaylistsInSortOrder.FirstOrDefault();
        }

        private void linkLabelShowFilterWindow_Click(object sender, EventArgs e)
        {
            ShowFilterWindow();
        }

        private void checkBoxShowAllPlaylists_CheckedChanged(object sender, EventArgs e)
        {
            _showAllPlaylists = checkBoxShowAllPlaylists.Checked;

            RefreshPlaylists();

            if (ShowAllChanged != null)
                ShowAllChanged(this, EventArgs.Empty);
        }

        private void RefreshPlaylists()
        {
            Playlists = Playlists;
        }

        public void ReconfigurePlaylist(Playlist playlist)
        {
            if (!Playlists.Contains(playlist))
                return;

            var newItem = Transform(playlist);

            var items = listView.Items.OfType<ListViewItem>().Where(curItem => curItem.Tag == newItem.Tag).ToArray();
            var oldItem = items.FirstOrDefault();
            if (oldItem == null)
                return;

            newItem.Selected = oldItem.Selected;

            // Replacing the item by inserting the new one and removing the old one
            // causes a stack overflow because it triggers a SelectedIndexChanged event, which then
            // calls this method, etc.  So we must resort to copying the new values into the old subitems.

            oldItem.Text = newItem.Text;
            oldItem.Tag = newItem.Tag;
            oldItem.ToolTipText = newItem.ToolTipText;
            oldItem.Font = newItem.Font;
            oldItem.BackColor = newItem.BackColor;
            oldItem.ForeColor = newItem.ForeColor;

            var oldSubItems = oldItem.SubItems.OfType<ListViewItem.ListViewSubItem>().ToArray();
            var newSubItems = newItem.SubItems.OfType<ListViewItem.ListViewSubItem>().ToArray();

            for (var i = 0; i < oldSubItems.Length; i++)
            {
                oldSubItems[i].Text = newSubItems[i].Text;
                oldSubItems[i].Tag = newSubItems[i].Tag;
                oldSubItems[i].Font = newSubItems[i].Font;
                oldSubItems[i].BackColor = newSubItems[i].BackColor;
                oldSubItems[i].ForeColor = newSubItems[i].ForeColor;
            }
        }

        private void NotifyPlaylistReconfigured()
        {
            if (PlaylistReconfigured != null)
                PlaylistReconfigured(SelectedPlaylist);
        }
    }
}
