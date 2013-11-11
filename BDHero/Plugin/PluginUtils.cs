using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BDHero.Plugin
{
    public static class PluginUtils
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static T GetPreferences<T>(PluginAssemblyInfo assemblyInfo, TypeFactory<T> defaultFactory)
        {
            if (File.Exists(assemblyInfo.ConfigFilePath))
            {
                try
                {
                    var json = File.ReadAllText(assemblyInfo.ConfigFilePath);
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception e)
                {
                    Logger.WarnFormat("Unable to deserialize settings file: {0}", e);
                }
            }
            return defaultFactory();
        }

        public static void SavePreferences(PluginAssemblyInfo assemblyInfo, Object prefs)
        {
            var json = JsonConvert.SerializeObject(prefs, Formatting.Indented);
            var directory = Path.GetDirectoryName(assemblyInfo.ConfigFilePath);
            if (directory != null)
                Directory.CreateDirectory(directory);
            File.WriteAllText(assemblyInfo.ConfigFilePath, json);
        }
    }

    public delegate T TypeFactory<T>();
}
