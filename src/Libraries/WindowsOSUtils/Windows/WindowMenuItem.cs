using System;
using OSUtils.Window;

namespace WindowsOSUtils.Windows
{
    public class WindowMenuItem : IWindowMenuItem
    {
        public uint Id { get; private set; }

        public string Text { get; set; }

        public bool Enabled { get; set; }

        public bool Checked { get; set; }

        public event EventHandler Clicked;

        internal WindowMenuItem(uint id)
        {
            Id = id;
            Enabled = true;
            Checked = false;
        }

        public void Click(EventArgs eventArgs)
        {
            if (Clicked != null)
            {
                Clicked(this, eventArgs);
            }
        }
    }
}