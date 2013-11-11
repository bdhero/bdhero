using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Plugin
{
    /// <summary>
    ///     Provides an interface for observers to be notified whenever a plugin updates its progress.
    /// </summary>
    public interface IPluginHost
    {
        /// <summary>
        ///     Allows plugins to report their progress to the host.
        /// </summary>
        /// <param name="plugin">Plugin that is reporting its progress</param>
        /// <param name="percentComplete">0.0 to 100.0</param>
        /// <param name="status">
        ///     Description of what the plugin is currently doing
        ///     (e.g., "Parsing 00850.MPLS", "Muxing to MKV @ 00:45:19").
        /// </param>
        void ReportProgress(IPlugin plugin, double percentComplete, string status);

        /// <summary>
        ///     Gets the <see cref="ProgressProvider"/> for the specified <paramref name="plugin"/>.
        /// </summary>
        /// <param name="plugin">The plugin whose <see cref="ProgressProvider"/> is required.</param>
        /// <returns>
        ///     The <see cref="ProgressProvider"/> for <paramref name="plugin"/>.
        /// </returns>
        ProgressProvider GetProgressProvider(IPlugin plugin);
    }
}
