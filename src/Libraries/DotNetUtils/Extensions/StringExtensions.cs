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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="String"/>s.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to Title Case (a.k.a., Proper Case).
        /// </summary>
        public static String ToTitle(this String str)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            var titleCase = textInfo.ToTitleCase(textInfo.ToLower(str));
            return titleCase;
        }

        /// <summary>
        /// Removes HTML tags and comments.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// TODO: Write unit tests
        public static string StripHtml(this string str)
        {
            return new Regex(@"</?[a-z][a-z0-9]*[^<>]*>", RegexOptions.IgnoreCase)
                .Replace(str, "")
                .RegexReplace(@"<!--.*?-->", "")
                .RegexReplace(@"[\s\n\r\f]+", " ")
                .Trim();
        }

        /// TODO: Write unit tests
        public static string UnescapeHtml(this string str)
        {
            return WebUtility.HtmlDecode(str);
        }

        #region CsQuery.ExtensionMethods

        // https://github.com/jamietre/CsQuery/blob/master/source/CsQuery/ExtensionMethods/ExtensionMethods.cs

        /// <summary>
        /// Perform a substring replace using a regular expression.
        /// </summary>
        ///
        /// <param name="input">
        /// The target of the replacement.
        /// </param>
        /// <param name="pattern">
        /// The pattern to match.
        /// </param>
        /// <param name="replacement">
        /// The replacement string.
        /// </param>
        ///
        /// <returns>
        /// A new string.
        /// </returns>
        public static String RegexReplace(this String input, string pattern, string replacement)
        {
            return input.RegexReplace(new[] { pattern }, new[] { replacement });
        }

        /// <summary>
        /// Perform a substring replace using a regular expression and one or more patterns
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when the list of replacements is not the same length as the list of patterns.
        /// </exception>
        /// <param name="input">
        /// The target of the replacement.
        /// </param>
        /// <param name="patterns">
        /// The patterns.
        /// </param>
        /// <param name="replacements">
        /// The replacements.
        /// </param>
        /// <returns>
        /// A new string.
        /// </returns>
        public static String RegexReplace(this String input, IEnumerable<string> patterns, IEnumerable<string> replacements)
        {
            List<string> patternList = new List<string>(patterns);
            List<string> replacementList = new List<string>(replacements);
            if (replacementList.Count != patternList.Count)
            {
                throw new ArgumentException("Mismatched pattern and replacement lists.");
            }

            for (var i = 0; i < patternList.Count; i++)
            {
                input = Regex.Replace(input, patternList[i], replacementList[i]);
            }

            return input;
        }

        /// <summary>
        /// Perform a substring replace using a regular expression.
        /// </summary>
        /// <param name="input">
        /// The target of the replacement.
        /// </param>
        /// <param name="pattern">
        /// The pattern to match.
        /// </param>
        /// <param name="evaluator">
        /// The evaluator.
        /// </param>
        /// <returns>
        /// A new string.
        /// </returns>
        public static string RegexReplace(this String input, string pattern, MatchEvaluator evaluator)
        {
            return Regex.Replace(input, pattern, evaluator);
        }

        /// <summary>
        /// Test whether the regular expression pattern matches the string.
        /// </summary>
        /// <param name="input">
        /// The string to test
        /// </param>
        /// <param name="pattern">
        /// The pattern
        /// </param>
        /// <returns>
        /// true if the pattern matches, false if not.
        /// </returns>
        public static bool RegexTest(this String input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }


        #endregion
    }
}