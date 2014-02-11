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
