using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSUtils.Windows
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
