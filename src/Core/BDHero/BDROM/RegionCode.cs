using System;
using System.ComponentModel;
using System.Linq;

namespace BDHero.BDROM
{
    /// <summary>
    /// Represents a Blu-ray region code.
    /// NOTE: Region-free discs may be reported by AnyDVD HD as being coded to a particular region depending on the user's AnyDVD config settings.
    /// </summary>
    public enum RegionCode
    {
        /// <summary>
        /// No region code is present (region-free).
        /// </summary>
        [Description("No region code is present (region-free).")]
        Free = -1,

        /// <summary>
        /// A/1: Includes most North, Central, and South American and Southeast Asian countries plus Taiwan, Japan, Hong Kong, Macau, and Korea.
        /// </summary>
        /// <remarks>
        /// The Americas (except Greenland) and their dependencies, East Asia (except mainland China and Mongolia), and Southeast Asia.
        /// </remarks>
        [Description("The Americas (except Greenland) and their dependencies, East Asia (except mainland China and Mongolia), and Southeast Asia.")]
        A = 1,

        /// <summary>
        /// B/2: Includes most European, African, and Southwest Asian countries plus Australia and New Zealand.
        /// </summary>
        /// <remarks>
        /// Africa, Middle East, Southwest Asia, Europe (except Belarus, Russia, Ukraine and Kazakhstan), Australia, New Zealand, and their dependencies.
        /// </remarks>
        [Description("Africa, Middle East, Southwest Asia, Europe (except Belarus, Russia, Ukraine and Kazakhstan), Australia, New Zealand, and their dependencies.")]
        B = 2,

        /// <summary>
        /// C/3: Includes the remaining central and south Asian countries, as well as China and Russia.
        /// </summary>
        /// <remarks>
        /// Central Asia, East Asia (mainland China and Mongolia only), South Asia, Eastern Europe (Belarus, Russia, Ukraine and Kazakhstan only), and their dependencies.
        /// </remarks>
        [Description("Central Asia, East Asia (mainland China and Mongolia only), South Asia, Eastern Europe (Belarus, Russia, Ukraine and Kazakhstan only), and their dependencies.")]
        C = 3
    }

    public static class RegionCodeParser
    {
        public static RegionCode Parse(string str)
        {
            // Older versions of AnyDVD HD (7.1.3.0 and lower) used "0" for region-free, whereas
            // newer versions use "-1".  Normalize old values to their new equivalent.
            if (str == "0")
                str = "-1";

            RegionCode code;
            Enum.TryParse(str, out code);
            return code;
        }

        /// <summary>
        ///     Gets the region code's short name (e.g., <c>"A"</c>, <c>"B"</c>, <c>"C"</c>, <c>"Free"</c>).
        /// </summary>
        public static string GetName(this RegionCode regionCode)
        {
            return string.Format("{0}", Enum.GetName(regionCode.GetType(), regionCode));
        }

        /// <summary>
        ///     Gets the region code's long name (e.g., <c>"Region A"</c>, <c>"Region B"</c>, <c>"Region C"</c>, <c>"Region-free"</c>).
        /// </summary>
        public static string GetLongName(this RegionCode regionCode)
        {
            return regionCode == RegionCode.Free
                       ? "Region-free"
                       : string.Format("Region {0}", regionCode.GetName());
        }

        public static string GetDescription(this RegionCode regionCode)
        {
            var type = typeof(RegionCode);
            var info = type.GetMember(regionCode.ToString());

            if (!info.Any())
                return null;

            var attr = info[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (!attr.Any())
                return null;

            var description = ((DescriptionAttribute)attr[0]).Description;
            return description;
        }
    }
}