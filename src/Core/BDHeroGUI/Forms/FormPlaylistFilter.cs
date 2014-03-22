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
using System.Linq;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Extensions;

namespace BDHeroGUI.Forms
{
    public partial class FormPlaylistFilter : Form
    {
        private readonly PlaylistFilter _filter;

        public FormPlaylistFilter(PlaylistFilter playlistFilter)
        {
            _filter = playlistFilter;
            InitializeComponent();
            Load += OnLoad;
            Shown += OnShown;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            var values = Enum.GetValues(typeof (TrackType)).OfType<object>().ToArray();
            checkedListBoxTypes.Items.AddRange(values);
            this.EnableSelectAll();
        }

        private void OnShown(object sender, EventArgs eventArgs)
        {
            textBoxMinDuration.Text = _filter.MinDuration.ToStringShort();
            textBoxMinChapterCount.Text = _filter.MinChapterCount.ToString();

            var i = 0;
            var trackTypes = checkedListBoxTypes.Items.OfType<TrackType>().ToArray();
            foreach (var trackType in trackTypes)
            {
                checkedListBoxTypes.SetItemChecked(i, _filter.TrackTypes.Contains(trackType));
                i++;
            }

            checkBoxHideDuplicatePlaylists.Checked = _filter.HideDuplicatePlaylists;
            checkBoxHideDuplicateStreamClips.Checked = _filter.HideDuplicateStreamClips;
            checkBoxHideLoops.Checked = _filter.HideLoops;
            checkBoxHideHiddenFirstTracks.Checked = _filter.HideHiddenFirstTracks;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _filter.MinDuration = TimeSpan.Parse(textBoxMinDuration.Text);
            _filter.MinChapterCount = Int32.Parse(textBoxMinChapterCount.Text);

            _filter.TrackTypes = checkedListBoxTypes.CheckedItems.OfType<TrackType>().ToList();

            _filter.HideDuplicatePlaylists = checkBoxHideDuplicatePlaylists.Checked;
            _filter.HideDuplicateStreamClips = checkBoxHideDuplicateStreamClips.Checked;
            _filter.HideLoops = checkBoxHideLoops.Checked;
            _filter.HideHiddenFirstTracks = checkBoxHideHiddenFirstTracks.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
