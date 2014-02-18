using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSUtils.Windows
{
    public interface IWindowMenu
    {
        void AppendMenu(IWindowMenuItem menuItem);

        void AppendSeparator();

        void InsertMenu(uint position, IWindowMenuItem menuItem);

        void InsertSeparator(uint position);

        void UpdateMenu(IWindowMenuItem menuItem);
    }
}
