using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetUtils;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace LicenseUtils
{
    /// <summary>
    ///     A person or organization that contributed to a <see cref="Work"/>.
    /// </summary>
    [UsedImplicitly]
    public class Author
    {
        /// <summary>
        ///     Array of years that this author made copyrightable contributions to the <see cref="Work"/>.
        /// </summary>
        public int[] Years;

        /// <summary>
        ///     The person's full name.
        /// </summary>
        public string Name;

        /// <summary>
        ///     The name of the organization that commissioned the <see cref="Work"/>.
        /// </summary>
        public string Organization;

        /// <summary>
        ///     The author's primary contact email address.
        /// </summary>
        public string Email;

        /// <summary>
        ///     The author's homepage URL.
        /// </summary>
        public string Url;

        /// <summary>
        ///     Gets a human-readable summary of <see cref="Years"/> as a series of
        ///     year ranges.
        /// </summary>
        /// <example>
        ///     <c>"2001-2005, 2009, 2011-2012"</c>
        /// </example>
        [JsonIgnore]
        public string YearRanges
        {
            get
            {
                if (!Years.Any())
                    return "";

                var ranges = new List<string>();

                var numYears = Years.Length;

                for (var i = 0; i < numYears; i++)
                {
                    var rangeStart = Years[i];
                    var rangeEnd = rangeStart;

                    for (var j = i + 1; j < numYears; j++)
                    {
                        if (Years[j] == rangeEnd + 1)
                        {
                            rangeEnd = Years[j];
                            i += 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (rangeStart == rangeEnd)
                        ranges.Add(rangeStart + "");
                    else
                        ranges.Add(string.Format("{0}-{1}", rangeStart, rangeEnd));
                }

                return string.Join(", ", ranges);
            }
        }

        public override string ToString()
        {
            var props = new List<string>();
            var years = YearRanges;
            if (!string.IsNullOrWhiteSpace(years))
                props.Add(years);
            props.Add(Name ?? Organization);
            return string.Join(" ", props);
        }

        public string ToStringDescriptive()
        {
            var props = new List<string>();

            props.Add(ToString());

            if (!string.IsNullOrEmpty(Email))
                props.Add(string.Format("<{0}>", Email));
            else if (!string.IsNullOrEmpty(Url))
                props.Add(string.Format("({0})", Url));

            return string.Join(" ", props);
        }
    }
}
