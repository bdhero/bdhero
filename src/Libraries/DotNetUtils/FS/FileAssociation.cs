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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.IconLib;
using System.IO;
using System.Linq;
using System.Text;
using DotNetUtils.Annotations;
using NativeAPI.Win.ShellLightWeight;

namespace DotNetUtils.FS
{
    /// <summary>
    ///     Represents a file and the default program that is associated with it (if any).
    /// </summary>
    public class FileAssociation
    {
        /// <summary>
        ///     Gets the relative or absolute path to the file that <see cref="ExePath"/> is associated with and
        ///     to which this <see cref="FileAssociation"/> object belongs.
        /// </summary>
        public readonly string FilePath;

        /// <summary>
        ///     Full, absolute path to the executable associated with <see cref="FilePath"/>.
        /// </summary>
        [CanBeNull]
        public readonly string ExePath;

        /// <summary>
        ///     Human-friendly name of the application associated with <see cref="FilePath"/>.
        /// </summary>
        /// <example>
        ///     <c>"BDHero GUI"</c>
        /// </example>
        [CanBeNull]
        public readonly string AppName;

        /// <summary>
        ///     Human-friendly name of the product suite that contains the program associated with <see cref="FilePath"/>.
        /// </summary>
        [CanBeNull]
        public readonly string ProductName;

        /// <summary>
        ///     Gets whether <see cref="FilePath"/> has a default program associated with it or not.
        /// </summary>
        public bool HasAssociation
        {
            get { return ExePath != null; }
        }

        /// <summary>
        ///     Gets whether <see cref="FilePath"/> can be opened via <see cref="Process.Start(string)"/>.
        /// </summary>
        public bool CanBeOpened
        {
            get { return GetDefaultProgram(FilePath) != null; }
        }

        [CanBeNull]
        private readonly MultiIcon _multiIcon;

        #region Constructor

        /// <summary>
        ///     Constructs a new <see cref="FileAssociation"/> object containing information about the default program
        ///     associated with the given <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">
        ///     Relative or absolute path to a file (e.g., <c>"C:\some\file.docx"</c>).
        /// </param>
        public FileAssociation(string filePath)
        {
            FilePath = filePath;
            ExePath = GetExePath();
            AppName = GetAppName();
            ProductName = GetProductName();
            _multiIcon = GetMultiIcon();
        }

        #region Initialization

        [CanBeNull]
        private string GetExePath()
        {
            return HasExplicitAssociation(FilePath) ? GetDefaultProgram(FilePath) : null;
        }

        [CanBeNull]
        private string GetAppName()
        {
            return ExePath == null ? null : FileVersionInfo.GetVersionInfo(ExePath).FileDescription;
        }

        [CanBeNull]
        private string GetProductName()
        {
            return ExePath == null ? null : FileVersionInfo.GetVersionInfo(ExePath).ProductName;
        }

        [CanBeNull]
        private MultiIcon GetMultiIcon()
        {
            if (ExePath == null)
            {
                return null;
            }

            var multiIcon = new MultiIcon();
            multiIcon.Load(ExePath);
            return multiIcon;
        }

        #endregion

        #endregion

        #region Icon getters

        /// <summary>
        ///     Gets the first icon image from the default program that has the specified <paramref name="size"/>.
        /// </summary>
        /// <param name="size">
        ///     Height and width of the icon to return.
        /// </param>
        /// <returns>
        ///     The first icon image with the specified <paramref name="size"/>, or <c>null</c> if none were found.
        /// </returns>
        [CanBeNull]
        public IconImage GetProgramIconImage(int size)
        {
            if (_multiIcon == null || !_multiIcon.Any())
            {
                return null;
            }
            var sizeObj = new Size(size, size);
            var iconImages = _multiIcon.First()
                                       .Where(img => img.Size.Equals(sizeObj))
                                       .OrderByDescending(img => img.ColorsInPalette)
                                       .ToArray();
            return iconImages.FirstOrDefault();
        }

        /// <summary>
        ///     Gets the first icon from the default program that has the specified <paramref name="size"/>.
        /// </summary>
        /// <param name="size">
        ///     Height and width of the icon to return.
        /// </param>
        /// <returns>
        ///     The first icon with the specified <paramref name="size"/>, or <c>null</c> if none were found.
        /// </returns>
        [CanBeNull]
        public Icon GetProgramIcon(int size)
        {
            var iconImage = GetProgramIconImage(size);
            return iconImage == null ? null : iconImage.Icon;
        }

        /// <summary>
        ///     Gets the first icon from the default program that has the specified <paramref name="size"/>
        ///     as a transparent bitmap.
        /// </summary>
        /// <param name="size">
        ///     Height and width of the icon to return.
        /// </param>
        /// <returns>
        ///     The first icon with the specified <paramref name="size"/> as a transparent bitmap, or <c>null</c>
        ///     if none were found.
        /// </returns>
        [CanBeNull]
        public Image GetProgramImage(int size)
        {
            var iconImage = GetProgramIconImage(size);
            return iconImage == null ? null : iconImage.Transparent;
        }

        #endregion

        /// <summary>
        ///     Gets the full, absolute path to the default program associated with the given <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">
        ///     Relative or absolute path to a file.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        ///     Starting in Windows 8, file types that don't have an explicit program association
        ///     are implicitly associated with <c>C:\Windows\System32\OpenWith.exe</c>.
        ///     Use <see cref="HasExplicitAssociation"/> to check if the file has an explicit association.
        /// </remarks>
        [CanBeNull]
        public static string GetDefaultProgram([CanBeNull] string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            if (!Path.HasExtension(filePath))
            {
                return null;
            }

            try
            {
                var defaultExePath = FileExtentionInfo(AssocStr.Executable, Path.GetExtension(filePath));

                if (string.IsNullOrEmpty(defaultExePath))
                    return null;

                if (!File.Exists(defaultExePath))
                    return null;

                return defaultExePath;
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        ///     Determines if the given file type has a program explicitly associated with it in the Windows registry.
        /// </summary>
        /// <param name="filePath">
        ///     Relative or absolute path to a file.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the given file has a program associated with it; otherwise <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     Starting in Windows 8, file types that don't have an explicit program association
        ///     are implicitly associated with <c>C:\Windows\System32\OpenWith.exe</c>.
        /// </remarks>
        public static bool HasExplicitAssociation(string filePath)
        {
            var exePath = GetDefaultProgram(filePath);
            var exeName = Path.GetFileName(exePath);

            if (string.IsNullOrEmpty(exePath))
                return false;

            if ("OpenWith.exe".Equals(exeName, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        #region Native Win32 interop

        private static string FileExtentionInfo(AssocStr assocStr, string doctype)
        {
            uint pcchOut = 0;
            AssociationAPI.AssocQueryString(AssocF.Verify, assocStr, doctype, null, null, ref pcchOut);

            var pszOut = new StringBuilder((int)pcchOut);
            AssociationAPI.AssocQueryString(AssocF.Verify, assocStr, doctype, null, pszOut, ref pcchOut);
            return pszOut.ToString();
        }

        #endregion
    }
}
