using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TextBox"/> controls.
    /// </summary>
    public static class TextBoxExtensions
    {
        /// <summary>
        /// Highlights the TextBox by changing its <see cref="TextBox.BorderStyle"/>.
        /// </summary>
        /// <param name="textBox"></param>
        public static void Highlight(this TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Removes highlighting from the TextBox by reverting its <see cref="TextBox.BorderStyle"/> back to its original value.
        /// </summary>
        /// <param name="textBox"></param>
        public static void UnHighlight(this TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.Fixed3D;
        }
    }
}
