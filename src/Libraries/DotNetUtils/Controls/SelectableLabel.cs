using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetUtils.Controls
{
    public class SelectableLabel : TextBox
    {
        public SelectableLabel()
        {
            ReadOnly = true;
            BorderStyle = BorderStyle.None;
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            UpdateBackgroundColor();
        }

        private void UpdateBackgroundColor()
        {
            try
            {
                BackColor = Parent.BackColor;
            }
            catch
            {
            }
        }
    }
}
