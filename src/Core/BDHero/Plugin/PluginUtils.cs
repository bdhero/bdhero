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
