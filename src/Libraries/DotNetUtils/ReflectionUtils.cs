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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotNetUtils.Attributes;

namespace DotNetUtils
{
    /// <summary>
    ///     Utility methods for working with .NET reflection.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        ///     Invokes the specified <paramref name="action"/> if and only if a non-public field of type
        ///     <typeparamref name="T"/> with the specified <paramref name="name"/> exists on <paramref name="obj"/>
        ///     and the value of the field is not <c>null</c>.
        /// </summary>
        /// <param name="obj">
        ///     An object whose non-public field will be accessed.
        /// </param>
        /// <param name="name">
        ///     The name of the non-public field to access.
        /// </param>
        /// <param name="action">
        ///     An action to invoke if the requested field exists with the specified <paramref name="name"/> and
        ///     type <typeparamref name="T"/> and is not <c>null</c>.
        /// </param>
        /// <typeparam name="T">
        ///     The data type of the field being accessed.
        /// </typeparam>
        public static void WithField<T>(this object obj, string name, Action<T> action)
            where T : class
        {
            try
            {
                var type = obj.GetType();
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);
                var member = fields.FirstOrDefault(info => info.FieldType == typeof(T) && info.Name == name);
                if (member == null)
                    return;

                var value = member.GetValue(obj) as T;
                if (value == default(T))
                    return;

                action(value);
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Invokes the method with the specified <paramref name="name"/> on the given <paramref name="obj"/>
        ///     with the specified <paramref name="params"/>.  If the requested method does not exist,
        ///     nothing will happen (no exception is thrown).
        /// </summary>
        /// <param name="obj">
        ///     Object to invoke the method on.
        /// </param>
        /// <param name="name">
        ///     Name of the method to invoke.
        /// </param>
        /// <param name="params">
        ///     Parameters to pass to the method.
        /// </param>
        public static void InvokeMethod(this object obj, string name, params object[] @params)
        {
            try
            {
                var type = obj.GetType();
                var method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);
                if (method == null)
                    return;

                method.Invoke(obj, @params);
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Generates a hierarchical YAML-like string representation of an object.
        /// </summary>
        /// <param name="obj">Object to stringify.</param>
        /// <returns>Human-readable string representation of <paramref name="obj"/>.</returns>
        public static string ToString(Object obj)
        {
            var fields = obj.GetType()
                            .GetFields(BindingFlags.Instance | BindingFlags.Public)
                            .Select(member => ToString(member, member.GetValue(obj)));
            var props = obj.GetType()
                           .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                           .Select(member => ToString(member, member.GetValue(obj, new object[0])));
            return string.Join(Environment.NewLine, fields.Concat(props));
        }

        private static string ToString(MemberInfo member, Object value)
        {
            var str = ToStringImpl(member, value);
            return string.Format("{0}: {1}", member.Name, str);
        }

        private static string ToStringImpl(MemberInfo member, Object value)
        {
            if (value == null)
                return "null";

            var fileSizeAttr = member.GetCustomAttributes(typeof (FileSizeAttribute), false)
                                     .OfType<FileSizeAttribute>()
                                     .FirstOrDefault();

            if (fileSizeAttr != null)
            {
                if (value is long)
                    return fileSizeAttr.Format((long) value);
                if (value is ulong)
                    return fileSizeAttr.Format((ulong) value);
            }

            var collection = value as ICollection<Object>;
            if (collection != null)
            {
                return string.Format("[ {0} ]", string.Join(", ", collection));
            }

            var str = value.ToString();
            var lines = Regex.Split(str, @"[\n\r\f]+");

            if (lines.Count() > 1)
                str = Environment.NewLine + string.Join(Environment.NewLine, lines.Select(s => "    " + s));

            return str;
        }
    }
}
