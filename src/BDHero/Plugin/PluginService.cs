﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BDHero.Startup;
using DotNetUtils;
using DotNetUtils.Annotations;
using log4net;
using Ninject;

namespace BDHero.Plugin
{
    /// <summary>
    ///     Default implementation of <see cref="IPluginService"/>.
    /// </summary>
    internal class PluginService : IPluginService
    {
        protected readonly ILog Logger;

        private readonly IKernel _kernel;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly IPluginRepository _repository;

        [UsedImplicitly]
        public PluginService(ILog logger, IKernel kernel, IDirectoryLocator directoryLocator, IPluginRepository repository)
        {
            Logger = logger;

            _kernel = kernel;
            _directoryLocator = directoryLocator;
            _repository = repository;
        }

        public virtual void LoadPlugins(string path)
        {
            AddPluginsRecursive(path);
        }

        public virtual void UnloadPlugins()
        {
            foreach (var plugin in _repository.PluginsByType)
            {
                // Close all plugin instances
                // We call the plugins Dispose sub first incase it has to do
                // Its own cleanup stuff
                plugin.UnloadPlugin();
            }

            // Finally, clear our collection of available plugins
            _repository.Clear();
        }

        /// <summary>
        /// Searches the given directory and its subdirectories recursively for Plugins
        /// </summary>
        /// <param name="pluginDir">Root directory to search for Plugins in</param>
        protected void AddPluginsRecursive(string pluginDir)
        {
            if (!Directory.Exists(pluginDir))
                return;

            // Go through all the files in the plugin directory
            foreach (FileInfo file in Directory.GetFiles(pluginDir).Select(filePath => new FileInfo(filePath)).Where(IsPlugin))
            {
                AddPlugin(file.FullName);
            }

            foreach (string dir in Directory.GetDirectories(pluginDir))
            {
                AddPluginsRecursive(dir);
            }
        }

        private static bool IsPlugin(FileInfo file)
        {
            return file.Name.EndsWith("Plugin.dll", StringComparison.OrdinalIgnoreCase);
        }

        private void AddPlugin(string dllPath)
        {
            // Create a new assembly from the plugin file we're adding..
            Assembly pluginAssembly = Assembly.LoadFrom(dllPath);

            var guid = AssemblyUtils.Guid(pluginAssembly);

            var machineName = Path.GetFileNameWithoutExtension(dllPath) ?? pluginAssembly.GetName().Name ?? "";
            machineName = Regex.Replace(machineName, "Plugin$", "", RegexOptions.IgnoreCase);
            var configFileName = machineName + ".config.json";
            var configFilePath = Path.Combine(_directoryLocator.PluginConfigDir, machineName, configFileName);

            // Next we'll loop through all the Types found in the assembly
            foreach (Type pluginType in pluginAssembly.GetTypes().Where(IsValidPlugin))
            {
                // Create a new instance and store the instance in the collection for later use
                // We could change this later on to not load an instance.. we have 2 options
                // 1- Make one instance, and use it whenever we need it.. it's always there
                // 2- Don't make an instance, and instead make an instance whenever we use it, then close it
                // For now we'll just make an instance of all the plugins
                var newPlugin = (IPlugin) _kernel.Get(pluginType);

                // TODO: Store this in preferences file
                newPlugin.Enabled = true;

                var assemblyInfo = new PluginAssemblyInfo(dllPath,
                                                          AssemblyUtils.GetAssemblyVersion(pluginAssembly),
                                                          AssemblyUtils.GetLinkerTimestamp(pluginAssembly),
                                                          guid,
                                                          configFilePath);

                // Initialize the plugin
                newPlugin.LoadPlugin(_repository, assemblyInfo);

                // Add the new plugin to our collection here
                _repository.Add(newPlugin);
            }
        }

        private static bool IsValidPlugin(Type pluginType)
        {
            return HasPublicVisibility(pluginType)
                && IsConcreteClass(pluginType)
                && ImplementsPluginInterface(pluginType)
                ;
        }

        private static bool HasPublicVisibility(Type pluginType)
        {
            return pluginType.IsPublic;
        }

        private static bool IsConcreteClass(Type pluginType)
        {
            return !pluginType.IsAbstract && !pluginType.IsInterface;
        }

        private static bool ImplementsPluginInterface(Type pluginType)
        {
            // Gets a type object of the interface we need the plugins to match
            Type typeInterface = pluginType.GetInterface(typeof(IPlugin).FullName);

            // Make sure the interface we want to use actually exists
            return typeInterface != null;
        }
    }
}
