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
using System.IO;
using System.Linq;
using System.Threading;
using BDHero;
using BDHero.Plugin;
using BDHero.Startup;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using Mono.Options;
using ProcessUtils;

namespace BDHeroCLI
{
    [UsedImplicitly]
    class CLI
    {
        private static string ExeFileName
        {
            get { return Path.GetFileName(AssemblyUtils.AssemblyOrDefault().Location); }
        }

        private readonly log4net.ILog _logger;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly PluginLoader _pluginLoader;
        private readonly IController _controller;

        private bool? _replaceExistingFiles;
        private string _bdromPath;
        private string _mkvPath;

        public CLI(IDirectoryLocator directoryLocator, PluginLoader pluginLoader, IController controller)
        {
            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _directoryLocator = directoryLocator;
            _pluginLoader = pluginLoader;
            _controller = controller;
        }

        public void Run(IEnumerable<string> args)
        {
            ShowVersion();
            ParseArgs(args);
            LogDirectoryPaths();
            LoadPlugins();
            LogPlugins();
            PromptForPaths();
            InitController();
            ExecuteStages();
            PromptToExit();
        }

        private void ParseArgs(IEnumerable<string> args)
        {
            var loggerRepository = _logger.Logger.Repository;

            var optionSet = new OptionSet
                {
                    { "h|?|help", v => ShowHelp() },
                    { "V|version", s => Environment.Exit(0) },
                    { "debug", s => loggerRepository.Threshold = log4net.Core.Level.Debug },
                    { "v|verbose", s => loggerRepository.Threshold = log4net.Core.Level.Info },
                    { "w|warn", s => loggerRepository.Threshold = log4net.Core.Level.Warn },
                    { "q|quiet", s => loggerRepository.Threshold = log4net.Core.Level.Error },
                    { "s|silent", s => loggerRepository.Threshold = log4net.Core.Level.Fatal },
                    { "i=|input=", s => _bdromPath = s },
                    { "o=|output=", s => _mkvPath = s },
                    { "y|yes", s => _replaceExistingFiles = true },
                    { "n|no", s => _replaceExistingFiles = false }
                };

            var extraArgs = optionSet.Parse(args);

            if (extraArgs.Any())
            {
                _logger.WarnFormat("Unknown argument{0}: {1}", extraArgs.Count == 1 ? "" : "s", new ArgumentList(extraArgs));
            }
        }

        private void LogDirectoryPaths()
        {
            _logger.InfoFormat("IsPortable = {0}", _directoryLocator.IsPortable);
            _logger.InfoFormat("InstallDir = {0}", _directoryLocator.InstallDir);
            _logger.InfoFormat("AppConfigDir = {0}", _directoryLocator.AppConfigDir);
            _logger.InfoFormat("PluginConfigDir = {0}", _directoryLocator.PluginConfigDir);
            _logger.InfoFormat("RequiredPluginDir = {0}", _directoryLocator.RequiredPluginDir);
            _logger.InfoFormat("CustomPluginDir = {0}", _directoryLocator.CustomPluginDir);
            _logger.InfoFormat("LogDir = {0}", _directoryLocator.LogDir);
        }

        private void LoadPlugins()
        {
            _pluginLoader.LoadPlugins();
        }

        private void LogPlugins()
        {
            _pluginLoader.LogPlugins();
        }

        private void PromptForPaths()
        {
            // Prompt user for path to BD-ROM directory
            while (string.IsNullOrWhiteSpace(_bdromPath) || !Directory.Exists(_bdromPath))
            {
                Console.Write("Source BD-ROM path: ");
                _bdromPath = Console.ReadLine();
            }

            // Prompt user for path to output MKV file
            while (string.IsNullOrWhiteSpace(_mkvPath))
            {
                Console.Write("Output MKV path: ");
                _mkvPath = Console.ReadLine();
            }
        }

        private void InitController()
        {
            _controller.ScanStarted += ControllerOnScanStarted;
            _controller.ScanSucceeded += ControllerOnScanSucceeded;
            _controller.PluginProgressUpdated += ControllerOnPluginProgressUpdated;
            _controller.SetEventScheduler();
        }

        private void ExecuteStages()
        {
            if (Scan(_bdromPath, _mkvPath))
            {
                if (Convert())
                {
                    _logger.Info("Muxing succeeded!");
                }
                else
                {
                    _logger.Error("Muxing failed!");
                }
            }
            else
            {
                _logger.Error("Scanning failed!");
            }
        }

        private bool Scan(string bdromPath, string mkvPath)
        {
            var cancellationToken = new CancellationToken();
            var scanTask = _controller.CreateScanTask(cancellationToken, bdromPath, mkvPath);
            scanTask.Start();
            return scanTask.Result;
        }

        private bool Convert()
        {
            var cancellationToken = new CancellationToken();
            var convertTask = _controller.CreateConvertTask(cancellationToken);
            convertTask.Start();
            return convertTask.Result;
        }

        private static void ShowHelp(int exitCode = 0)
        {
            var exeName = ExeFileName;
            var usage = new Usage(exeName).TransformText();
            Console.Error.WriteLine(usage);
            Environment.Exit(exitCode);
        }

        private static void ShowVersion()
        {
            Console.Error.WriteLine("{0} v{1} - compiled {2}", AssemblyUtils.GetAssemblyName(), AssemblyUtils.GetAssemblyVersion(), AssemblyUtils.GetLinkerTimestamp());
        }

        private static void ControllerOnScanStarted()
        {
            Console.WriteLine();
        }

        private static void ControllerOnScanSucceeded()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        private static void ControllerOnPluginProgressUpdated(IPlugin plugin, ProgressProvider progressProvider)
        {
            var line = string.Format("{0} is {1} - {2} complete - {3} - {4} elapsed, {5} remaining",
                                     plugin.Name, progressProvider.State, (progressProvider.PercentComplete / 100.0).ToString("P"),
                                     progressProvider.Status,
                                     progressProvider.RunTime.ToStringShort(),
                                     progressProvider.TimeRemaining.ToStringShort());
            if (progressProvider.State == ProgressProviderState.Running)
            {
                Console.WriteLine("\r{0}", line);
            }
            else
            {
                Console.WriteLine(line);
            }
        }

        private void PromptToExit()
        {
            Console.WriteLine();
            Console.WriteLine("*** BDHero CLI Finished - press <ENTER> to exit ***");
            Console.WriteLine();
            Console.Read();
        }
    }
}
