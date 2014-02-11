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

using System.IO;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils.FS;

namespace DotNetUtils.Dialogs.FS
{
    /// <summary>
    ///     Allows the user to save a file.
    /// </summary>
    public class SaveFileDialog2 : IFileSystemDialog
    {
        private readonly SaveFileDialog _dialog = new SaveFileDialog();
        private FileType[] _fileTypes;

        /// <summary>
        ///     Constructs a new <see cref="SaveFileDialog2"/> instance.
        /// </summary>
        public SaveFileDialog2()
        {
            _dialog.AutoUpgradeEnabled = true;
            _dialog.CheckPathExists = true;
            _dialog.DereferenceLinks = true;
        }

        /// <summary>
        ///     Gets or sets whether to prompt the user before overwriting an existing file or not.
        /// </summary>
        public bool OverwritePrompt
        {
            get { return _dialog.OverwritePrompt; }
            set { _dialog.OverwritePrompt = value; }
        }

        /// <summary>
        ///     Gets or sets the list of allowed file types that the user can select.
        /// </summary>
        public FileType[] FileTypes
        {
            get { return _fileTypes; }
            set
            {
                _fileTypes = value;
                SetFilter();
            }
        }

        /// <summary>
        ///     Gets or sets whether to add an "Any file" option to the list of file types.
        /// </summary>
        public bool AllowAnyExtension { get; set; }

        private void SetFilter()
        {
            var exts = FileTypes.ToList();

            if (AllowAnyExtension)
            {
                exts.Add(new FileType
                         {
                             Description = "Any file",
                             Extensions = new[] { ".*" }
                         });
            }

            // Format: "Image Files (*.BMP; *.JPG; *.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            _dialog.Filter = string.Join("|", exts);
        }

        public string Title
        {
            get { return _dialog.Title; }
            set { _dialog.Title = value; }
        }

        public string SelectedPath
        {
            get { return _dialog.FileName; }
            set
            {
                _dialog.InitialDirectory = Path.GetDirectoryName(value);
                _dialog.FileName = Path.GetFileName(value);
            }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return _dialog.ShowDialog(owner);
        }
    }
}