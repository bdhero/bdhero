using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restorer
{
    class Program
    {
        static void Main(string[] args)
        {
            var curDir = new DirectoryInfo(Environment.CurrentDirectory);
            var isSolutionDir = false;

            while (curDir != null && !(isSolutionDir = curDir.GetFiles("*.sln").Any()))
            {
                curDir = curDir.Parent;
            }

            if (curDir == null || !isSolutionDir)
            {
                Console.Error.WriteLine("ERROR: Unable to find solution directory!");
                Environment.Exit(1);
            }

            var solutionDir = curDir;
            var packages = Package.GetPackagesWithContent(solutionDir);
            var projects = args.Any() ? Project.GetProjects(args) : Project.GetProjects(solutionDir);

            Console.WriteLine("Projects: {0}", string.Join(", ", projects));

            foreach (var package in packages)
            {
                foreach (var project in projects.Where(package.IsReferencedBy))
                {
                    package.CopyContentTo(project);
                }
            }
        }
    }

    [DebuggerDisplay("ProjectDirectory = {ProjectDirectory}")]
    class Project
    {
        [NotNull]
        public readonly DirectoryInfo ProjectDirectory;

        private Project(DirectoryInfo projectDirectory)
        {
            ProjectDirectory = projectDirectory;
        }

        public string PackagesConfig
        {
            get { return File.ReadAllText(Path.Combine(ProjectDirectory.FullName, "packages.config")); }
        }

        public override string ToString()
        {
            return string.Format("ProjectDirectory: {0}", ProjectDirectory);
        }

        public static List<Project> GetProjects(string[] projectPaths)
        {
            var projectDirs = projectPaths.Select(projectPath => new DirectoryInfo(projectPath)).ToList();
            return GetProjects(projectDirs);
        }

        public static List<Project> GetProjects(DirectoryInfo solutionDir)
        {
            var allDirs = solutionDir.GetDirectories("*", SearchOption.AllDirectories).ToList();
            return GetProjects(allDirs);
        }

        private static List<Project> GetProjects(List<DirectoryInfo> directories)
        {
            var projects = directories.Where(IsProjectDirectory)
                                      .Select(CreateProject)
                                      .ToList();
            return projects;
        }

        private static bool IsProjectDirectory(DirectoryInfo dir)
        {
            var csprojFiles = dir.GetFiles("*.csproj").ToArray();
            var packagesConfigFiles = dir.GetFiles("packages.config").ToArray();
            return csprojFiles.Any() && packagesConfigFiles.Any();
        }

        private static Project CreateProject(DirectoryInfo projectDir)
        {
            return new Project(projectDir);
        }
    }

    [DebuggerDisplay("PackageDirectory = {PackageDirectory}")]
    class Package
    {
        /// <summary>
        ///     Matches a NuGet package directory name.
        /// </summary>
        /// <example>
        ///     <code>"BDHero.BuildTools.0.8.7.2" -> { "id": "BDHero.BuildTools", "version": "0.8.7.2" }</code>
        ///     <code>"BDHero.BuildTools.0.8.7.2-alpha23" -> { "id": "BDHero.BuildTools", "version": "0.8.7.2-alpha23" }</code>
        ///     <code>"BDHero.BuildTools.0.8.7.2-alpha2.3" -> NO MATCH</code>
        /// </example>
        private static readonly Regex NuPkgRegex = new Regex(@"^(?<id>[\w.-]+[a-zA-Z])\.(?<version>\d+(?:\.\d+){0,3}(?:-\w+)?)", RegexOptions.Multiline);

        /// <summary>
        ///     Matches a NuGet package listing found in a project's <c>packages.config</c> file.
        /// </summary>
        private static readonly Regex PackageRegex = new Regex("<package id=\"(?<id>[^\"]+)\" version=\"(?<version>[^\"]+)\"[^>]*?/>", RegexOptions.Multiline);

        /// <summary>
        ///     /packages/[ID].[VERSION]
        /// </summary>
        [NotNull]
        public readonly DirectoryInfo PackageDirectory;

        /// <summary>
        ///     /packages/[ID].[VERSION]/content
        /// </summary>
        [CanBeNull]
        public readonly DirectoryInfo ContentDirectory;

        public readonly string Id;

        public readonly string Version;

        private Package(DirectoryInfo packageDirectory, string id, string version)
        {
            PackageDirectory = packageDirectory;
            ContentDirectory = PackageDirectory.GetDirectories("content").FirstOrDefault();
            Id = id;
            Version = version;
        }

        public bool IsReferencedBy(Project project)
        {
            var projectPackagesConfig = project.PackagesConfig;
            var allPackageMatches = PackageRegex.Matches(projectPackagesConfig).OfType<Match>();
            var thisPackageMatches =
                allPackageMatches.Where(match => match.Value("id") == Id && match.Value("version") == Version)
                                 .ToList();
            return thisPackageMatches.Any();
        }

        public void CopyContentTo(Project project)
        {
            var sourcePath = ContentDirectory.FullName;
            var destinationPath = project.ProjectDirectory.FullName;

            GetNewPathDelegate getNewPath = fsi => destinationPath + fsi.FullName.Substring(sourcePath.Length);

            // Now Create all of the directories
            foreach (var sourceDir in ContentDirectory.GetDirectories("*", SearchOption.AllDirectories))
                Try(sourceDir, getNewPath, CreateDirectory);

            // Copy all the files
            foreach (var sourceFile in ContentDirectory.GetFiles("*.*", SearchOption.AllDirectories))
                Try(sourceFile, getNewPath, File.Copy);
        }

        public override string ToString()
        {
            return string.Format("PackageDirectory: {0}", PackageDirectory);
        }

        public static List<Package> GetPackagesWithContent(DirectoryInfo solutionDir)
        {
            var packagesDir = solutionDir.GetDirectories("packages").First();
            var packageDirs = packagesDir.GetDirectories();
            var packages = packageDirs.Where(IsPackageDirectoryWithContent).Select(CreatePackage).ToList();
            return packages;
        }

        private static void CreateDirectory(string srcPath, string destPath)
        {
            Directory.CreateDirectory(destPath);
        }

        private static void Try(FileSystemInfo source, GetNewPathDelegate destPathGetter, FileSystemAction action)
        {
            var destPath = destPathGetter(source);

            if (File.Exists(destPath) || Directory.Exists(destPath))
            {
                Console.WriteLine("{0} already exists, skipping", destPath);
                return;
            }

            Console.WriteLine("{0} => {1}", source, destPath);

            try
            {
                action(source.FullName, destPath);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        private static bool IsPackageDirectoryWithContent(DirectoryInfo packageDir)
        {
            return packageDir.GetFiles("*.nupkg").Any() &&
                   packageDir.GetDirectories("content").Any() &&
                   packageDir.GetDirectories("content").First().GetFiles("*.*", SearchOption.AllDirectories)
                             .Any(file => !file.Name.EndsWith(".pp", StringComparison.OrdinalIgnoreCase));
        }

        private static Package CreatePackage(DirectoryInfo packageDir)
        {
            var match = NuPkgRegex.Match(packageDir.Name);

            if (!match.Success)
            {
                Console.Error.WriteLine("Unable to find NuGet package ID and Version in \"{0}\" directory; skipping", packageDir.FullName);
                return null;
            }

            return new Package(packageDir, match.Value("id"), match.Value("version"));
        }
    }

    internal static class RegexExtensions
    {
        public static string Value(this Match match, string groupName)
        {
            return match.Groups[groupName].Value;
        }
    }

    internal delegate string GetNewPathDelegate(FileSystemInfo source);
    internal delegate void FileSystemAction(string sourcePath, string destinationPath);

    /// <summary>
    /// Indicates that the value of the marked element could be <c>null</c> sometimes, 
    /// so the check for <c>null</c> is necessary before its usage.
    /// </summary>
    /// <example>
    /// <code>
    /// [CanBeNull]
    /// public object Test()
    /// {
    ///   return null;
    /// }
    /// 
    /// public void UseTest()
    /// {
    ///   var p = Test(); 
    ///   var s = p.ToString(); // Warning: Possible 'System.NullReferenceException' 
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class CanBeNullAttribute : Attribute { }

    /// <summary>
    /// Indicates that the value of the marked element could never be <c>null</c>
    /// </summary>
    /// <example>
    /// <code>
    /// [NotNull]
    /// public object Foo()
    /// {
    ///   return null; // Warning: Possible 'null' assignment
    /// } 
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class NotNullAttribute : Attribute { }
}
