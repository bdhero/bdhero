using System;

namespace BDHero.Plugin.FFmpegMuxer
{
    class ArgumentAttribute : Attribute
    {
        public string Name { get; private set; }

        public ArgumentAttribute(string name)
        {
            Name = name;
        }
    }
}