using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.ErrorReporting;
using DotNetUtils.Controls;

namespace BDHeroGUI.Helpers
{
    internal class ToolStripIssueBuilder
    {
        private static readonly Padding ZeroMargin = new Padding(0);

        private readonly FlowLayoutPanel _panel = new FlowLayoutPanel
                                                  {
                                                      Padding = new Padding(3, 2, 8, 1),
                                                      Margin = ZeroMargin
                                                  };

        #region Public API

        public ToolStripIssueBuilder AddImage(Image image)
        {
            _panel.Controls.Add(new PictureBox
                                {
                                    Image = image,
                                    Size = image.Size,
                                    Margin = ZeroMargin
                                });
            return this;
        }

        public ToolStripIssueBuilder AddLabel(string text)
        {
            _panel.Controls.Add(new Label
                                {
                                    Text = text,
                                    AutoSize = true,
                                    Margin = ZeroMargin
                                });
            return this;
        }

        public ToolStripIssueBuilder AddHyperlink(ErrorReportResultSuccess result)
        {
            AddHyperlinkImpl(result);
            return this;
        }

        public ToolStripIssueBuilder AddLink(string text, EventHandler clickHandler)
        {
            var label = new LinkLabel2
                             {
                                 Text = text,
                                 Margin = ZeroMargin,
                                 Padding = ZeroMargin
                             };
            label.Click += clickHandler;
            _panel.Controls.Add(label);
            return this;
        }

        public ToolStripIssueBuilder AddLink(Image image, string text, EventHandler clickHandler)
        {
            var label = new LinkLabel2
                             {
                                 Text = text,
                                 Image = image,
                                 ImageRightPad = 0,
                                 Margin = ZeroMargin,
                                 Padding = ZeroMargin
                             };
            label.Click += clickHandler;
            _panel.Controls.Add(label);
            return this;
        }

        public ToolStripControlHost Build()
        {
            AddDismissButton();
            return new ToolStripControlHost(_panel);
        }

        #endregion

        #region Private implementation

        private void AddHyperlinkImpl(ErrorReportResultSuccess result)
        {
            var image = Properties.Resources.github_mark_16;
            var text = string.Format("Issue #{0}", result.IssueNumber);
            var url = result.Url;

            _panel.Controls.Add(new HyperlinkLabel
                                {
                                    Image = image,
                                    ImageRightPad = 0,
                                    Text = text,
                                    Url = url,
                                    Margin = ZeroMargin
                                });
        }

        private void AddDismissButton()
        {
            var closeImage = Properties.Resources.cross_small_gray;
            var closePictureBox = new PictureBox
                                  {
                                      Image = closeImage,
                                      Size = closeImage.Size,
                                      Margin = ZeroMargin,
                                      Cursor = Cursors.Hand
                                  };
            closePictureBox.Click += (sender, args) => Dismiss();
            new ToolTip().SetToolTip(closePictureBox, "Dismiss");
            _panel.Controls.Add(closePictureBox);
        }

        private void Dismiss()
        {
            _panel.Hide();
            if (_panel.Parent != null)
            {
                _panel.Parent.Controls.Remove(_panel);
            }
        }

        #endregion
    }
}
