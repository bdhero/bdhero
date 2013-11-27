using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Plugin;

namespace BDHero.Exceptions
{
    /// <summary>
    /// Thrown when no instances of a required plugin could be found.
    /// </summary>
    public class RequiredPluginNotFoundException<T> : Exception where T : IPlugin
    {
        public RequiredPluginNotFoundException()
            : base("Required plugin " + typeof(T).Name + " not found")
        {
        }

        public RequiredPluginNotFoundException(string message)
            : base(message)
        {
        }

        public RequiredPluginNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
