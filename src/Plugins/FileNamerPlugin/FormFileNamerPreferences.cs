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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHero.JobQueue;
using UILib.Extensions;
using UILib.WinForms.Controls;

namespace BDHero.Plugin.FileNamer
{
    internal partial class FormFileNamerPreferences : Form
    {
        private readonly Preferences _userPrefs;
        private readonly Preferences _prefsCopy;

        private readonly Job _movieJob;
        private readonly FileNamer _movieNamer;

        private readonly Job _tvShowJob;
        private readonly FileNamer _tvShowNamer;

        #region Properties

        private Codec SelectedCodec
        {
            get
            {
                var indices = listViewCodecNames.SelectedIndices;
                var index = indices.Count > 0 ? indices[0] : -1;
                if (index > -1)
                {
                    var codec = listViewCodecNames.Items[index].Tag as Codec;
                    return codec;
                }
                return null;
            }
        }

        #endregion

        #region Constructor and OnLoad

        public FormFileNamerPreferences(Preferences prefs)
        {
            _userPrefs = prefs;
            _prefsCopy = _userPrefs.Clone();

            _movieJob = MockJobFactory.CreateMovieJob();
            _movieNamer = new FileNamer(_movieJob, _prefsCopy);

            _tvShowJob = MockJobFactory.CreateTVShowJob();
            _tvShowNamer = new FileNamer(_tvShowJob, _prefsCopy);

            InitializeComponent();
            Load += OnLoad;
            this.EnableSelectAll();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            InitTextEditors();
            InitLabelBackgrounds();
            InitValues();
            InitReplaceSpaces();
            InitCodecListView();
            InitTextBoxEvents();
            InitComboBoxEvents();
            InitToolTips();
            Rename();
        }

        #endregion

        #region Initialization

        private void InitTextEditors()
        {
            // TODO: 
            textBoxMovieFileName.Editor.SetSyntaxFromExtension(".bdheromoviefilename");
            textBoxTVShowFileName.Editor.SetSyntaxFromExtension(".bdherotvshowfilename");
        }

        private void InitLabelBackgrounds()
        {
            foreach (var label in this.Descendants<SelectableLabel>())
            {
                label.BackColor = SystemColors.Window;
            }
        }

        /// <summary>
        /// Converts a number format string to its UI text representation.
        /// </summary>
        /// <param name="numberFormat"></param>
        /// <returns></returns>
        private static string NF2S(string numberFormat)
        {
            return numberFormat == "D2" ? "01" : "1";
        }

        /// <summary>
        /// Converts a UI text representation of a number format string to the number format string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string S2NF(string text)
        {
            return text == "01" ? "D2" : "D1";
        }

        private void InitValues()
        {
            _ignoreChange = true;

            checkBoxReplaceSpaces.Checked = _prefsCopy.ReplaceSpaces;
            textBoxReplaceSpacesWith.Enabled = _prefsCopy.ReplaceSpaces;
            textBoxReplaceSpacesWith.Text = _prefsCopy.ReplaceSpacesWith;

            textBoxMovieDirectory.Text = _prefsCopy.Movies.Directory;
            textBoxMovieFileName.Text = _prefsCopy.Movies.FileName;

            textBoxTVShowDirectory.Text = _prefsCopy.TVShows.Directory;
            textBoxTVShowFileName.Text = _prefsCopy.TVShows.FileName;
            comboBoxSeasonNumberFormat.SelectedItem = NF2S(_prefsCopy.TVShows.SeasonNumberFormat);
            comboBoxEpisodeNumberFormat.SelectedItem = NF2S(_prefsCopy.TVShows.EpisodeNumberFormat);
            textBoxTVShowReleaseDateFormat.Text = _prefsCopy.TVShows.ReleaseDateFormat;

            _ignoreChange = false;
        }

        private void InitReplaceSpaces()
        {
            checkBoxReplaceSpaces.CheckedChanged += CheckBoxReplaceSpacesOnCheckedChanged;
            textBoxReplaceSpacesWith.TextChanged += TextBoxReplaceSpacesWithOnTextChanged;

            CheckBoxReplaceSpacesOnCheckedChanged();
            TextBoxReplaceSpacesWithOnTextChanged();
        }

        private void InitCodecListView()
        {
            listViewCodecNames.AfterLabelEdit += ListViewCodecNamesOnAfterLabelEdit;
            PopulateCodecListView();
            listViewCodecNames.ToggleColumnSort(columnHeaderNumber.Index);
        }

        private void PopulateCodecListView()
        {
            listViewCodecNames.SuspendDrawing();
            listViewCodecNames.Items.Clear();

            var codecs = Codec.MuxableBDCodecs.Where(codec => _prefsCopy.Codecs.ContainsKey(codec.SerializableName)).ToArray();

            var i = 1;
            foreach (var codec in codecs)
            {
                var label = _prefsCopy.Codecs[codec.SerializableName];
                var group = codec.IsVideo
                                ? listViewCodecNames.Groups["listViewGroupVideo"]
                                : codec.IsAudio
                                      ? listViewCodecNames.Groups["listViewGroupAudio"]
                                      : listViewCodecNames.Groups["listViewGroupSubtitles"];
                var item = new ListViewItem(label, @group) {Tag = codec};
                var subitems = new[]
                    {
                        new ListViewItem.ListViewSubItem(item, codec.FullNameDisambig),
                        new ListViewItem.ListViewSubItem(item, i.ToString("D")) {Tag = i}
                    };
                item.SubItems.AddRange(subitems);
                listViewCodecNames.Items.Add(item);
                i++;
            }

            listViewCodecNames.ResumeDrawing();
            listViewCodecNames.AutoSizeColumns();
        }

