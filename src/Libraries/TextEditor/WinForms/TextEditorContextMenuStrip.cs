using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;

namespace TextEditor.WinForms
{
    internal class TextEditorContextMenuStrip : ContextMenuStrip
    {
        private readonly ITextEditor _editor;

        private readonly List<EventHandler> _eventHandlers = new List<EventHandler>();

        private readonly ToolStripMenuItem _undo;
        private readonly ToolStripMenuItem _redo;
        private readonly ToolStripMenuItem _cut;
        private readonly ToolStripMenuItem _copy;
        private readonly ToolStripMenuItem _paste;
        private readonly ToolStripMenuItem _delete;
        private readonly ToolStripMenuItem _selectAll;

        private readonly ToolStripSeparator _optionsDivider;
        private readonly ToolStripMenuItem _options;

        private readonly ToolStripMenuItem _showLineNumbers;
        private readonly ToolStripMenuItem _showWhiteSpace;
        private readonly ToolStripMenuItem _wordWrap;
        private readonly ToolStripMenuItem _ruler;

        private readonly ToolStripMenuItem _none;
        private readonly ToolStripMenuItem _seventy;
        private readonly ToolStripMenuItem _seventyEight;
        private readonly ToolStripMenuItem _eighty;
        private readonly ToolStripMenuItem _oneHundred;
        private readonly ToolStripMenuItem _oneHundredTwenty;

        public TextEditorContextMenuStrip(ITextEditor editor)
        {
            _editor = editor;

            #region Edit commands

            _undo = CreateMenuItem("&Undo", Undo);
            _redo = CreateMenuItem("&Redo", Redo);

            _cut = CreateMenuItem("Cu&t", Cut);
            _copy = CreateMenuItem("&Copy", Copy);
            _paste = CreateMenuItem("&Paste", Paste);
            _delete = CreateMenuItem("&Delete", Delete);

            _selectAll = CreateMenuItem("Select &All", SelectAll);

            #endregion

            #region Options

            _optionsDivider = CreateSeparator();

            _options = CreateMenuItem("&Options");
            _options.DropDown.Closing += OptionsDropDownOnClosing;

            _showLineNumbers = CreateOptionsMenuItem("Show &Line Numbers", ToggleShowLineNumbers);
            _showWhiteSpace = CreateOptionsMenuItem("Show &Whitespace", ToggleShowWhiteSpace);
            _wordWrap = CreateOptionsMenuItem("Word &Wrap", ToggleWordWrap);

            #region Ruler

            _ruler = CreateMenuItem("&Ruler");
            _ruler.DropDown.Closing += OptionsDropDownOnClosing;

            _none             = CreateRulerMenuItem("None");
            _seventy          = CreateRulerMenuItem(70);
            _seventyEight     = CreateRulerMenuItem(78);
            _eighty           = CreateRulerMenuItem(80);
            _oneHundred       = CreateRulerMenuItem(100);
            _oneHundredTwenty = CreateRulerMenuItem(120);

            _ruler.DropDownItems.AddRange(new ToolStripItem[]
                                          {
                                              _none,
                                              CreateSeparator(),
                                              _seventy,
                                              _seventyEight,
                                              _eighty,
                                              _oneHundred,
                                              _oneHundredTwenty,
                                          });

            #endregion

            _options.DropDownItems.AddRange(new ToolStripItem[]
                                            {
                                                _showLineNumbers,
                                                _showWhiteSpace,
                                                CreateSeparator(),
                                                _wordWrap,
                                                _ruler,
                                            });

            #endregion

            #region Top-level menu

            base.Items.AddRange(new ToolStripItem[]
                                {
                                    _undo,
                                    _redo,
                                    CreateSeparator(),
                                    _cut,
                                    _copy,
                                    _paste,
                                    _delete,
                                    CreateSeparator(),
                                    _selectAll,
                                    _optionsDivider,
                                    _options,
                                });

            Opened += (sender, args) => SetMenuItemStates();

            #endregion
        }

        ~TextEditorContextMenuStrip()
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

        private void UnbindEvents()
        {
            Items.OfType<ToolStripMenuItem>().ForEach(UnbindClickEvents);
            _eventHandlers.Clear();
        }

        private void UnbindClickEvents(ToolStripMenuItem item)
        {
            foreach (var handler in _eventHandlers)
            {
                item.Click -= handler;
            }
        }

        #region Actions

        private void Undo()
        {
            _editor.Undo();
        }

        private void Redo()
        {
            _editor.Redo();
        }

