using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils.Extensions;

namespace BuildUtils
{
    public class VersionUtils
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string InnoSetupScriptPath = @"Build\InnoSetup\setup.iss";
        private const string BDHeroAssemblyInfoPath = @"BDHero\Properties\AssemblyInfo.cs";
        private const string BDHeroCLIAssemblyInfoPath = @"BDHeroCLI\Properties\AssemblyInfo.cs";
        private const string BDHeroGUIAssemblyInfoPath = @"BDHeroGUI\Properties\AssemblyInfo.cs";

        /*
[assembly: AssemblyVersion("0.7.5.7")]
[assembly: AssemblyFileVersion("0.7.5.7")]
         */
        static readonly Regex AssemblyRegex = new Regex(@"^(\[assembly: Assembly(?:File)?Version\(.)((?:\d+\.){3}\d+)(.\)\])", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        static readonly Regex InnoSetupVersionRegex = new Regex(@"(#define MyAppVersion .)((?:\d+\.){3}\d+)(.)", RegexOptions.IgnoreCase);
        static readonly Regex ArtifactFileNameRegex = new Regex(@"(<filename>\w+-)([\d.]+)(-(?:(?:windows|mac|linux)-)?(?:installer|setup|portable).(?:exe|zip|run|bin|tgz|dmg)+</filename>)", RegexOptions.IgnoreCase);

        public static readonly string[] Files = new[]
            {
                InnoSetupScriptPath,
                BDHeroAssemblyInfoPath,
                BDHeroCLIAssemblyInfoPath,
                BDHeroGUIAssemblyInfoPath
            };

        public static Version CurrentVersion
        {
            get
            {
                var assemblyInfo = File.ReadAllText(BDHeroAssemblyInfoPath);
                var match = AssemblyRegex.Match(assemblyInfo);
                return Version.Parse(match.Groups[2].Value);
            }
        }

        public static void SetVersion(string filePath, Version newVersion, bool writeToDisk)
        {
            var file = ReadFile(filePath);
            var contents = file.Key;
            var encoding = file.Value;

            Logger.InfoFormat("File \"{0}\" has encoding {1}", filePath, encoding.EncodingName);

            contents = AssemblyRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = InnoSetupVersionRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = ArtifactFileNameRegex.Replace(contents, "${1}" + newVersion + "${3}");

            if (writeToDisk)
            {
                File.WriteAllText(filePath, contents, encoding);
            }
        }

        private static KeyValuePair<string, Encoding> ReadFile(string filePath)
        {
            // open the file with the stream-reader:
            using (var reader = new StreamReader(filePath, true))
            {
                // read the contents of the file into a string
                var contents = reader.ReadToEnd();

                // return the encoding.
                return new KeyValuePair<string, Encoding>(contents, reader.CurrentEncoding);
            }
        }
    }
}
