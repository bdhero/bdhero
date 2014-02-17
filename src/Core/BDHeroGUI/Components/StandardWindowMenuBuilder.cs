using System;
using System.Windows.Forms;
using WindowsOSUtils.Windows;
using BDHeroGUI.Forms;
using DotNetUtils.Forms;

namespace BDHeroGUI.Components
{
    class StandardWindowMenuBuilder
    {
        private readonly WndProcObservableForm _form;
        private readonly WindowMenu _menu;

        /// <summary>
        ///     Position of the divider between the "Maximize" and "Close" menu items.
        /// </summary>
        private const uint InitialPos = 5;

        /// <summary>
        ///     Position counter.  Incremented as items are added to the menu.
        /// </summary>
        private uint _pos = InitialPos;

        private bool _isSeparatorAdded;

        public StandardWindowMenuBuilder(WndProcObservableForm form)
        {
            _form = form;
            _menu = new WindowMenu(form);
        }

        private void EnsureSeparatorExists()
        {
            if (_isSeparatorAdded)
                return;

            Separator();

            _isSeparatorAdded = true;
        }

        public StandardWindowMenuBuilder Resize()
        {
            EnsureSeparatorExists();

            var resizeMenuItem = _menu.CreateMenuItem("&Resize...");
            resizeMenuItem.Clicked += delegate { new FormResizeWindow(_form).ShowDialog(_form); };
            _menu.InsertMenu(_pos++, resizeMenuItem);

            return this;
        }

        public StandardWindowMenuBuilder AlwaysOnTop()
        {
            EnsureSeparatorExists();

            var alwaysOnTopMenuItem = _menu.CreateMenuItem("Always on &top");
            alwaysOnTopMenuItem.Clicked += delegate
                                           {
                                               var alwaysOnTop = !alwaysOnTopMenuItem.Checked;
                                               _form.TopMost = alwaysOnTop;
                                               alwaysOnTopMenuItem.Checked = alwaysOnTop;
                                               _menu.UpdateMenu(alwaysOnTopMenuItem);
                                           };
            _menu.InsertMenu(_pos++, alwaysOnTopMenuItem);

            return this;
        }

        public StandardWindowMenuBuilder Separator()
        {
            _menu.InsertSeparator(_pos++);
            return this;
        }
    }
}
