using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Extensions;
using I18N;

namespace BDHeroGUI.Forms
{
    public partial class FormTrackFilter : Form
    {
        private readonly TrackFilter _filter;
        private readonly Language[] _languages; 

        public FormTrackFilter(TrackFilter filter)
        {
            _filter = filter;
            _languages = Language.AllLanguages.OrderBy(language => language.ISO_639_2).ToArray();

            InitializeComponent();

            Load += OnLoad;
            Shown += OnShown;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            var values = Enum.GetValues(typeof(TrackType)).OfType<object>().ToArray();
            checkedListBoxTypes.Items.AddRange(values);
            this.EnableSelectAll();
        }

        private void OnShown(object sender, EventArgs eventArgs)
        {
            comboBoxPreferredLanguage.Items.AddRange(_languages.Select(language => language.UIDisplayName as object).ToArray());
            var curLangIndex = Array.IndexOf(_languages, _filter.PreferredLanguage);
            if (curLangIndex > -1)
            {
                comboBoxPreferredLanguage.SelectedIndex = curLangIndex;
            }

            var i = 0;
            var trackTypes = checkedListBoxTypes.Items.OfType<TrackType>().ToArray();
            foreach (var trackType in trackTypes)
            {
                checkedListBoxTypes.SetItemChecked(i, _filter.TrackTypes.Contains(trackType));
                i++;
            }

            checkBoxHideHidden.Checked = _filter.HideHiddenTracks;
            checkBoxHideUnsupportedCodecs.Checked = _filter.HideUnsupportedCodecs;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _filter.PreferredLanguage = _languages[comboBoxPreferredLanguage.SelectedIndex];
            _filter.TrackTypes = checkedListBoxTypes.CheckedItems.OfType<TrackType>().ToList();

            _filter.HideHiddenTracks = checkBoxHideHidden.Checked;
            _filter.HideUnsupportedCodecs = checkBoxHideUnsupportedCodecs.Checked;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
