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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WindowsOSUtils.JobObjects
{
    /// <summary>
    /// Helper class for constructing a list of arguments to be passed to a command line application.
    /// </summary>
    public class ArgumentList : List<string>
    {
        private const string DoubleQuote = "\"";
        private const string DoubleQuoteEscaped = "\\\"";

        private static readonly Regex ReservedShellCharsRegex = new Regex("[ &|()<>^\"]");

        /// <summary>
        /// Gets or sets whether null/empty arguments should be kept by ToString() as a set of two double quotes ("") or skipped.
        /// </summary>
        public bool KeepEmptyArgs = true;

        public ArgumentList(params string[] args)
        {
            AddAll(args);
        }

        public ArgumentList(IEnumerable<string> args)
        {
            AddAll(args);
        }

        /// <summary>
        /// Adds all arguments, regardless of whether they are null or empty.
        /// </summary>
        /// <param name="args"></param>
        public void AddAll(params string[] args)
        {
            AddRange(args);
        }

        /// <summary>
        /// Adds all arguments, regardless of whether they are null or empty.
        /// </summary>
        /// <param name="args"></param>
        public void AddAll(IEnumerable<string> args)
        {
            AddRange(args ?? new string[] { });
        }

        /// <summary>
        /// Adds all non-null, non-empty arguments (i.e., skips null/empty arguments).
        /// </summary>
        /// <param name="args"></param>
        public void AddNonEmpty(params string[] args)
        {
            AddAll(args.Where(arg => !string.IsNullOrEmpty(arg)));
        }

        /// <summary>
        /// Adds all arguments iff all arguments are non-null and non-empty.  If any arguments are null or empty, nothing is added.
        /// </summary>
        /// <param name="args"></param>
        public void AddIfAllNonEmpty(params string[] args)
        {
            if (args.Any(string.IsNullOrEmpty))
                return;

            AddAll(args);
        }

        private bool KeepArg(string rawArg)
        {
            return rawArg != null || KeepEmptyArgs;
        }

        public static string Escape(string rawArg)
        {
            return string.Format("{0}", rawArg).Replace(DoubleQuote, DoubleQuoteEscaped);
        }

        public static string ForCommandLine(string rawArg)
        {
            rawArg = rawArg ?? "";
            return
                string.IsNullOrEmpty(rawArg) || ReservedShellCharsRegex.IsMatch(rawArg)
                    ? string.Format("{0}{1}{0}", DoubleQuote, Escape(rawArg))
                    : rawArg;
        }

        public override string ToString()
        {
            return string.Join(" ", this.Where(KeepArg).Select(ForCommandLine));
        }
    }
}
