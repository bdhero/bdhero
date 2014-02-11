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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DotNetUtils
{
    /// <summary>
    ///     Collection of helpful utilities for working with Assemblies (name, version, install path)
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        ///     Returns
        ///     <paramref name="assembly" /> if it is not <c>null</c>, otherwise
        ///     <see cref="Assembly.GetEntryAssembly()" /> if it is not <c>null</c>, or
        ///     <see cref="Assembly.GetCallingAssembly()" /> as a last resort.
        ///     This method is guaranteed not to return null.
        /// </summary>
        public static Assembly AssemblyOrDefault(Assembly assembly = null)
        {
            return assembly ?? Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        }

        /// <summary>
        ///     Gets the .NET GuidAttribute value for the given assembly.
        /// </summary>
        public static Guid Guid(Assembly assembly = null)
        {
            assembly = AssemblyOrDefault(assembly);
            var guid = ((GuidAttribute) assembly.GetCustomAttributes(typeof (GuidAttribute), true)[0]).Value;
            return new Guid(guid);
        }

        #region Dates

        /// <summary>
        ///     Gets the date and time that the given assembly was linked.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>Date and time the assembly was linked.</returns>
        /// <seealso cref="http://stackoverflow.com/a/1600990/467582" />
        public static DateTime GetLinkerTimestamp(Assembly assembly = null)
        {
            assembly = AssemblyOrDefault(assembly);

            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            const int bufferSize = 2048;

            var filePath = assembly.Location;
            var buffer = new byte[bufferSize];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, bufferSize);
            }

            var i = BitConverter.ToInt32(buffer, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, i + linkerTimestampOffset);
            var timestamp = new DateTime(1970, 1, 1, 0, 0, 0);
            timestamp = timestamp.AddSeconds(secondsSince1970);
            timestamp = timestamp.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(timestamp).Hours);
            return timestamp;
        }

        #endregion

        #region Assembly Name

        /// <summary>
        ///     Gets the simple name of the assembly.
        ///     This is usually, but not necessarily, the file name of the manifest file of the assembly,
        ///     minus its extension.
        /// </summary>
        /// <param name="assembly">See <see cref="AssemblyOrDefault" />.</param>
        /// <returns>The simple name of the assembly.</returns>
        /// <seealso cref="AssemblyOrDefault" />
        public static string GetAssemblyName(Assembly assembly = null)
        {
            return AssemblyOrDefault(assembly).GetName().Name;
        }

        /// <summary>
        ///     Gets the simple name of the assembly that contains <paramref name="type" />.
        ///     This is usually, but not necessarily, the file name of the manifest file of the assembly,
        ///     minus its extension.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The simple name of the assembly that contains <paramref name="type" />.</returns>
        public static string GetAssemblyName(Type type)
        {
            return GetAssemblyName(Assembly.GetAssembly(type));
        }

        /// <summary>
        ///     Gets the assembly's version.
        /// </summary>
        /// <param name="assembly">See <see cref="AssemblyOrDefault" />.</param>
        /// <returns>The assembly's version.</returns>
        public static Version GetAssemblyVersion(Assembly assembly = null)
        {
            return AssemblyOrDefault(assembly).GetName().Version;
        }

        /// <summary>
        ///     Gets the version of the assembly that contains <paramref name="type" />.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>The version of the assembly that contains <paramref name="type" />.</returns>
        public static Version GetAssemblyVersion(Type type)
        {
            return GetAssemblyVersion(Assembly.GetAssembly(type));
        }

        #endregion

        #region Assembly Version

        /// <summary>
        ///     Gets a "human friendly" version number for the given assembly in which any trailing ".0" are removed.
        /// </summary>
        /// <param name="assembly">The assembly containing the required version number.</param>
        /// <param name="keepMinorIfZero">Keep the minor version number even if it is zero.</param>
        /// <returns>Human friendly version number of the given assembly.</returns>
        /// <example><pre>GetAssemblyVersionShort(1.0.0.0) -> 1</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.0.0) -> 1.2</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.4.0) -> 1.2.4</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.4.8) -> 1.2.4.8</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.0.0.0, true) -> 1.0</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.0.0, true) -> 1.2</pre></example>
        /// <seealso cref="AssemblyOrDefault" />
        public static string GetAssemblyVersionShort(Assembly assembly = null, bool keepMinorIfZero = false)
        {
            var version = Regex.Replace(GetAssemblyVersion(assembly).ToString(), @"(?:\.0)+$", "");

            // 1.0.0.0 -> 1 -> 1.0
            if (keepMinorIfZero && new Regex(@"^\d+$").IsMatch(version))
            {
                version += ".0";
            }

            return version;
        }

        /// <summary>
        ///     Gets a "human friendly" version number of the assembly that contains <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Human friendly version number of the assembly that contains <paramref name="type"/>.</returns>
        /// <example><pre>GetAssemblyVersionShort(1.0.0.0) -> 1</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.0.0) -> 1.2</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.4.0) -> 1.2.4</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.4.8) -> 1.2.4.8</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.0.0.0, true) -> 1.0</pre></example>
        /// <example><pre>GetAssemblyVersionShort(1.2.0.0, true) -> 1.2</pre></example>
        public static string GetAssemblyVersionShort(Type type)
        {
            return GetAssemblyVersionShort(Assembly.GetAssembly(type));
        }

        #endregion

        #region Install Directory

        /// <summary>
        ///     Gets the path to the directory that contains the given assembly.
        /// </summary>
        /// <seealso cref="AssemblyOrDefault" />
        public static string GetInstallDir(Assembly assembly = null)
        {
            return Path.GetDirectoryName(AssemblyOrDefault(assembly).Location);
        }

        /// <summary>
        ///     Gets the path to the directory that contains the assembly for the given <paramref name="type" />.
        /// </summary>
        /// <seealso cref="AssemblyOrDefault" />
        public static string GetInstallDir(Type type)
        {
            return Path.GetDirectoryName(AssemblyOrDefault(Assembly.GetAssembly(type)).Location);
        }

        #endregion
    }
}
