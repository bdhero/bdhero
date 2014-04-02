using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TextEditor.WPF
{
    internal class ElementHostImpl : ElementHost
    {
        public void TriggerMouseEnter()
        {
            OnMouseEnter(EventArgs.Empty);
        }

        public void TriggerMouseLeave()
        {
            OnMouseLeave(EventArgs.Empty);
        }

        public void TriggerPreviewKeyDown(Keys keyData)
        {
            OnPreviewKeyDown(new PreviewKeyDownEventArgs(keyData));
        }
    }
}