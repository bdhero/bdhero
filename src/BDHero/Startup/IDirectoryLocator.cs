namespace BDHero.Startup
{
    public interface IDirectoryLocator
    {
        bool   IsPortable        { get; }
        string InstallDir        { get; }
        string AppConfigDir      { get; }
        string PluginConfigDir   { get; }
        string RequiredPluginDir { get; }
        string CustomPluginDir   { get; }
        string LogDir            { get; }
    }
}