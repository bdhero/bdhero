namespace BDHero.Plugin
{
    /// <summary>
    ///     Responsible for finding, loading, and unloading plugin DLLs.
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        ///     Searches the plugin directory for plugin DLLs and loads them.
        /// </summary>
        /// <param name="path">
        ///     Full path to the root Plugins directory.
        /// </param>
        void LoadPlugins(string path);

        /// <summary>
        ///     Unloads and clears all available loaded plugins.
        /// </summary>
        void UnloadPlugins();
    }
}
