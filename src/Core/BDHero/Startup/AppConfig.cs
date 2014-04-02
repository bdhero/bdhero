using System.Linq;

namespace BDHero.Startup
{
    public class AppConfig
    {
        public bool IsDebugMode;

        public void ParseArgs(params string[] args)
        {
            IsDebugMode = args.Contains("--debug");
        }
    }
}
