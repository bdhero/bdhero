using System;

namespace OSUtils.Window
{
    public interface IWindowMenuItem
    {
        uint Id { get; }

        string Text { get; set; }

        bool Enabled { get; set; }

        bool Checked { get; set; }

        event EventHandler Clicked;

        void Click(EventArgs eventArgs);
    }
}
