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
        ///     Registers the given <paramref name="filePath"/> for deletion when the application exits.
        /// </summary>
        /// <param name="filePath">Path to a file that should be deleted when the application exits.</param>
        void RegisterTempFile(string filePath);

        /// <summary>
        ///     Registers the given <paramref name="dirPath"/> for deletion when the application exits.
        /// </summary>
        /// <param name="dirPath">Path to a directory that should be deleted when the application exits.</param>
        void RegisterTempDirectory(string dirPath);

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
        ///     Attempts to delete the given file, suppressing any exceptions that are thrown.
        /// </summary>
        /// <param name="filePath">Path to the file to delete.</param>
        void DeleteTempFile(string filePath);

        /// <summary>
        ///     Attempts to delete the given directory and its contents recursively, suppressing any exceptions that are thrown.
        /// </summary>
        /// <param name="dirPath">Path to the directory to delete.</param>
        void DeleteTempDirectory(string dirPath);

        /// <summary>
        ///     Deletes all registered files and directories, suppressing any exceptions that may be thrown.
        /// </summary>
        void DeleteEverything();
    }
}