        private void Cut()
        {
            _editor.Cut();
        }

        private void Copy()
        {
            _editor.Copy();
        }

        private void Paste()
        {
            _editor.Paste();
        }

        private void Delete()
        {
            _editor.Delete();
        }

        private void SelectAll()
        {
            _editor.SelectAll();
        }

        private void ToggleShowLineNumbers()
        {
            _editor.Options.ShowLineNumbers = !_editor.Options.ShowLineNumbers;
        }

        private void ToggleShowWhiteSpace()
        {
            _editor.Options.ShowWhiteSpace = !_editor.Options.ShowWhiteSpace;
        }

        private void ToggleWordWrap()
        {
            _editor.Options.WordWrap = !_editor.Options.WordWrap;
        }

        #endregion

        private void SetMenuItemStates()
        {
            SetEditCommandMenuItemStates();
            SetOptionsMenuItemStates();
        }

        private void SetEditCommandMenuItemStates()
        {
            _undo.Enabled    = _editor.CanUndo   && !_editor.ReadOnly;
            _redo.Enabled    = _editor.CanRedo   && !_editor.ReadOnly;
            _cut.Enabled     = _editor.CanCut    && !_editor.ReadOnly;
            _copy.Enabled    = _editor.CanCopy;
            _paste.Enabled   = _editor.CanPaste  && !_editor.ReadOnly;
            _delete.Enabled  = _editor.CanDelete && !_editor.ReadOnly;
        }

        private void SetOptionsMenuItemStates()
        {
            _optionsDivider.Visible  = _editor.Multiline;
            _options.Visible         = _editor.Multiline;

            _showLineNumbers.Checked = _editor.Options.ShowLineNumbers;
            _showWhiteSpace.Checked  = _editor.Options.ShowWhiteSpace;
            _wordWrap.Visible        = _editor.Options.SupportsWordWrap;
            _wordWrap.Checked        = _editor.Options.WordWrap;

            SetWordWrapMenuItemStates();
        }

        private void SetWordWrapMenuItemStates()
        {
            _none.Checked             = !_editor.Options.ShowColumnRuler;
            _seventy.Checked          = _editor.Options.ShowColumnRuler && _editor.Options.ColumnRulerPosition == 70;
            _seventyEight.Checked     = _editor.Options.ShowColumnRuler && _editor.Options.ColumnRulerPosition == 78;
            _eighty.Checked           = _editor.Options.ShowColumnRuler && _editor.Options.ColumnRulerPosition == 80;
            _oneHundred.Checked       = _editor.Options.ShowColumnRuler && _editor.Options.ColumnRulerPosition == 100;
            _oneHundredTwenty.Checked = _editor.Options.ShowColumnRuler && _editor.Options.ColumnRulerPosition == 120;
        }

        private void OptionsDropDownOnClosing(object sender, ToolStripDropDownClosingEventArgs args)
        {
            if (args.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                args.Cancel = true;
            }
        }

        private void OptionsMenuItemOnClick(Action action)
        {
            action();
            SetMenuItemStates();
        }

        #region Creation

        private ToolStripMenuItem CreateMenuItem(string text, Action clickAction = null)
        {
            var item = new ToolStripMenuItem(text, null);
            if (clickAction != null)
            {
                EventHandler handler = (sender, args) => clickAction();
                _eventHandlers.Add(handler);
                item.Click += handler;
            }
            return item;
        }

        private ToolStripMenuItem CreateOptionsMenuItem(string text, [NotNull] Action clickAction)
        {
            return CreateMenuItem(text, () => OptionsMenuItemOnClick(clickAction));
        }

        private static ToolStripSeparator CreateSeparator()
        {
            return new ToolStripSeparator();
        }

        private ToolStripMenuItem CreateRulerMenuItem(string text)
        {
            var item = CreateMenuItem(string.Format("&{0}", text));
            item.Click += (sender, args) => OptionsMenuItemOnClick(() => SetRulerColumn(0));
            return item;
        }

        private ToolStripMenuItem CreateRulerMenuItem(int col)
        {
            var item = CreateMenuItem(string.Format("&{0}", col));
            item.Click += (sender, args) => OptionsMenuItemOnClick(() => SetRulerColumn(col));
            return item;
        }

        #endregion

        private void SetRulerColumn(int col)
        {
            if (col > 0)
            {
                _editor.Options.ShowColumnRuler = true;
                _editor.Options.ColumnRulerPosition = col;
            }
            else
            {
                _editor.Options.ShowColumnRuler = false;
            }
        }
    }
}
