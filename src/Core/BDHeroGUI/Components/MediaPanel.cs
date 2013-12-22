using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using BDHero.JobQueue;
using BDHeroGUI.Properties;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Controls;
using DotNetUtils.Extensions;
using DotNetUtils.TaskUtils;

namespace BDHeroGUI.Components
{
    public partial class MediaPanel : UserControl
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const double DefaultRatio = 2.0 / 3.0;

        private readonly Hyperlink _hyperlink;

        #region Public getter/setter properties

        [CanBeNull]
        public CoverArt SelectedCoverArt
        {
            get
            {
                return _selectedCoverArt;
            }
            private set
            {
                _selectedCoverArt = value;
                pictureBox.Image = SelectedCoverArt != null ? SelectedCoverArt.Image : Resources.no_poster_w185;
                AutoResize();
            }
        }

        private CoverArt _selectedCoverArt;

        public Job Job
        {
            set
            {
                _job = value;
                LoadSearchResults();
            }
        }

        private Job _job;

        #endregion

        #region Private properties

        private ReleaseMedium SelectedReleaseMedium
        {
            get { return _job != null ? _job.SelectedReleaseMedium : null; }
        }

        private string SelectedUrl
        {
            get
            {
                var medium = SelectedReleaseMedium;
                return medium != null ? medium.Url : null;
            }
        }

        #endregion

        #region Public events

        public event EventHandler SelectedMediaChanged;

        public Action Search;

        #endregion

        public MediaPanel()
        {
            InitializeComponent();
            _hyperlink = new Hyperlink(pictureBox);
            Resize += (sender, args) => AutoResize();
            comboBoxMedia.SelectedIndexChanged += (sender, args) => OnSelectedMediaChanged();
            LoadSearchResults();
        }

        #region Private controller methods

        private void AutoResize()
        {
            var ratio = DefaultRatio;
            if (SelectedCoverArt != null && SelectedCoverArt.Image != null)
            {
                var image = SelectedCoverArt.Image;
                ratio = ((double) image.Width) / image.Height;
            }
            var width = ratio * pictureBox.Height;
            splitContainer.SplitterDistance = (int) Math.Round(width);
        }

        private void LoadSearchResults()
        {
            var media = _job != null
                            ? _job.Movies.OfType<ReleaseMedium>().ToList()
                            : new Collection<ReleaseMedium>().ToList();

            // TODO: Insert "No metadata" item
//            media.Insert(0, new Movie()
//                            {
//                                Title = "No metadata"
//                            });

            PopulateComboBox(media);
            OnSelectedMediaChanged();
        }

        private void PopulateComboBox(ICollection<ReleaseMedium> releaseMedia)
        {
            comboBoxMedia.Items.Clear();

            releaseMedia.ForEach(item => comboBoxMedia.Items.Add(item));

            var hasItems = releaseMedia.Any();

            if (hasItems)
                comboBoxMedia.SelectedIndex = 0;

            comboBoxMedia.Enabled = hasItems;
        }

        private void OnSelectedMediaChanged()
        {
            if (comboBoxMedia.Items.Count > 0)
                _job.Movies.ForEach(AutoSelect);

            LoadCoverArt();

            _hyperlink.Url = SelectedUrl;

            if (SelectedMediaChanged != null)
                SelectedMediaChanged(this, EventArgs.Empty);
        }

        private void AutoSelect(Movie movie, int i)
        {
            movie.IsSelected = (i == comboBoxMedia.SelectedIndex);
        }

        private void LoadCoverArt()
        {
            SelectedCoverArt = null;

            var medium = SelectedReleaseMedium;
            if (medium == null) return;

            var coverArt = medium.CoverArtImages.FirstOrDefault();
            if (coverArt == null) return;

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork(delegate
                {
                    var image = coverArt.Image;
                    Logger.DebugFormat("Finished loading poster image: {0}", image);
                })
                .Fail(delegate(ExceptionEventArgs args)
                {
                    Logger.Error("Unable to fetch poster image", args.Exception);
                    SelectedCoverArt = null;
                })
                .Succeed(delegate
                {
                    SelectedCoverArt = coverArt;
                })
                .Build()
                .Start()
                ;
        }

        #endregion

        #region UI event handlers

        private void linkLabelSearch_Click(object sender, EventArgs e)
        {
            if (Search != null)
                Search();
        }

        #endregion
    }
}
