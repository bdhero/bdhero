using System.ComponentModel;
using System.Windows.Forms;

namespace DotNetUtils.Forms
{
    /// <summary>
    ///     Invoked whenever the hooked control's <see cref="Control.WndProc"/> method is called.
    /// </summary>
    /// <param name="m">
    ///     Native window message.
    /// </param>
    /// <param name="args">
    ///     Event data.
    /// </param>
    public delegate void WndProcEventHandler(ref Message m, HandledEventArgs args);
}