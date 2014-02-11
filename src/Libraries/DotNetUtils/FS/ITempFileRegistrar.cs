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
using System.IO;
using System.Reflection;

namespace DotNetUtils.FS
{
    /// <summary>
    ///     Interface for a class that manages temporary files and directories.
    /// </summary>
    public interface ITempFileRegistrar : IDisposable
    {
        /// <summary>
        ///     Registers the given <paramref name="filePaths"/> for deletion when the application exits.
        /// </summary>
        /// <param name="filePaths">Paths to files that should be deleted when the application exits.</param>
        void RegisterTempFiles(params string[] filePaths);

        /// <summary>
        ///     Registers the given <paramref name="dirPaths"/> for deletion when the application exits.
        /// </summary>
        /// <param name="dirPaths">Paths to directories that should be deleted when the application exits.</param>
        void RegisterTempDirectories(params string[] dirPaths);

        /// <summary>
        ///     Creates and returns the path to a temporary directory that includes the name of the given
        ///     <paramref name="assembly"/> (or the entry assembly if <paramref name="assembly"/> is <c>null</c>)
        ///     and <paramref name="subdirectoryNames"/> in its path.
        ///     The directory will be deleted when the application exits.
        /// </summary>
        /// <param name="assembly">Assembly to include the name of in the generated path.</param>
        /// <param name="subdirectoryNames">Optional subdirectory names to include in the generated path.</param>
        /// <returns>Full, absolute path to a temp directory.</returns>
        /// <exception cref="IOException">
        ///     Path already exists and is a file.
        ///     -or-
        ///     The network name is not known.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Path contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     Path exceeds the system-defined maximum length.  For example, on Windows-based platforms,
        ///     paths must be less than 248 characters and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     Path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     Path contains a colon character (:) that is not part of a drive label ("C:\").
        /// </exception>
        string CreateTempDirectory(Assembly assembly = null, params string[] subdirectoryNames);

        /// <summary>
        ///     Creates and returns the path to a temporary directory that includes the name of the given
        ///     <paramref name="type"/>'s assembly and <paramref name="subdirectoryNames"/> in its path.
        ///     The directory will be deleted when the application exits.
        /// </summary>
        /// <param name="type">Type whose parent assembly's name should be included in the generated path.</param>
        /// <param name="subdirectoryNames">Optional subdirectory names to include in the generated path.</param>
        /// <returns>Full, absolute path to a temp directory.</returns>
        /// <exception cref="IOException">
        ///     Path already exists and is a file.
        ///     -or-
        ///     The network name is not known.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Path contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     Path exceeds the system-defined maximum length.  For example, on Windows-based platforms,
        ///     paths must be less than 248 characters and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     Path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     Path contains a colon character (:) that is not part of a drive label ("C:\").
        /// </exception>
        string CreateTempDirectory(Type type, params string[] subdirectoryNames);

        /// <summary>
        ///     Creates and returns a path to a temporary file.
        ///     If the file already exists, its contents will not be altered.
        ///     The file will be deleted when the application exits.
        /// </summary>
        /// <param name="assembly">Assembly to include the name of in the generated path.</param>
        /// <param name="filename">
        ///     Name of the temporary file.  If none is specified, one will be automatically generated.
        /// </param>
        /// <param name="subdirectoryNames">Optional subdirectory names to include in the generated path.</param>
        /// <returns>Full, absolute path to a temp file.</returns>
        /// <exception cref="IOException">
        ///     An I/O error occurs, such as no unique temporary file name is available.
        ///     -or-
        ///     This method was unable to create a temporary file.
        ///     -or-
        ///     The specified file is in use.
        ///     -or-
        ///     There is an open handle on the file, and the operating system is Windows XP or earlier.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        ///     -or-
        ///     Path is a directory.
        ///     -or-
        ///     Path specified a read-only file.
        /// </exception>
        /// <seealso cref="CreateTempDirectory(System.Reflection.Assembly,string[])"/>
        /// <seealso cref="FileUtils.TouchFile"/>
        string CreateTempFile(Assembly assembly, string filename = null, params string[] subdirectoryNames);

        /// <summary>
        ///     Creates and returns a path to a temporary file.
        ///     If the file already exists, its contents will not be altered.
        ///     The file will be deleted when the application exits.
        /// </summary>
        /// <param name="type">Type whose parent assembly's name should be included in the generated path.</param>
        /// <param name="filename">
        ///     Name of the temporary file.  If none is specified, one will be automatically generated.
        /// </param>
        /// <param name="subdirectoryNames">Optional subdirectory names to include in the generated path.</param>
        /// <returns>Full, absolute path to a temp file.</returns>
        /// <exception cref="IOException">
        ///     An I/O error occurs, such as no unique temporary file name is available.
        ///     -or-
        ///     This method was unable to create a temporary file.
        ///     -or-
        ///     The specified file is in use.
        ///     -or-
        ///     There is an open handle on the file, and the operating system is Windows XP or earlier.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        ///     -or-
        ///     Path is a directory.
        ///     -or-
        ///     Path specified a read-only file.
        /// </exception>
        /// <seealso cref="CreateTempDirectory(System.Type,string[])"/>
        /// <seealso cref="FileUtils.TouchFile"/>
        string CreateTempFile(Type type, string filename = null, params string[] subdirectoryNames);

        /// <summary>
        ///     Attempts to delete the given files, suppressing any exceptions that are thrown.
        /// </summary>
        /// <param name="filePaths">Paths to the files to delete.</param>
        void DeleteTempFiles(params string[] filePaths);

        /// <summary>
        ///     Attempts to delete the given directories and their contents recursively, suppressing any exceptions that are thrown.
        /// </summary>
        /// <param name="dirPaths">Paths to the directories to delete.</param>
        void DeleteTempDirectories(params string[] dirPaths);

        /// <summary>
        ///     Deletes all registered files and directories, suppressing any exceptions that may be thrown.
        /// </summary>
        void DeleteEverything();
    }
}