using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DotNetUtils.Extensions;
using NativeAPI.Win.UXTheme;
using UILib.Extensions;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
namespace TextEditor.WinForms
{
    /// <summary>
    ///     Windows Forms control that wraps a <see cref="ITextEditor"/> object.
    /// </summary>
    [DefaultProperty("Text")]
    [DefaultEvent("TextChanged")]
    public class TextEditorControl : Control
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Gets the underlying editor for the control.
        /// </summary>
        [Browsable(false)]
        public ITextEditor Editor { get; private set; }

        private readonly ContextMenuStrip _standardContextMenuStrip;

        private bool _isMouseOver;
        private Padding _borderPadding;
        private BorderStyle _borderStyle;

        /// <summary>
        ///     Constructs a new <see cref="TextEditorControl"/> instance.
        /// </summary>
        public TextEditorControl()
        {
            Editor = TextEditorFactory.CreateMultiLineTextEditor();
            Editor.Multiline = true;
            Editor.Control.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom);

            _standardContextMenuStrip = new TextEditorContextMenuStrip(Editor);

            SetStyle(ControlStyles.Selectable, false);

            BindEvents();

            // Default values
            BorderStyle = BorderStyle.Fixed3D;
            StandardContextMenu = true;

            Controls.Add(Editor.Control);

            Editor.ReadOnlyChanged += EditorOnReadOnlyChanged;

