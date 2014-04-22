using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DotNetUtils.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJsonPlainIndented(this Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
