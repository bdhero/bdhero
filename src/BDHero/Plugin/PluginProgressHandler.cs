using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Plugin
{
    /// <summary>
    /// Invoked by the <see cref="IPluginHost"/> whenever an <see cref="IPlugin"/>'s state or progress changes.
    /// </summary>
    /// <param name="plugin">Plugin that is reporting its progress</param>
    /// <param name="progressProvider">Provides plugin status information</param>
    public delegate void PluginProgressHandler(IPlugin plugin, ProgressProvider progressProvider);
}
