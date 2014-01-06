using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils.Attributes;

namespace DotNetUtils
{
    public static class ReflectionUtils
    {
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

            var str = (value ?? "null").ToString();
            var lines = Regex.Split(str, @"[\n\r\f]+");

            if (lines.Count() > 1)
                str = Environment.NewLine + string.Join(Environment.NewLine, lines.Select(s => "    " + s));

            return str;
        }
    }
}
