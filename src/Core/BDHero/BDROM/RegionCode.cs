using System;

namespace BDHero.BDROM
{
    /// <summary>
    /// Represents a Blu-ray region code.
    /// NOTE: Region-free discs may be reported by AnyDVD HD as being coded to a particular region depending on the user's AnyDVD config settings.
    /// </summary>
    public enum RegionCode
    {
        /// <summary>
        /// No region code is present.
        /// </summary>
        None = 0,

        /// <summary>
        /// A/1: Includes most North, Central, and South American and Southeast Asian countries plus Taiwan, Japan, Hong Kong, Macau, and Korea.
        /// </summary>
        /// <remarks>
        /// The Americas (except Greenland) and their dependencies, East Asia (except mainland China and Mongolia), and Southeast Asia.
        /// </remarks>
        A = 1,

        /// <summary>
        /// B/2: Includes most European, African, and Southwest Asian countries plus Australia and New Zealand.
        /// </summary>
        /// <remarks>
        /// Africa, Middle East, Southwest Asia, Europe (except Belarus, Russia, Ukraine and Kazakhstan), Australia, New Zealand, and their dependencies.
        /// </remarks>
        B = 2,

        /// <summary>
        /// C/3: Includes the remaining central and south Asian countries, as well as China and Russia.
        /// </summary>
        /// <remarks>
        /// Central Asia, East Asia (mainland China and Mongolia only), South Asia, Eastern Europe (Belarus, Russia, Ukraine and Kazakhstan only), and their dependencies.
        /// </remarks>
        C = 3
    }

    public static class RegionCodeParser
    {
        public static RegionCode Parse(string str)
        {
            RegionCode code;
            Enum.TryParse(str, out code);
            return code;
        }
    }
}