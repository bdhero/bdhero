// Copyright 2014 Andrew C. Dvorak
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
