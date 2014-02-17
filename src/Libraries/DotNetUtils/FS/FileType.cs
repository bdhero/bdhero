// Copyright 2013-2014 Andrew C. Dvorak
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

using System.Linq;
using System.Windows.Forms;

namespace DotNetUtils.FS
{
    /// <summary>
    ///     Represents a set of logically-related file extensions that form a single file type.
    /// </summary>
    public struct FileType
    {
        /// <summary>
        ///     Gets or sets a list of file extensions represented by this set.
        /// </summary>
        /// <example>
        ///     <c>{ ".mkv", ".mka", "mks" }</c>
        /// </example>
        public string[] Extensions;

        /// <summary>
        ///     Gets or sets a brief description of the file type.
        /// </summary>
        /// <example>
        ///     <c>"Matroska container file"</c>
        /// </example>
        public string Description;

        /// <summary>
        ///     Returns a string that can be assigned to <see cref="FileDialog.Filter"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var exts =
                FileUtils.NormalizeFileExtensions(Extensions)
                         .Select(ext => string.Format("*{0}", ext))
                         .ToArray();
            return string.Format("{0} ({1})|{2}", Description, string.Join("; ", exts), string.Join(";", exts));
        }
    }
}