            Editor.Control.PreviewKeyDown += ControlOnPreviewKeyDown;
        }

        private void ControlOnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.I)
            {
                // AvalonEdit captures and suppresses CTRL+I (for "Indent"),
                // even though the command doesn't appear to actually do anything.
                // Since the host application may actually want to process this,
                // we proxy it from Avalon to this control.
                OnPreviewKeyDown(e);
            }
        }

        private void EditorOnReadOnlyChanged(object sender, EventArgs eventArgs)
        {
            Editor.SetBackgroundColor(Editor.ReadOnly ? Color.Transparent : Color.White);
        }

        ~TextEditorControl()
        {
            UnbindEvents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnbindEvents();
            }
            base.Dispose(disposing);
        }

        #region Event binding

        private void BindEvents()
        {
            Editor.TextChanged += EditorOnTextChanged;
            Editor.FontSizeChanged += EditorOnFontSizeChanged;
            Editor.MultilineChanged += EditorOnMultilineChanged;

            HandleCreated += OnHandleCreated;

            // Prevent border artifacts
            Resize += OnResize;

            PaddingChanged += OnPaddingChanged;
            BorderStyleChanged += OnBorderStyleChanged;

            PaintBackground += OnPaintBackground;
        }

        private void UnbindEvents()
        {
            Editor.TextChanged -= EditorOnTextChanged;
            Editor.FontSizeChanged -= EditorOnFontSizeChanged;
            Editor.MultilineChanged -= EditorOnMultilineChanged;

            HandleCreated -= OnHandleCreated;

            UnbindMouseEvents();
            UnbindFocusEvents();

            Resize -= OnResize;

            PaddingChanged -= OnPaddingChanged;
            BorderStyleChanged -= OnBorderStyleChanged;

            PaintBackground -= OnPaintBackground;
        }

        #region Event handler proxy methods

        private void EditorOnTextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        private void EditorOnFontSizeChanged(object sender, EventArgs e)
        {
            OnFontChanged(e);
        }

        private void EditorOnMultilineChanged(object sender, EventArgs e)
        {
            OnMultilineChanged(e);
        }

        private void OnHandleCreated(object sender, EventArgs e)
        {
            BindMouseEvents();
            BindFocusEvents();
            AdjustRects(true);
        }

        private void OnPaddingChanged(object sender, EventArgs e)
        {
            OnPaddingChanged();
        }

        private void OnBorderStyleChanged(object sender, EventArgs e)
        {
            AdjustBorderPadding();
        }

        private void OnResize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void OnPaintBackground(object sender, PaintEventArgs e)
        {
            PaintBorder(e);
        }

        #endregion

        #region Mouse events

        private void BindMouseEvents()
        {
            BindMouseEvents(this);
            this.Descendants().ForEach(BindMouseEvents);
        }

        private void UnbindMouseEvents()
        {
            UnbindMouseEvents(this);
            this.Descendants().ForEach(UnbindMouseEvents);
        }

        private void BindMouseEvents(Control control)
        {
            control.MouseEnter += ControlOnMouseEnter;
            control.MouseLeave += ControlOnMouseLeave;
        }

        private void UnbindMouseEvents(Control control)
        {
            control.MouseEnter -= ControlOnMouseEnter;
            control.MouseLeave -= ControlOnMouseLeave;
        }

        #endregion

        #region Focus events

        private void BindFocusEvents()
        {
            BindFocusEvents(this);
            this.Descendants().ForEach(BindFocusEvents);
        }

        private void UnbindFocusEvents()
        {
            UnbindFocusEvents(this);
            this.Descendants().ForEach(UnbindFocusEvents);
        }

        private void BindFocusEvents(Control control)
        {
            control.GotFocus += OnGotFocus;
            control.LostFocus += OnLostFocus;
        }

        private void UnbindFocusEvents(Control control)
        {
            control.GotFocus -= OnGotFocus;
            control.LostFocus -= OnLostFocus;
        }

        #endregion

        #endregion

        #region Browsable properties and events

        #region Context menu

        /// <summary>
        ///     Gets or sets whether a standard context menu (cut, copy, paste, etc.) is available.
        /// </summary>
        [Browsable(true)]
        [Description("Determines whether the user can enter multiple lines of text.")]
        [DefaultValue(true)]
        public bool StandardContextMenu
        {
            get { return base.ContextMenuStrip == _standardContextMenuStrip; }
            set
            {
                if (value == StandardContextMenu)
                    return;

                ContextMenuStrip = value ? _standardContextMenuStrip : null;
            }
        }

        #endregion

        #region Text property

        [Browsable(true)]
        [DefaultValue("")]
        public override string Text
        {
            get { return Editor.Text; }
            set { Editor.Text = value; }
        }

        #endregion

        #region BorderStyle property + event

        /// <summary>
        ///     Gets or sets the border style for the control.
        /// </summary>
        [Browsable(true)]
        [Description("Indicates the border style for the control.")]
        [DefaultValue(BorderStyle.Fixed3D)]
        public BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (value == BorderStyle)
                    return;

                _borderStyle = value;

                OnBorderStyleChanged();
                Invalidate();
            }
        }

        /// <summary>
        ///     Triggered whenever the value of the <see cref="BorderStyle"/> property changes.
        /// </summary>
        [Browsable(true)]
        [Description("Triggered whenever the value of the BorderStyle property changes.")]
        public event EventHandler BorderStyleChanged;

        /// <summary>
        ///     Notifies <see cref="BorderStyleChanged"/> listeners whenever the value of the <see cref="BorderStyle"/> property changes.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnBorderStyleChanged(EventArgs args = null)
        {
            if (BorderStyleChanged != null)
                BorderStyleChanged(this, args ?? EventArgs.Empty);
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
            get { return Editor.Options.ShowWhiteSpace; }
            set { Editor.Options.ShowWhiteSpace = value; }
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

        #endregion

        #region Border styles

        private void PaintBorder(PaintEventArgs e)
        {
            // Occurs when switching from Aero to Classic theme in Windows Vista or newer
            if (e.ClipRectangle.IsEmpty)
            {
                Logger.Warn("PaintBorder() - PaintEventArgs.ClipRectangle is empty; ignoring paint request");
                return;
            }

            if (BorderStyle == BorderStyle.None)
                return;

            var state = !Enabled      ? TextBoxState.Disabled :
                        ContainsFocus ? TextBoxState.Focused :
                        _isMouseOver  ? TextBoxState.Hot :
                                        TextBoxState.Normal;

            ThemeAPI.DrawThemedTextBoxBorder(Handle, e.Graphics, e.ClipRectangle, state);
        }

        #region Padding

        /// <summary>
        ///     Gets the full amount of padding required to accommodate the requested <see cref="Padding"/> value
        ///     and the editor's border, if applicable.
        /// </summary>
        protected virtual Padding CalculatedPadding
        {
            get
            {
                return new Padding(Padding.Left   + _borderPadding.Left,
                                   Padding.Top    + _borderPadding.Top,
                                   Padding.Right  + _borderPadding.Right,
                                   Padding.Bottom + _borderPadding.Bottom);
            }
        }

        private void AdjustBorderPadding()
        {
            _borderPadding = (BorderStyle == BorderStyle.None) ? new Padding(0) : new Padding(1);
            AdjustChildRect();
        }

        private void OnPaddingChanged()
        {
            AdjustChildRect();
            Invalidate();
        }

        #endregion

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

        /// <summary>
        ///     Notifies <see cref="MultilineChanged"/> listeners whenever the value of the <see cref="Multiline"/> property changes.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnMultilineChanged(EventArgs args)
        {
            if (!Multiline)
            {
                Editor.Options.ConvertTabsToSpaces = false;
                Editor.Options.CutCopyWholeLine = false;
                Editor.Options.ShowColumnRuler = false;
                Editor.Options.ShowLineNumbers = false;
                Editor.Options.ShowWhiteSpace = false;
            }

            SetStyle(ControlStyles.FixedHeight, !Multiline);

            RecreateHandle();

            if (IsHandleCreated)
                AdjustRects(false);

            if (MultilineChanged != null)
                MultilineChanged(this, args);
        }

        #endregion

        #region Sizing

        protected override Size DefaultSize
        {
            get { return new Size(500, 300); }
        }

        /// <summary>
        ///     Automatically adjusts the height of this control and its child edit control.
        ///     For singleline (non-multiline) controls, the height is calculated from the child control's font size.
        /// </summary>
        private void AdjustRects(bool returnIfAnchored)
        {
            // TODO: Always adjust child rect properly, even when returnIfAnchored is true

            var rects = GetAdjustedRects(returnIfAnchored);

            AdjustParentRect(rects.Parent);
            AdjustChildRect(rects.Child);
        }

        private void AdjustParentRect(Rectangle rect)
        {
            Size = rect.Size;
        }

        private void AdjustChildRect(Rectangle rect)
        {
            Editor.Control.Size = rect.Size;
            Editor.Control.Location = rect.Location;
        }

        private void AdjustChildRect()
        {
            AdjustChildRect(GetAdjustedRects(false).Child);
        }

        private RectSet GetAdjustedRects(bool returnIfAnchored)
        {
            var parentSize = Size;
            var parentLocation = Location;

            var childSize = Editor.Control.Size;
            var childLocation = new Point(CalculatedPadding.Left, CalculatedPadding.Top);

            // --- Anchored

            // If we're anchored to two opposite sides of the form, don't adjust the size because
            // we'll lose our anchored size by resetting to the requested width.
            if (returnIfAnchored &&
                (Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom))
            {
                return new RectSet(new Rectangle(parentLocation, parentSize), new Rectangle(childLocation, childSize));
            }

            var lrPad = CalculatedPadding.Left + CalculatedPadding.Right;
            var tbPad = CalculatedPadding.Top  + CalculatedPadding.Bottom;

            // --- Multiline

            if (Multiline)
            {
                childSize = new Size(parentSize.Width - lrPad,
                                     parentSize.Height - tbPad);

                return new RectSet(new Rectangle(parentLocation, parentSize), new Rectangle(childLocation, childSize));
            }

            // --- Singleline

            var fontSize = FontSizeConverter.GetWinFormsFontSize(Editor.FontSize);
            var font = new Font(Font.FontFamily, (float)fontSize, Font.Style, GraphicsUnit.Point, Font.GdiCharSet, Font.GdiVerticalFont);

            Size measuredTextSize;

            using (var g = CreateGraphics())
            {
                var size = g.MeasureString("MQ", font);
                measuredTextSize = new Size((int) Math.Ceiling(size.Width), (int) Math.Ceiling(size.Height));
            }

            var parentWidth = parentSize.Width;
            var parentHeight = (int) Math.Ceiling((double) measuredTextSize.Height);

            parentSize = new Size(parentWidth,
                                  parentHeight + tbPad);

            childSize = new Size(parentWidth + Editor.VerticalScrollBarWidthAllowance - lrPad,
                                 parentHeight + Editor.HorizontalScrollBarHeightAllowance);

            return new RectSet(new Rectangle(parentLocation, parentSize), new Rectangle(childLocation, childSize));
        }

        private class RectSet
        {
            public readonly Rectangle Parent;
            public readonly Rectangle Child;

            public RectSet(Rectangle parent, Rectangle child)
            {
                Parent = parent;
                Child = child;
            }
        }

        #endregion

        #region Font

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Editor.FontSize = FontSizeConverter.GetWpfFontSize(Font.Size);
            AdjustRects(false);
        }

        #endregion

        #region Painting

        private event PaintEventHandler PaintBackground;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (PaintBackground != null)
                PaintBackground(this, e);
        }

        #endregion
    }
}
