using System;
using System.Drawing;
using System.Windows.Forms;
using BDHero.ErrorReporting;
using DotNetUtils.Extensions;
using UILib.Extensions;
using UILib.WinForms.Controls;

namespace BDHeroGUI.Helpers
{
    internal class ToolStripControlBuilder
    {
        private static readonly Padding ZeroMargin = new Padding(0);

        private readonly FlowLayoutPanel _panel = new FlowLayoutPanel
                                                  {
                                                      Padding = new Padding(3, 2, 8, 1),
                                                      Margin = ZeroMargin
                                                  };

        public ToolStripControlBuilder()
        {
            _panel.MouseUp += OnMouseUp;
        }

        #region Public API

        public ToolStripControlBuilder AddImage(Image image)
        {
            AddControl(new PictureBox
                       {
                           Image = image,
                           Size = image.Size,
                           Margin = ZeroMargin
                       });
            return this;
        }

        public ToolStripControlBuilder AddLabel(string text)
        {
            AddControl(new Label
                       {
                           Text = text,
                           AutoSize = true,
                           Margin = ZeroMargin
                       });
            return this;
        }

        public ToolStripControlBuilder AddHyperlink(ErrorReportResultSuccess result)
        {
            AddHyperlinkImpl(result);
            return this;
        }

        public ToolStripControlBuilder AddLink(string text, EventHandler clickHandler)
        {
            var label = new LinkLabel2
                             {
                                 Text = text,
                                 Margin = ZeroMargin,
                                 Padding = ZeroMargin
                             };
            label.Click += clickHandler;
            AddControl(label);
            return this;
        }

        public ToolStripControlBuilder AddLink(Image image, string text, EventHandler clickHandler)
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
            AddControl(label);
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

            AddControl(new HyperlinkLabel
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
            var closePictureBox = new SelectablePictureBox
                                  {
                                      Image = closeImage,
                                      Size = closeImage.Size,
                                      Margin = ZeroMargin,
                                      Cursor = Cursors.Hand
                                  };
            closePictureBox.Click += (sender, args) => Dismiss();
            new ToolTip().SetToolTip(closePictureBox, "Dismiss");
            AddControl(closePictureBox);
        }

        private void AddControl(Control control)
        {
            control.MouseUp += OnMouseUp;
            control.Descendants().ForEach(descendant => descendant.MouseUp += OnMouseUp);
            _panel.Controls.Add(control);
        }

        private void OnMouseUp(object sender, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Middle)
                Dismiss();
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
