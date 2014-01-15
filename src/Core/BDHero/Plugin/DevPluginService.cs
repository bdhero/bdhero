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

using System.IO;
using System.Linq;
using BDHero.Prefs;
using BDHero.Startup;
using DotNetUtils;
using DotNetUtils.Annotations;
using log4net;
using Ninject;

namespace BDHero.Plugin
{
    /// <summary>
    ///     Subclass of <see cref="PluginService"/> used for local testing and development
    ///     in Visual Studio / Xamarin Studio.
    /// </summary>
    internal class DevPluginService : PluginService
    {
        private bool _loaded;

        [UsedImplicitly]
        public DevPluginService(ILog logger, IKernel kernel, IDirectoryLocator directoryLocator, IPreferenceManager preferenceManager, IPluginRepository repository)
            : base(logger, kernel, directoryLocator, preferenceManager, repository)
        {
        }

        public static bool IsDevMode
        {
            get
            {
                var isVisualStudio = HasBinFiles("*.vshost.exe");
                var isXamarinMono = HasBinFiles("*.mdb");
                return isVisualStudio || isXamarinMono;
            }
        }

        private static bool HasBinFiles(string searchPattern)
        {
            var installDir = AssemblyUtils.GetInstallDir();
            var debugFiles = Directory.GetFiles(installDir, searchPattern, SearchOption.TopDirectoryOnly).ToArray();
            return debugFiles.Any();
        }

        public override void LoadPlugins(string path)
        {
            if (_loaded) { return; }
            LoadDevPlugins();
            _loaded = true;
        }

        public override void UnloadPlugins()
        {
            base.UnloadPlugins();
            _loaded = false;
        }

        private void LoadDevPlugins()
        {
            var solutionDir = GetSolutionDirPath();
            var projects = new[]
                           {
                               "AutoDetectorPlugin", "ChapterGrabberPlugin", "ChapterWriterPlugin", "DiscReaderPlugin",
                               "FFmpegMuxerPlugin", "FileNamerPlugin", "MKVMergeMuxerPlugin", "IsanPlugin", "TmdbPlugin"
                           };
            foreach (var projectName in projects)
            {
                try
                {
                    var pluginDir = Path.Combine(solutionDir, "Plugins", projectName, "bin", "Debug");
                    AddPluginsRecursive(pluginDir);
                }
                catch
                {
                }
            }
        }

        private static string GetSolutionDirPath()
        {
            var curDir = AssemblyUtils.GetInstallDir();
            DirectoryInfo parent;
            while (!SolutionFileExists(curDir) && (parent = new DirectoryInfo(curDir).Parent) != null)
            {
                curDir = parent.FullName;
            }
            return SolutionFileExists(curDir) ? curDir : @"C:\Projects\bdhero";
        }

        private static bool SolutionFileExists(string dirPath)
        {
            return File.Exists(Path.Combine(dirPath, "BDHero.sln"));
        }
    }
}