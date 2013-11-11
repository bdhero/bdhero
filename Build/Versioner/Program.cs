using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BuildUtils;
using DotNetUtils.Extensions;
using Mono.Options;

namespace Versioner
{
    class Program
    {
        private const bool DefaultLimit10 = true;

        private static bool _limit10 = DefaultLimit10;

        private static bool _commitChanges = true;
        private static bool _printCurrentVersion;
        private static bool _printCurrentVersionId;

        private static bool IsPrintAndExit { get { return _printCurrentVersion || _printCurrentVersionId; } }

        static void Main(string[] args)
        {
            var strategy = VersionStrategy.None;
            var custom = "";
            var testVersion = "";

            var optionSet = new OptionSet
                {
                    { "h|?|help", s => PrintUsageAndExit() },
                    { "workspace=", s => Environment.CurrentDirectory = s },
                    { "test", s => _commitChanges = false },
                    { "test-with=|testwith=", s => testVersion = s },
                    { "v|version|p|print", s => _printCurrentVersion = true },
                    { "id|version-id", s => _printCurrentVersionId = true },
                    { "strategy=", s => strategy = VersionStrategyParser.Parse(s) },
                    { "custom=", s => custom = s },
                    { "infinite|no-limit", s => _limit10 = false }
                };

            optionSet.Parse(args);

            var overrideCurrentVersion = !string.IsNullOrWhiteSpace(testVersion);
            if (overrideCurrentVersion)
                _commitChanges = false;

            if (!_commitChanges && !IsPrintAndExit)
                Console.WriteLine("TEST RUN - changes will NOT be written to disk");

            var currentVersion = overrideCurrentVersion ? Version.Parse(testVersion) : CurrentVersion;
            var newVersion = strategy == VersionStrategy.Custom ? Version.Parse(custom) : Bump(currentVersion, strategy);

            if (_printCurrentVersion)
                PrintCurrentVersionAndExit(currentVersion);

            if (_printCurrentVersionId)
                PrintCurrentVersionIdAndExit(currentVersion);

            foreach (var filePath in VersionUtils.Files.Where(File.Exists))
            {
                VersionUtils.SetVersion(filePath, newVersion, _commitChanges);
            }

            Console.WriteLine("{0} => {1}", currentVersion, newVersion);
        }

        private static void PrintUsageAndExit()
        {
            var exeName = Assembly.GetEntryAssembly().GetName().Name;
            var usage = new Usage(exeName).TransformText();
            Console.Error.WriteLine(usage);
            Environment.Exit(0);
        }

        private static void PrintCurrentVersionAndExit(Version version = null)
        {
            Console.Write(version ?? CurrentVersion);
            Environment.Exit(0);
        }

        private static void PrintCurrentVersionIdAndExit(Version version = null)
        {
            Console.Write((version ?? CurrentVersion).GetId());
            Environment.Exit(0);
        }

        static Version CurrentVersion
        {
            get { return VersionUtils.CurrentVersion; }
        }

        static Version Bump(Version version, VersionStrategy strategy)
        {
            switch (strategy)
            {
                case VersionStrategy.BugFix:
                    return BumpBugFix(version);
                case VersionStrategy.MinorFeature:
                    return BumpMinorFeature(version);
                case VersionStrategy.FullRelease:
                    return BumpFullRelease(version);
                case VersionStrategy.MajorMilestone:
                    return BumpMajorMilestone(version);
            }
            return version;
        }

        private static Version BumpBugFix(Version version)
        {
            if (!IsPrintAndExit)
                Console.WriteLine("BumpBugFix({0})", version);
            var revision = version.Revision + 1;
            if (_limit10 && revision > 9)
            {
                revision = 0;
                version = BumpMinorFeature(version);
            }
            return new Version(version.Major, version.Minor, version.Build, revision);
        }

        private static Version BumpMinorFeature(Version version)
        {
            if (!IsPrintAndExit)
                Console.WriteLine("BumpMinorFeature({0})", version);
            var build = version.Build + 1;
            if (_limit10 && build > 9)
            {
                build = 0;
                version = BumpFullRelease(version);
            }
            return new Version(version.Major, version.Minor, build, 0);
        }

        private static Version BumpFullRelease(Version version)
        {
            if (!IsPrintAndExit)
                Console.WriteLine("BumpFullRelease({0})", version);
            var minor = version.Minor + 1;
            if (_limit10 && minor > 9)
            {
                minor = 0;
                version = BumpMajorMilestone(version);
            }
            return new Version(version.Major, minor, 0, 0);
        }

        private static Version BumpMajorMilestone(Version version)
        {
            if (!IsPrintAndExit)
                Console.WriteLine("BumpMajorMilestone({0})", version);
            return new Version(version.Major + 1, 0, 0, 0);
        }
    }

    /*
_._.x._ - Minor feature/enhancement (default)
_._._.x - Bug fix
_.x._._ - Full release
x._._._ - Major milestone
x.x.x.x - Custom
     */
    enum VersionStrategy
    {
        BugFix,
        MinorFeature,
        FullRelease,
        MajorMilestone,
        Custom,
        None
    }

    static class VersionStrategyParser
    {
        public static VersionStrategy Parse(string arg)
        {
            arg = (arg ?? "").Trim();
            Console.WriteLine("arg = {0}", arg);
            if (arg.StartsWith("_._._.x", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.BugFix;
            if (arg.StartsWith("_._.x._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.MinorFeature;
            if (arg.StartsWith("_.x._._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.FullRelease;
            if (arg.StartsWith("x._._._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.MajorMilestone;
            if (arg.StartsWith("x.x.x.x", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.Custom;
            return VersionStrategy.None;
        }
    }
}
