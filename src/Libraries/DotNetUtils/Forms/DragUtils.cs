// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils.FS;

// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace DotNetUtils.Forms
{
    /// <summary>
    ///     Utility class containing helper methods for working with drag and drop operations in Windows Forms applications.
    /// </summary>
    public static class DragUtils
    {
        /// <summary>
        ///     Gets a list of all files in the drag event <paramref name="e"/> that have a file extension
        ///     that matches one of the extensions in <paramref name="extensions"/>.
        /// </summary>
        /// <param name="e">Drag event args.</param>
        /// <param name="extensions">
        ///     Collection of file extensions of the form <c>".ext"</c> or <c>"ext"</c> (case insensitive).
        /// </param>
        /// <returns></returns>
        public static IList<string> GetFilesWithExtension(DragEventArgs e, params string[] extensions)
        {
            return GetFilePaths(e).Where(path => FileUtils.FileHasExtension(path, extensions)).ToList();
        }

        /// <summary>
        ///     Gets a list of all file and directory paths in the given drag event <paramref name="e"/>.
        /// </summary>
        /// <param name="e">Drag event args.</param>
        /// <returns>List of all file and directory paths in the drag event.</returns>
        public static IList<string> GetPaths(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            }
            return new string[0];
        }

        /// <summary>
        ///     Gets a list of all file paths in the given drag event <paramref name="e"/>.
        /// </summary>
        /// <param name="e">Drag event args.</param>
        /// <returns>List of all file paths in the drag event.</returns>
        public static IList<string> GetFilePaths(DragEventArgs e)
        {
            return GetPaths(e).Where(FileUtils.IsFile).ToList();
        }

        /// <summary>
        ///     Gets a list of all directory paths in the given drag event <paramref name="e"/>.
        /// </summary>
        /// <param name="e">Drag event args.</param>
        /// <returns>List of all directory paths in the drag event.</returns>
        public static ICollection<string> GetDirectoryPaths(DragEventArgs e)
        {
            return GetPaths(e).Where(FileUtils.IsDirectory).ToArray();
        }

        /// <summary>
        ///     Extracts the unicode text (if any) contained within the drag event <paramref name="e"/>.
        /// </summary>
        /// <param name="e">Drag event args.</param>
        /// <returns>Unicode text (if any) from the drag event.</returns>
        public static string GetUnicodeText(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                return (string) e.Data.GetData(DataFormats.UnicodeText, false);
            }
            return null;
        }
    }
}
