using System.Windows.Forms;

namespace DotNetUtils.Forms
{
    /// <summary>
    ///     Interface for a Windows Forms control that notifies observers whenever its <see cref="Control.WndProc"/> method
    ///     is called.
    /// </summary>
    public interface IWndProcObservable
    {
        /// <summary>
        ///     Invoked whenever the implementing class receives a Windows <see cref="Message"/>.
        /// </summary>
        event WndProcEventHandler WndProcMessage;
    }

    /// <summary>
    ///     Represents a method that will handle a Windows <see cref="Message"/>.
    /// </summary>
    /// <param name="m">Windows Message received from the operating system.</param>
    public delegate void WndProcEventHandler(ref Message m);
}
