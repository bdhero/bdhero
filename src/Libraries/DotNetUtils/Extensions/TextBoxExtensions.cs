// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

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
