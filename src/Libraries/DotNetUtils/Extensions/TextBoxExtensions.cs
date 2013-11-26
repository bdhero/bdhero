using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static void SelectVariablesOnClick(this TextBox textBox)
        {
            textBox.Click += TextBoxOnClick;
        }

        private static void TextBoxOnClick(object sender, EventArgs eventArgs)
        {
            var textBox = sender as TextBoxBase;
            if (textBox == null) { return; }

            if (textBox.SelectionLength > 0) { return; }

            var tokens = new Regex(@"%\w+%").Matches(textBox.Text).OfType<Match>().ToArray();
            var token = tokens.FirstOrDefault(match => ShouldSelectToken(match, textBox));

            if (token == null) { return; }

            textBox.Select(token.Index, token.Length);
        }

        private static bool ShouldSelectToken(Match match, TextBoxBase label)
        {
            var start = match.Index;
            var end = start + match.Length;
            var caret = label.SelectionStart;
            return caret >= start && caret <= end;
        }
    }
}
