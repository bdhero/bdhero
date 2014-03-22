using System.ComponentModel;
using System.Windows.Forms;

namespace WpfUtils
{
    /// <summary>
    ///     Invoked whenever the hooked control's <c>WndProc</c> method is called.
    /// </summary>
    /// <param name="m">
    ///     Native window message.
    /// </param>
    /// <param name="args">
    ///     Event data.
    /// </param>
    public delegate void WpfWndProcEventHandler(ref Message m, HandledEventArgs args);
}