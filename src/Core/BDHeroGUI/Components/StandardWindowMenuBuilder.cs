using System.Windows.Forms;
using BDHeroGUI.Forms;
using OSUtils.Window;

namespace BDHeroGUI.Components
{
    class StandardWindowMenuBuilder
    {
        private readonly Form _form;
        private readonly IWindowMenuFactory _factory;
        private readonly IWindowMenu _menu;

        /// <summary>
        ///     Position of the divider between the "Maximize" and "Close" menu items.
        /// </summary>
        private const uint InitialPos = 5;

        /// <summary>
        ///     Position counter.  Incremented as items are added to the menu.
        /// </summary>
        private uint _pos = InitialPos;

        private bool _isSeparatorAdded;

        public StandardWindowMenuBuilder(Form form, IWindowMenuFactory factory)
        {
            _form = form;
            _factory = factory;
            _menu = factory.CreateMenu(form);
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

            var resizeMenuItem = _factory.CreateMenuItem("&Resize...");
            resizeMenuItem.Clicked += delegate { new FormResizeWindow(_form).ShowDialog(_form); };
            _menu.InsertMenu(_pos++, resizeMenuItem);

            return this;
        }

        public StandardWindowMenuBuilder AlwaysOnTop()
        {
            EnsureSeparatorExists();

            var alwaysOnTopMenuItem = _factory.CreateMenuItem("Always on &top");
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
