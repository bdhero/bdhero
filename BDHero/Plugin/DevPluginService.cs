using System.IO;
using System.Linq;
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
        public DevPluginService(ILog logger, IKernel kernel, IDirectoryLocator directoryLocator, IPluginRepository repository)
            : base(logger, kernel, directoryLocator, repository)
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
            var curDir = Directory.GetCurrentDirectory();
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