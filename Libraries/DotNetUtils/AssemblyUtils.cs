using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetUtils
{
    /// <summary>
    /// Collection of helpful utilities for working with Assemblies (name, version, install path)
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// Returns
        /// <paramref name="assembly"/> if it is not <c>null</c>, otherwise
        /// <see cref="Assembly.GetEntryAssembly()"/> if it is not <c>null</c>, or
        /// <see cref="Assembly.GetCallingAssembly()"/> as a last resort.
        /// This method is guaranteed not to return null.
        /// </summary>
        public static Assembly AssemblyOrDefault(Assembly assembly = null)
        {
            return assembly ?? Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        }

        #region Assembly Name

        /// <seealso cref="AssemblyOrDefault"/>
        public static string GetAssemblyName(Assembly assembly = null)
        {
            return AssemblyOrDefault(assembly).GetName().Name;
        }

        public static string GetAssemblyName(Type type)
        {
            return GetAssemblyName(Assembly.GetAssembly(type));
        }

        /// <seealso cref="AssemblyOrDefault"/>
        public static Version GetAssemblyVersion(Assembly assembly = null)
        {
            return AssemblyOrDefault(assembly).GetName().Version;
        }

        public static Version GetAssemblyVersion(Type type)
        {
            return GetAssemblyVersion(Assembly.GetAssembly(type));
        }

        #endregion

        #region Assembly Version

        /// <summary>
        /// Gets a "human friendly" version number for the given assembly in which any trailing ".0" are removed.
        /// </summary>
        /// <example>GetAssemblyVersionShort(1.0.0.0) -> 1</example>
        /// <example>GetAssemblyVersionShort(1.2.0.0) -> 1.2</example>
        /// <example>GetAssemblyVersionShort(1.2.4.0) -> 1.2.4</example>
        /// <example>GetAssemblyVersionShort(1.2.4.8) -> 1.2.4.8</example>
        /// <example>GetAssemblyVersionShort(1.0.0.0, true) -> 1.0</example>
        /// <example>GetAssemblyVersionShort(1.2.0.0, true) -> 1.2</example>
        /// <param name="assembly">The assembly containing the required version number.</param>
        /// <param name="keepMinorIfZero">Keep the minor version number even if it is zero.</param>
        /// <seealso cref="AssemblyOrDefault"/>
        public static string GetAssemblyVersionShort(Assembly assembly = null, bool keepMinorIfZero = false)
        {
            var version = Regex.Replace(GetAssemblyVersion(assembly).ToString(), @"(?:\.0)+$", "");

            // 1.0.0.0 -> 1 -> 1.0
            if (keepMinorIfZero && new Regex(@"^\d+$").IsMatch(version))
                version += ".0";

            return version;
        }

        public static string GetAssemblyVersionShort(Type type)
        {
            return GetAssemblyVersionShort(Assembly.GetAssembly(type));
        }

        #endregion

        #region Install Directory

        /// <summary>
        /// Gets the path to the directory that contains the given assembly.
        /// </summary>
        /// <seealso cref="AssemblyOrDefault"/>
        public static string GetInstallDir(Assembly assembly = null)
        {
            return Path.GetDirectoryName(AssemblyOrDefault(assembly).Location);
        }

        #endregion

        /// <summary>
        /// Gets the .NET GuidAttribute value for the given assembly.
        /// </summary>
        public static string Guid(Assembly assembly = null)
        {
            assembly = AssemblyOrDefault(assembly);
            return ((GuidAttribute) assembly.GetCustomAttributes(typeof (GuidAttribute), true)[0]).Value;
        }

        #region Dates

        /// <summary>
        /// Gets the date and time that the given assembly was linked.
        /// </summary>
        /// <returns>Date and time the assembly was linked</returns>
        /// <seealso cref="http://stackoverflow.com/a/1600990/467582"/>
        public static DateTime GetLinkerTimestamp(Assembly assembly = null)
        {
            assembly = AssemblyOrDefault(assembly);

            string filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            Stream s = null;

            try
            {
                s = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

        #endregion

        #region Temp Directories

        /// <summary>
        /// Creates a temporary directory "%TEMP%/{Assembly.Name}/{Assembly.Version}/{Process.ID}/{subFolderNames[0]}/{subFolderNames[1]}/{...}" and returns its path.
        /// </summary>
        /// <seealso cref="AssemblyOrDefault"/>
        public static string GetTempDir(Assembly assembly = null, params string[] subFolderNames)
        {
            var folderNames = new List<string>
                            {
                                Path.GetTempPath(),
                                GetAssemblyName(assembly),
                                GetAssemblyVersion(assembly).ToString(),
                                Process.GetCurrentProcess().Id.ToString("0")
                            };
            folderNames.AddRange(subFolderNames.Where(s => !string.IsNullOrWhiteSpace(s)));
            var path = Path.Combine(folderNames.ToArray());
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Creates a temporary directory "%TEMP%/{Assembly.Name}/{Assembly.Version}/{Process.ID}/{subFolderNames[0]}/{subFolderNames[1]}/{...}" and returns its path.
        /// </summary>
        public static string GetTempDir(Type type, params string[] subFolderNames)
        {
            return GetTempDir(Assembly.GetAssembly(type), subFolderNames);
        }

        #endregion

        #region Temp Files

        /// <summary>
        /// Creates a temporary directory "%TEMP%/{Assembly.Name}/{Assembly.Version}/{Process.ID}/{subFolderNames[0]}/{subFolderNames[1]}/{...}" and returns the full path to <paramref name="filename"/> within that directory.
        /// </summary>
        /// <seealso cref="AssemblyOrDefault"/>
        public static string GetTempFilePath(Assembly assembly, string filename, params string[] subFolderNames)
        {
            return Path.Combine(GetTempDir(assembly, subFolderNames), filename);
        }

        /// <summary>
        /// Creates a temporary directory "%TEMP%/{Assembly.Name}/{Assembly.Version}/{Process.ID}/{subFolderNames[0]}/{subFolderNames[1]}/{...}" and returns the full path to <paramref name="filename"/> within that directory.
        /// </summary>
        public static string GetTempFilePath(Type type, string filename, params string[] subFolderNames)
        {
            return GetTempFilePath(Assembly.GetAssembly(type), filename, subFolderNames);
        }

        #endregion
    }
}