        private void InitTextBoxEvents()
        {
            textBoxMovieDirectory.TextChanged += (s, e) => Rename();
            textBoxMovieFileName.TextChanged += (s, e) => Rename();

            textBoxTVShowDirectory.TextChanged += (s, e) => Rename();
            textBoxTVShowFileName.TextChanged += (s, e) => Rename();

            textBoxTVShowReleaseDateFormat.TextChanged += textBoxTVShowReleaseDateFormat_TextChanged;

            selectableLabelMoviePlaceholders.SelectVariablesOnClick();
            selectableLabelTVShowPlaceholders.SelectVariablesOnClick();
        }

        private void InitComboBoxEvents()
        {
            comboBoxSeasonNumberFormat.SelectedIndexChanged += (s, e) => Rename();
            comboBoxEpisodeNumberFormat.SelectedIndexChanged += (s, e) => Rename();
        }

        private void InitToolTips()
        {
            new ToolTip().SetToolTip(buttonRevert, "Discard unsaved changes");
            new ToolTip().SetToolTip(buttonDefault, "Use default values");
        }

        #endregion

        #region UI events

        private void Rename()
        {
            OnChanged(_movieNamer, _tvShowNamer);
        }

        private bool _ignoreChange;

        private void OnChanged(FileNamer movieNamer, FileNamer tvShowNamer)
        {
            if (_ignoreChange) return;

            _ignoreChange = true;

            // Movies

            _prefsCopy.Movies.Directory = textBoxMovieDirectory.Text;
            _prefsCopy.Movies.FileName = textBoxMovieFileName.Text;

            var moviePath = movieNamer.GetPath();

            textBoxMovieDirectoryExample.Text = moviePath.Directory;
            textBoxMovieFileNameExample.Text = moviePath.FileName;

            // TV Shows

            _prefsCopy.TVShows.Directory = textBoxTVShowDirectory.Text;
            _prefsCopy.TVShows.FileName = textBoxTVShowFileName.Text;
            _prefsCopy.TVShows.SeasonNumberFormat = S2NF(comboBoxSeasonNumberFormat.SelectedItem as string);
            _prefsCopy.TVShows.EpisodeNumberFormat = S2NF(comboBoxEpisodeNumberFormat.SelectedItem as string);
            _prefsCopy.TVShows.ReleaseDateFormat = textBoxTVShowReleaseDateFormat.Text;

            var tvShowPath = tvShowNamer.GetPath();

            textBoxTVShowDirectoryExample.Text = tvShowPath.Directory;
            textBoxTVShowFileNameExample.Text = tvShowPath.FileName;

            // Save/revert/default buttons

            var hasChanged = !_prefsCopy.Equals(_userPrefs);
            var isDefault = _prefsCopy.Equals(new Preferences());

            buttonRevert.Enabled = hasChanged;
            buttonDefault.Enabled = !isDefault;
            buttonSave.Enabled = hasChanged;

            _ignoreChange = false;
        }

        private void CheckBoxReplaceSpacesOnCheckedChanged(object sender = null, EventArgs eventArgs = null)
        {
            textBoxReplaceSpacesWith.Enabled = checkBoxReplaceSpaces.Checked;
            _prefsCopy.ReplaceSpaces = checkBoxReplaceSpaces.Checked;
            Rename();
        }

        private void TextBoxReplaceSpacesWithOnTextChanged(object sender = null, EventArgs eventArgs = null)
        {
            using (Graphics g = textBoxReplaceSpacesWith.CreateGraphics())
            {
                var before = textBoxReplaceSpacesWith.Width;
                var text = textBoxReplaceSpacesWith.Text + "MM"; // add 2 letters for padding
                var size = g.MeasureString(text, textBoxReplaceSpacesWith.Font);
                var after = (int)Math.Ceiling(size.Width);
                var delta = after - before;
                textBoxReplaceSpacesWith.Width += delta;
            }
            _prefsCopy.ReplaceSpacesWith = textBoxReplaceSpacesWith.Text;
            Rename();
        }

        private void ListViewCodecNamesOnAfterLabelEdit(object sender, LabelEditEventArgs args)
        {
            if (args.CancelEdit || args.Label == null)
                return;

            var index = args.Item;
            var label = args.Label;

            if (SelectedCodec != null)
            {
                _prefsCopy.Codecs[SelectedCodec.SerializableName] = label;
            }

            Rename();
        }

        private void textBoxTVShowReleaseDateFormat_TextChanged(object sender, EventArgs e)
        {
            var format = textBoxTVShowReleaseDateFormat.Text;
            try
            {
                var str = DateTime.Now.ToString(format);
                _prefsCopy.TVShows.ReleaseDateFormat = format;
                Rename();
            }
            catch
            {
                // TODO: Tell user the format is wrong
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _userPrefs.CopyFrom(_prefsCopy);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            Default();
        }

        private void buttonRevert_Click(object sender, EventArgs e)
        {
            Revert();
        }

        private void Revert()
        {
            _prefsCopy.CopyFrom(_userPrefs);
            InitValues();
            PopulateCodecListView();
            Rename();
        }

        private void Default()
        {
            _prefsCopy.CopyFrom(new Preferences());
            InitValues();
            PopulateCodecListView();
            Rename();
        }

        #endregion
    }
}
