﻿using System;
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

            var str = value.ToString();
            var lines = Regex.Split(str, @"[\n\r\f]+");

            if (lines.Count() > 1)
                str = Environment.NewLine + string.Join(Environment.NewLine, lines.Select(s => "    " + s));

            return str;
        }
    }
}
