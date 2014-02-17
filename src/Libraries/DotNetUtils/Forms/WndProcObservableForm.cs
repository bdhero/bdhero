using System.Windows.Forms;

namespace DotNetUtils.Forms
{
    public class WndProcObservableForm : Form, IWndProcObservable
    {
        public event WndProcEventHandler WndProcMessage;

        protected override void WndProc(ref Message m)
        {
            if (WndProcMessage != null)
            {
                WndProcMessage(ref m);
            }
            base.WndProc(ref m);
        }
    }
}
