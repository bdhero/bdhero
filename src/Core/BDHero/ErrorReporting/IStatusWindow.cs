using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BDHero.ErrorReporting
{
    internal interface IStatusWindow : IWin32Window
    {
        StatusStrip StatusStrip { get; set; }
    }
}
