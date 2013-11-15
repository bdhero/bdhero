using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetUtils
{
    public static class ReflectionUtils
    {
        public static string ToString(Object obj)
        {
            var fields = obj.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Select(info => ToStringImpl(info.GetValue(obj), info));
            var props = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(info => ToStringImpl(info.GetValue(obj, new object[0]), info));
            return string.Join(Environment.NewLine, fields.Concat(props));
        }

        private static string ToStringImpl(Object obj, MemberInfo info)
        {
            var value = (obj ?? "null").ToString();
            var lines = Regex.Split(value, @"[\n\r\f]+");
            if (lines.Count() > 1)
                value = Environment.NewLine + string.Join(Environment.NewLine, lines.Select(s => "    " + s));
            return string.Format("{0}: {1}", info.Name, value);
        }
    }
}
