using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using DotNetUtils.Extensions;
using DotNetUtils.Forms;
using NativeAPI.Win.User;
using NativeAPI.Win.UXTheme;

namespace TextEditor.WinForms
{
    [DefaultProperty("Text")]
    [DefaultEvent("TextChanged")]
    public class TextEditorControl : Panel
    {
        public TextEditorControl()
        {
            Editor = TextEditorFactory.CreateMultiLineTextEditor();
            Editor.TextChanged += (sender, args) => OnTextChanged(args);
            Editor.FontSizeChanged += (sender, args) => OnFontChanged(args);
            Editor.MultilineChanged += (sender, args) => OnMultilineChanged(args);

            Editor.Control.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);
            Controls.Add(Editor.Control);

            Padding = new Padding(1);

            SetStyle(ControlStyles.Selectable, false);

            EnableContextMenu = true;

            InitBorderEvents();
        }

        protected override Size DefaultSize
        {
            get { return new Size(500, 300); }
        }

        private ContextMenu CreateContextMenu()
        {
            var undo = new MenuItem("&Undo", (sender, args) => Editor.Undo());
            var redo = new MenuItem("&Redo", (sender, args) => Editor.Redo());

            var cut = new MenuItem("Cu&t", (sender, args) => Editor.Cut());
            var copy = new MenuItem("&Copy", (sender, args) => Editor.Copy());
            var paste = new MenuItem("&Paste", (sender, args) => Editor.Paste());
            var delete = new MenuItem("&Delete", (sender, args) => Editor.Delete());

            var selectAll = new MenuItem("Select &All", (sender, args) => Editor.SelectAll());

            var menu = new ContextMenu(new[]
                                       {
                                           undo,
                                           redo,
                                           new MenuItem("-"),
                                           cut,
                                           copy,
                                           paste,
                                           delete,
                                           new MenuItem("-"),
                                           selectAll,
                                       });

            menu.Popup += delegate
                          {
                              undo.Enabled = Editor.CanUndo;
                              redo.Enabled = Editor.CanRedo;
                              cut.Enabled = Editor.CanCut;
                              copy.Enabled = Editor.CanCopy;
                              paste.Enabled = Editor.CanPaste;
                              delete.Enabled = Editor.CanDelete;
                          };

            return menu;
        }

        /// <summary>
        ///     Gets the underlying editor for the control.
        /// </summary>
        [Browsable(false)]
        public ITextEditor Editor { get; private set; }

        /// <summary>
        ///     Gets or sets whether a standard context menu (cut, copy, paste, etc.) is available.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether the user can enter multiple lines of text.")]
        [DefaultValue(true)]
        public bool EnableContextMenu
        {
            get { return base.ContextMenu != null; }
            set
            {
                if (value == EnableContextMenu)
                    return;

                ContextMenu = value ? CreateContextMenu() : null;
            }
        }

        #region Text

        [Browsable(true)]
        [DefaultValue("")]
        public override string Text
        {
            get { return Editor.Text; }
            set { Editor.Text = value; }
        }

        #endregion

        #region Border styles

        private bool _isMouseOver;

        private void InitBorderEvents()
        {
            HandleCreated += OnHandleCreated;
        }

        private void OnHandleCreated(object sender, EventArgs eventArgs)
        {
            MouseEnter += ControlOnMouseEnter;
            MouseLeave += ControlOnMouseLeave;

            this.Descendants().ForEach(control => control.MouseEnter += ControlOnMouseEnter);
            this.Descendants().ForEach(control => control.MouseLeave += ControlOnMouseLeave);

            GotFocus += OnGotFocus;
            LostFocus += OnLostFocus;

            this.Descendants().ForEach(control => control.GotFocus += OnGotFocus);
            this.Descendants().ForEach(control => control.LostFocus += OnLostFocus);
        }

        private bool IsThisTextEditorControl(object sender)
        {
            if (sender == this)
                return true;

            var source = sender as Control;
            if (source == null)
                return false;

            var control = source;
            while (control != null)
            {
                if (control == this)
                    return true;

                control = control.Parent;
            }

            return false;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var state = !Enabled      ? TextBoxBorderStyle.Disabled :
                        ContainsFocus ? TextBoxBorderStyle.Focused :
                        _isMouseOver  ? TextBoxBorderStyle.Hot :
                        TextBoxBorderStyle.Normal;

            Theme.DrawThemedTextBoxBorder(Handle, e.Graphics, e.ClipRectangle, state);
        }

        #region Mouse enter/leave

        private void ControlOnMouseEnter(object sender, EventArgs eventArgs)
        {
            _isMouseOver = IsThisTextEditorControl(sender);
            Invalidate();
        }

        private void ControlOnMouseLeave(object sender, EventArgs eventArgs)
        {
            _isMouseOver = false;
            Invalidate();
        }

        #endregion

        #region Focus/blur

        private void OnGotFocus(object sender, EventArgs eventArgs)
        {
            Invalidate();
        }

        private void OnLostFocus(object sender, EventArgs eventArgs)
        {
            Invalidate();
        }

        #endregion

        #region Enable/disable

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnParentEnabledChanged(EventArgs e)
        {
            base.OnParentEnabledChanged(e);
            Invalidate();
        }

        #endregion

        #endregion

        #region Multiline

        /// <summary>
        ///     Gets or sets whether the user can enter multiple lines of text.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether the user can enter multiple lines of text.")]
        [DefaultValue(true)]
        public virtual bool Multiline
        {
            get { return Editor.Multiline; }
            set
            {
                if (value == Multiline)
                    return;

                Editor.Multiline = value;
            }
        }

