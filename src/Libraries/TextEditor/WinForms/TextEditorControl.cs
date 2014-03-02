using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TextEditor.WinForms
{
    public class TextEditorControl : Control
    {
        public TextEditorControl()
        {
            Editor = TextEditorFactory.CreateTextEditor();
            Editor.TextChanged += (sender, args) => OnTextChanged(args);
            Editor.FontSizeChanged += (sender, args) => OnFontChanged(args);

            Editor.Control.Dock = DockStyle.Fill;
            Controls.Add(Editor.Control);

            SetStyle(ControlStyles.Selectable, false);

            EnableContextMenu = true;
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

        public ITextEditor Editor { get; private set; }

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

        public override string Text
        {
            get { return Editor.Text; }
            set { Editor.Text = value; }
        }

        #endregion

        #region Multiline

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

                if (!value)
                {
//                    Editor.Options.ConvertTabsToSpaces = false;
                    Editor.Options.CutCopyWholeLine = false;
                    Editor.Options.ShowColumnRuler = false;
                    Editor.Options.ShowLineNumbers = false;
//                    Editor.Options.ShowSpaces = false;
//                    Editor.Options.ShowTabs = false;
                }

                SetStyle(ControlStyles.FixedHeight, !value);

                RecreateHandle();

//                AdjustHeight(false);
                OnMultilineChanged(EventArgs.Empty);
            }
        }

        [Browsable(true)]
        public event EventHandler MultilineChanged;

        protected virtual void OnMultilineChanged(EventArgs args)
        {
            if (MultilineChanged != null)
                MultilineChanged(this, args);
        }

        #endregion

        #region Font

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
//            AdjustHeight(false);
        }

        #endregion
    }
}
