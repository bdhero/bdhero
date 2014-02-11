using System;
using System.IO;
using System.Reflection;
using MarkdownSharp;
using Mono.Options;

namespace Markdowner
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionSet = new OptionSet
                {
                    { "h|?|help", v => ShowHelp() }
                };

            var extraArgs = optionSet.Parse(args);

            if (extraArgs.Count < 1 || extraArgs.Count > 2)
            {
                ShowHelp(1);
            }

            var inputPath = extraArgs[0];
            var outputPath = extraArgs.Count == 2 ? extraArgs[1] : Path.Combine(Path.GetDirectoryName(inputPath), Path.GetFileNameWithoutExtension(inputPath) + ".html");

            var input = File.ReadAllText(inputPath);
            var output = new Markdown().Transform(input);

            output = new Html(Path.GetFileNameWithoutExtension(inputPath), output).TransformText();

            File.WriteAllText(outputPath, output);
        }

        private static void ShowHelp(int exitCode = 0)
        {
            var appName = new FileInfo(Assembly.GetEntryAssembly().Location).Name;
            Console.WriteLine("Usage: {0} INPUT.md [ OUTPUT.html ]", appName);
            Environment.Exit(exitCode);
        }
    }
}
