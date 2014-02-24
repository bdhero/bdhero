using System.Diagnostics;
using DotNetUtils.Annotations;

namespace GitHub.Models
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