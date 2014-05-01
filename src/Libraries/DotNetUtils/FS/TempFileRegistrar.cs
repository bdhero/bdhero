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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotNetUtils.Annotations;
using DotNetUtils.Crypto;
using DotNetUtils.Extensions;

namespace DotNetUtils.FS
{
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
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                Logger.Info("TempFileRegistrar.Dispose()");
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

        public void RegisterTempFiles(params string[] filePaths)
        {
            lock (this)
            {
                _tempFilePaths.AddRange(filePaths);
            }
        }

        public void RegisterTempDirectories(params string[] dirPaths)
        {
            lock (this)
            {
                _tempDirPaths.AddRange(dirPaths);
            }
        }

        #endregion

        #region Deletion

        public void DeleteEverything()
        {
            Logger.Info("TempFileRegistrar.DeleteEverything()");
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

        public void DeleteTempFiles(params string[] filePaths)
        {
            lock (this)
            {
                filePaths.ForEach(DeleteTempFileNonLocking);
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

        public void DeleteTempDirectories(params string[] dirPaths)
        {
            lock (this)
            {
                dirPaths.ForEach(DeleteTempDirectoryNonLocking);
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
            RegisterTempDirectories(path);
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
            RegisterTempFiles(path);
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
