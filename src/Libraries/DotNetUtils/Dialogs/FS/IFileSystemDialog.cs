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

using System.Windows.Forms;

namespace DotNetUtils.Dialogs.FS
{
    /// <summary>
    ///     Interface for a dialog window that interacts with the filesystem (e.g., "Open File", "Save File", "Open Folder").
    /// </summary>
    public interface IFileSystemDialog
    {
        /// <summary>
        ///     Gets or sets the title of the dialog.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        ///     Gets or sets the path to the currently selected file or folder.
        /// </summary>
        string SelectedPath { get; set; }

        /// <summary>
        ///     Displays the dialog as a modeless dialog.
        /// </summary>
        /// <returns>
        ///     The action that caused the dialog to close.
        /// </returns>
        DialogResult ShowDialog();

        /// <summary>
        ///     Displays the dialog as a modal dialog.
        /// </summary>
        /// <param name="owner">
        ///     The parent window.
        /// </param>
        /// <returns>
        ///     The action that caused the dialog to close.
        /// </returns>
        DialogResult ShowDialog(IWin32Window owner);
    }
}