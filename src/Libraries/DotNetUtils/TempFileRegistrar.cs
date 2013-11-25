using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils.Annotations;
using DotNetUtils.Crypto;
using DotNetUtils.Extensions;

namespace DotNetUtils
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

    /// <summary>
    ///     Concrete implementation of the <seealso cref="ITempFileRegistrar"/> interface.
    ///     <pre>
    ///         %TEMP%/{ProductName}/{EntryAssemblyPathSHA1Hash[0,7]}/{Process.ID}/{CallingAssembly.Name}/{subdirectoryNames[0]}/{subdirectoryNames[1]}/{...}
    ///     </pre>
    /// </summary>
    /// <example>
    ///     <pre>
    ///         C:\Documents and Settings\Emilio Estevez\Application Data\Local\Temp\BDHero\a0ec92f\5770\FFmpegMuxerPlugin\Metadata\Chapters\chapters.xml
    ///     </pre>
    /// </example>
    [UsedImplicitly]
    public class TempFileRegistrar : ITempFileRegistrar
    {
        private volatile bool _isDisposed;

        private readonly ISet<string> _tempFilePaths = new HashSet<string>();
        private readonly ISet<string> _tempDirPaths = new HashSet<string>();

        #region IDisposable Members

        ~TempFileRegistrar()
        {
            Dispose(false); // I am *not* calling you from Dispose, it's *not* safe to free managed resources
        }

        /// <summary>
        ///     Frees managed and unmanaged resources.
        /// </summary>
        /// <param name="freeManagedObjectsAlso">
        ///     Free managed resources.  Should only be set to <c>true</c> when called from <see cref="Dispose"/>.
        /// </param>
        /// <seealso cref="http://stackoverflow.com/a/538238/467582"/>
        private void Dispose(bool freeManagedObjectsAlso)
        {
            // Free unmanaged resources
            // ...

            // Free managed resources too, but only if I'm being called from Dispose()
            // (If I'm being called from Finalize then the objects might not exist anymore)
            if (freeManagedObjectsAlso)
            {
                if (_isDisposed) { return; }
                DeleteEverything();
                _isDisposed = true;
            }
        }

        /// <seealso cref="http://stackoverflow.com/a/538238/467582"/>
        public void Dispose()
        {
            Dispose(true); // I am calling you from Dispose, it's safe
            GC.SuppressFinalize(this); // Hey, GC: don't bother calling finalize later
        }

        #endregion

        #region Registration

        public void RegisterTempFile(string filePath)
        {
            lock (this)
            {
                _tempFilePaths.Add(filePath);
            }
        }

        public void RegisterTempDirectory(string dirPath)
        {
            lock (this)
            {
                _tempDirPaths.Add(dirPath);
            }
        }

        #endregion

        #region Deletion

        public void DeleteEverything()
        {
            lock (this)
            {
                var tempFilePaths = _tempFilePaths.ToArray();
                var tempDirPaths = _tempDirPaths.ToArray();

                // Get the parent directories of all explicitly registered files and directories
                var fileParentDirs = tempFilePaths.Select(Path.GetDirectoryName).ToArray();
                var dirParentDirs = tempDirPaths.Select(Path.GetDirectoryName).ToArray();

                // Delete files and directories that were explicitly registered
                tempFilePaths.ForEach(DeleteTempFileNonLocking);
                tempDirPaths.ForEach(DeleteTempDirectoryNonLocking);

                // Now that all explicitly registered files and directories have been deleted,
                // check if their parent directories are empty and, if so, delete them.
                fileParentDirs.ForEach(DeleteDirectoryTree);
                dirParentDirs.ForEach(DeleteDirectoryTree);

                DeleteRootTempDirectory();
            }
        }

        public void DeleteTempFile(string filePath)
        {
            lock (this)
            {
                DeleteTempFileNonLocking(filePath);
            }
        }

        private void DeleteTempFileNonLocking(string filePath)
        {
            Try(delegate
                {
                    File.Delete(filePath);
                    _tempFilePaths.Remove(filePath);
                    DeleteDirectoryTree(Path.GetDirectoryName(filePath));
                });
        }

        public void DeleteTempDirectory(string dirPath)
        {
            lock (this)
            {
                DeleteTempDirectoryNonLocking(dirPath);
            }
        }

        private void DeleteTempDirectoryNonLocking(string dirPath)
        {
            Try(delegate
                {
                    Directory.Delete(dirPath, true);
                    _tempDirPaths.Remove(dirPath);
                    DeleteDirectoryTree(Path.GetDirectoryName(dirPath));
                });
        }

        private static void DeleteDirectoryTree(string dirPath)
        {
            if (string.IsNullOrWhiteSpace(dirPath)) { return; }
            if (IsTempDir(dirPath)) { return; }

            Try(delegate
                {
                    var dir = new DirectoryInfo(dirPath);
                    if (!Directory.Exists(dirPath)) { return; }
                    if (dir.GetFiles().Any()) { return; }
                    if (dir.GetDirectories().Any()) { return; }
                    Directory.Delete(dirPath, true);
                });

            Try(() => DeleteDirectoryTree(Path.GetDirectoryName(dirPath)));
        }

        private static void DeleteRootTempDirectory()
        {
            Try(delegate
                {
                    var rootPath = RootTempDirectory;

                    if (!Directory.Exists(rootPath)) { return; }
                    if (new DirectoryInfo(rootPath).GetFiles("*.*", SearchOption.AllDirectories).Any()) { return; }

                    Directory.Delete(rootPath, true);
                });
        }

        #endregion

        #region Directory creation

        public string CreateTempDirectory(Assembly assembly = null, params string[] subdirectoryNames)
        {
            var pathHash = new SHA1Algorithm().ComputeText(AssemblyUtils.GetInstallDir());
            var folderNames = new List<string>
                              {
                                  RootTempDirectory,
                                  pathHash.Substring(0, 7),
                                  AssemblyUtils.GetAssemblyName(assembly),
                                  Process.GetCurrentProcess().Id.ToString("0")
                              };
            folderNames.AddRange(subdirectoryNames.Where(s => !string.IsNullOrWhiteSpace(s)));
            var path = Combine(folderNames.ToArray());
            Directory.CreateDirectory(path);
            RegisterTempDirectory(path);
            return path;
        }

        public string CreateTempDirectory(Type type, params string[] subdirectoryNames)
        {
            return CreateTempDirectory(Assembly.GetAssembly(type), subdirectoryNames);
        }

        #endregion

        #region File creation

        public string CreateTempFile(Assembly assembly, string filename = null, params string[] subdirectoryNames)
        {
            if (filename == null)
            {
                var tempFilePath = Path.GetTempFileName();
                filename = Path.GetFileName(tempFilePath);
                File.Delete(tempFilePath);
            }
            var path = Combine(CreateTempDirectory(assembly, subdirectoryNames), filename);
            FileUtils.TouchFile(path);
            RegisterTempFile(path);
            return path;
        }

        public string CreateTempFile(Type type, string filename = null, params string[] subdirectoryNames)
        {
            return CreateTempFile(Assembly.GetAssembly(type), filename, subdirectoryNames);
        }

        #endregion

        #region Private utilities

        /// <summary>
        ///     Gets the root temp directory.  This is a subdirectory within the system's temp directory whose name
        ///     is that of the application's product name (read from the entry assembly).
        /// </summary>
        private static string RootTempDirectory
        {
            get { return Combine(Path.GetTempPath(), AppUtils.ProductName); }
        }

        private static string Combine(params string[] pathParts)
        {
            pathParts = pathParts.Select(NormalizeDirectoryPath).ToArray();

            if (pathParts.Length == 1)
            {
                return pathParts.First();
            }

            // Don't sanitize the first path part (probably contains a valid path, such as C:\some\dir)
            var validPathParts = pathParts.Skip(1)
                                          .Where(pathPart => !string.IsNullOrWhiteSpace(pathPart))
                                          .SelectMany(s => s.Split(Path.DirectorySeparatorChar))
                                          .Select(FileUtils.SanitizeFileName)
                                          .Where(pathPart => !string.IsNullOrWhiteSpace(pathPart))
                                          .ToArray();

            var allPathParts = new List<string>();
            allPathParts.Add(pathParts.First());
            allPathParts.AddRange(validPathParts);

            return Path.Combine(allPathParts.ToArray());
        }

        private static string NormalizeDirectorySeparators(string path)
        {
            return new Regex(@"[/\\]+").Replace(path, Path.DirectorySeparatorChar + "");
        }

        /// <summary>
        ///     Removes trailing slashes and normalizes directory separator characters to the system's preferred character.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string NormalizeDirectoryPath(string path)
        {
            var regex = new Regex(string.Format("{0}+$", Regex.Escape(Path.DirectorySeparatorChar + "")));
            var normalized = regex.Replace(NormalizeDirectorySeparators(path), "");
            return normalized;
        }

        /// <summary>
        ///     Determines whether the given <paramref name="dirPath"/> is the system's temp directory.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        /// <remarks>
        ///     This method will fail on symbolic links.  E.G., on Mac OS X, /tmp is symlinked to /private/tmp.
        /// </remarks>
        private static bool IsTempDir(string dirPath)
        {
            dirPath = NormalizeDirectoryPath(dirPath);
            var tempPath = NormalizeDirectoryPath(Path.GetTempPath());
            return String.Equals(dirPath, tempPath, StringComparison.InvariantCultureIgnoreCase);
        }

        private static void Try(Action action)
        {
            try { action(); }
            catch { }
        }

        #endregion
    }
}
