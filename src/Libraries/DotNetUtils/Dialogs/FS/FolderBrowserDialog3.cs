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
using System.Windows.Forms;
using DotNetUtils.Extensions;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DotNetUtils.Dialogs.FS
{
    /// <summary>
    ///     Allows the user to select a single folder.
    ///     Compatible with Windows Vista and newer.
    /// </summary>
    public class FolderBrowserDialog3 : IFileSystemDialog
    {
        private readonly CommonOpenFileDialog _dialog;

        /// <summary>
        ///     Constructs a new <see cref="FolderBrowserDialog3"/> instance.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">
        ///     Thrown if the operating system is not Windows Vista or newer
        ///     or if the user is running Mono.
        /// </exception>
        public FolderBrowserDialog3()
        {
            _dialog = new CommonOpenFileDialog
                      {
                          EnsureReadOnly = true,
                          IsFolderPicker = true,
                          AllowNonFileSystemItems = false
                      };
        }

        /// <summary>
        ///     Not implemented.
        /// </summary>
        public bool ShowNewFolderButton
        {
            get { return false; }
            set {  }
        }

        public string Title
        {
            get { return _dialog.Title; }
            set { _dialog.Title = value; }
        }

        public string SelectedPath
        {
            get { return _dialog.FileAsShellObject.ParsingName; }
            set { _dialog.InitialDirectory = value; }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog().ToDialogResult();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return _dialog.ShowDialog(owner.Handle).ToDialogResult();
        }
    }
}