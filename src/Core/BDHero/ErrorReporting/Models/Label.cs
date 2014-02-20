using System.Diagnostics;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace BDHero.ErrorReporting.Models
{
    [UsedImplicitly]
    [DebuggerDisplay("{Name}")]
    public class Label
    {
        public string Url;
        public string Name;
        public string Color;
    }
}