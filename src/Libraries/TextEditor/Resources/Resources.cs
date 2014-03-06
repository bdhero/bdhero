using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TextEditor.Resources
{
    static class Resources
    {
        static readonly string Prefix = CurrentAssembly.GetName().Name + ".Resources.";

        private static Assembly CurrentAssembly
        {
            get { return typeof (Resources).Assembly; }
        }

        public static Stream OpenStream(string name)
        {
            var stream = CurrentAssembly.GetManifestResourceStream(Prefix + name);
            if (stream == null)
                throw new FileNotFoundException("The resource file '" + name + "' was not found.");
            return stream;
        }

        public static string[] GetResourceNamesEndingWith(string suffix)
        {
            return CurrentAssembly.GetManifestResourceNames().Where(name => name.EndsWith(suffix)).ToArray();
        }
    }
}
