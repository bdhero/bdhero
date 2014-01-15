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
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsOSUtils.WMI
{
    /// <summary>
    /// Utility methods for interacting with WMI objects.
    /// </summary>
    public static class WMIUtils
    {
        /// <summary>
        /// Queries the Win32 WMI object searcher for all instances of the given class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">Optional WMI query string condition (e.g., "WHERE X = 3")</param>
        /// <returns>List of WMI class object instances</returns>
        public static List<T> GetRuntimeInstances<T>(string condition = null)
        {
            var wmiClassName = GetWMIClassName<T>();
            using (var searcher = new ManagementObjectSearcher(string.Format("Select * From {0} {1}", wmiClassName, condition ?? "")))
            {
                using (var collection = searcher.Get())
                {
                    var runtimeObjects = (from ManagementBaseObject device in collection
                                          select FromManagementObject<T>(device)).ToList<T>();
                    return runtimeObjects;
                }
            }
        }

        /// <summary>
        /// Gets the WMI class name of a .NET class or struct.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The WMI class name of the .NET class or struct represented by <typeparamref name="T"/></returns>
        public static string GetWMIClassName<T>()
        {
            var type = typeof (T);
            var wmiClassNameAttr = Attribute.GetCustomAttributes(type)
                                            .OfType<WMIClassNameAttribute>()
                                            .FirstOrDefault();
            if (wmiClassNameAttr != null)
            {
                return wmiClassNameAttr.WMIClassName;
            }

            var wmiClassName = type.Name;

            if (!new Regex("^[a-zA-Z0-9]_").IsMatch(wmiClassName))
            {
                wmiClassName = string.Format("Win32_{0}", wmiClassName);
            }

            return wmiClassName;
        }

        /// <summary>
        /// Converts a Win32 WMI object to a .NET class instance or struct.
        /// </summary>
        /// <param name="managementBaseObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Instance of the .NET class <typeparamref name="T"/> populated with the data from <paramref name="managementBaseObject"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown if the .NET class <typeparamref name="T"/> does not have a zero-argument constructor.</exception>
        public static T FromManagementObject<T>(ManagementBaseObject managementBaseObject)
        {
            var type = typeof(T);
            var publicInstanceFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var zeroArgCtor = type.GetConstructor(new Type[0]);

            if (zeroArgCtor != null)
            {
                var instance = (T) zeroArgCtor.Invoke(new object[0]);

                foreach (var fieldInfo in publicInstanceFields)
                {
                    var value = managementBaseObject.GetPropertyValue(fieldInfo.Name);
                    fieldInfo.SetValue(instance, value);
                }

                return instance;
            }

            var publicFieldTypes = publicInstanceFields.Select(info => info.FieldType).ToArray();
            var fieldArgCtor = type.GetConstructor(publicFieldTypes);

            if (fieldArgCtor != null)
            {
                var args = publicInstanceFields.Select(info => managementBaseObject.GetPropertyValue(info.Name)).ToArray();
                var instance = (T) fieldArgCtor.Invoke(args);
                return instance;
            }

            throw new NotSupportedException(string.Format("No usable constructors found for type {0}.  Type must have a zero-argument constructor or a constructor with arguments for all public fields.", type.FullName));
        }

        /// <summary>
        /// Converts the Win32 WMI object stored in <paramref name="args"/> to a .NET class instance or struct.
        /// </summary>
        /// <param name="args"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Instance of the .NET class <typeparamref name="T"/> populated with the data from <paramref name="args"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown if the .NET class <typeparamref name="T"/> does not have a zero-argument constructor.</exception>
        public static T FromEvent<T>(EventArrivedEventArgs args)
        {
            return FromManagementObject<T>(args.NewEvent);
        }
    }
}
