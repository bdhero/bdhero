using System;

namespace WindowsOSUtils.Windows
{
    public class WindowMenuItem
    {
        public readonly uint Id;

        public string Text;
        public bool Enabled = true;
        public bool Checked = false;

        public event EventHandler Clicked;

        internal WindowMenuItem(uint id)
        {
            Id = id;
        }

        public void Click(EventArgs e)
        {
            if (Clicked != null)
            {
                Clicked(this, e);
            }
        }
    }
}