        /// <summary>
        ///     Event raised when the value of the <see cref="Multiline"/> property is changed on this <see cref="TextEditorControl"/>.
        /// </summary>
        [Browsable(true)]
        [Description("Occurs when the value of the Multiline property changes.")]
        [RefreshProperties(RefreshProperties.All)]
        public event EventHandler MultilineChanged;

        protected virtual void OnMultilineChanged(EventArgs args)
        {
            if (!Multiline)
            {
                Editor.Options.ConvertTabsToSpaces = false;
                Editor.Options.CutCopyWholeLine = false;
                Editor.Options.ShowColumnRuler = false;
                Editor.Options.ShowLineNumbers = false;
                Editor.Options.ShowSpaces = false;
                Editor.Options.ShowTabs = false;
            }

            SetStyle(ControlStyles.FixedHeight, !Multiline);

            RecreateHandle();

            if (IsHandleCreated)
                AdjustSize(false);

            if (MultilineChanged != null)
                MultilineChanged(this, args);
        }

        #endregion

        #region Sizing

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            AdjustSize(true);
        }

        private void AdjustSize(bool returnIfAnchored)
        {
            AdjustHeight(returnIfAnchored);
        }

        /// <summary>
        ///     Adjusts the height of a single-line edit control to match the height of
        ///     the control's font.
        /// </summary>
        private void AdjustHeight(bool returnIfAnchored)
        {
            // If we're anchored to two opposite sides of the form, don't adjust the size because
            // we'll lose our anchored size by resetting to the requested width.
            if (returnIfAnchored &&
                (Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom))
            {
                return;
            }

            var padLR = Padding.Left + Padding.Right;
            var padTB = Padding.Top + Padding.Bottom;

            if (Multiline)
            {
                Editor.Control.Size = new Size(Size.Width - padLR,
                                               Size.Height - padTB);
                return;
            }

            var fontSize = FontSizeConverter.GetWinFormsFontSize(Editor.FontSize);
            var font = new Font(Font.FontFamily, (float)fontSize, Font.Style, GraphicsUnit.Point, Font.GdiCharSet, Font.GdiVerticalFont);

            Size measuredTextSize;

            using (var g = CreateGraphics())
            {
                var size = g.MeasureString("MQ", font);
                measuredTextSize = new Size((int) Math.Ceiling(size.Width), (int) Math.Ceiling(size.Height));
            }

            var width = Width;
            var height = (int) Math.Ceiling((double) measuredTextSize.Height);

            Size = new Size(width,
                            height + padTB);

            Editor.Control.Size = new Size(width + Editor.VerticalScrollBarWidth - padLR,
                                           height + Editor.HorizontalScrollBarHeight);
        }

        #endregion

        #region Font

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
//            AdjustHeight(false);
        }

        #endregion

        #region Options

        /// <summary>
        ///     Gets or sets whether the user can change the contents of the editor.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether the user can change the contents of the editor.")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return Editor.ReadOnly; }
            set { Editor.ReadOnly = value; }
        }

        /// <summary>
        ///     Gets or sets whether line numbers are displayed in the gutter.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether line numbers are displayed in the gutter.")]
        [DefaultValue(true)]
        public bool ShowLineNumbers
        {
            get { return Editor.Options.ShowLineNumbers; }
            set { Editor.Options.ShowLineNumbers = value; }
        }

        /// <summary>
        ///     Gets or sets whether tab and space characters are visualized.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether tab and space characters are visualized.")]
        [DefaultValue(true)]
        public bool ShowWhiteSpace
        {
            get { return Editor.Options.ShowTabs && Editor.Options.ShowSpaces; }
            set { Editor.Options.ShowTabs = Editor.Options.ShowSpaces = value; }
        }

        /// <summary>
        ///     Gets or sets whether a column ruler is displayed.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether a column ruler is displayed.")]
        [DefaultValue(true)]
        public bool ShowColumnRuler
        {
            get { return Editor.Options.ShowColumnRuler; }
            set { Editor.Options.ShowColumnRuler = value; }
        }

        /// <summary>
        ///     Gets or sets the position of the column ruler.
        /// </summary>
        [Browsable(true)]
        [Description("Controls the position of the column ruler.")]
        [DefaultValue(80)]
        public int ColumnRulerPosition
        {
            get { return Editor.Options.ColumnRulerPosition; }
            set { Editor.Options.ColumnRulerPosition = value; }
        }

        /// <summary>
        ///     Gets or sets whether the tab key inserts spaces instead of a tab character.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether the tab key inserts spaces instead of a tab character.")]
        [DefaultValue(true)]
        public bool ConvertTabsToSpaces
        {
            get { return Editor.Options.ConvertTabsToSpaces; }
            set { Editor.Options.ConvertTabsToSpaces = value; }
        }

        /// <summary>
        ///     Gets or sets whether cutting or copying with nothing selected cuts or copies the whole line.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether cutting or copying with nothing selected cuts or copies the whole line.")]
        [DefaultValue(true)]
        public bool CutCopyWholeLine
        {
            get { return Editor.Options.CutCopyWholeLine; }
            set { Editor.Options.CutCopyWholeLine = value; }
        }

        /// <summary>
        ///     Gets or sets the indentation (tab/space) width.
        /// </summary>
        [Browsable(true)]
        [Description("Controls the indentation (tab/space) width.")]
        [DefaultValue(4)]
        public int IndentationSize
        {
            get { return Editor.Options.IndentationSize; }
            set { Editor.Options.IndentationSize = value; }
        }

        #endregion
    }
}
