// Copyright 2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Text;
using DotNetUtils;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace LicenseUtils
{
    /// <summary>
    ///     A software license agreement.
    /// </summary>
    [UsedImplicitly]
    public class License
    {
        /// <summary>
        ///     The machine-readable name of the license.  Appending <c>".md"</c> to this value will produce the
        ///     filename needed to load the contents of <see cref="Text"/>.
        /// </summary>
        /// <example>
        ///     <c>"cc_sa_2.5"</c>
        /// </example>
        [JsonIgnore]
        public string Id;

        /// <summary>
        ///     The full human-readable name of the license.
        /// </summary>
        /// <example>
        ///     <c>"GNU General Public License"</c>
        /// </example>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        ///     Abbreviation of <see cref="Name"/>, if applicable.
        /// </summary>
        /// <example>
        ///     <c>"GPL"</c>
        /// </example>
        [JsonProperty("abbr")]
        public string Abbreviation;

        /// <summary>
        ///     The specific version number of the license agreement, if applicable.
        /// </summary>
        /// <example>
        ///     <c>2</c>, <c>2.1</c>, <c>3</c>
        /// </example>
        [JsonProperty("version")]
        public float? Version;

        /// <summary>
        ///     The primary URL of the website where users should go to view or learn more about the license agreement.
        /// </summary>
        /// <example>
        ///     <c>"http://www.gnu.org/licenses/gpl-2.0.html"</c>
        /// </example>
        [JsonProperty("url")]
        public string Url;

        /// <summary>
        ///     URL of a short summary of the license on TLDRLegal
        /// </summary>
        /// <example>
        ///     <c>"http://www.gnu.org/licenses/gpl-2.0.html"</c>
        /// </example>
        [JsonProperty("tldr_url")]
        public string TlDrUrl;

        /// <summary>
        ///     Gets whether the license is a custom, project-specific license (<c>true</c>)
        ///     or a standard, general-purpose license (<c>false</c>).
        /// </summary>
        [JsonProperty("isCustom")]
        public bool IsCustom;

        /// <summary>
        ///     The full HTML markup of the license agreement.  This is the formatted equivalent of <see cref="Text"/>.
        /// </summary>
        [JsonIgnore]
        public string Html;

        /// <summary>
        ///     The plain text contents of the license agreement.  This is the unformatted equivalent of <see cref="Html"/>.
        /// </summary>
        [JsonIgnore]
        public string Text;

        public override string ToString()
        {
            var sb = new StringBuilder();

            // Name
            if (!string.IsNullOrEmpty(Abbreviation))
                sb.Append(Abbreviation);
            else
                sb.Append(Name);

            // Version
            if (Version.HasValue)
                sb.Append(" v" + Version.Value);

            return sb.ToString();
        }

        public string ToStringDescriptive()
        {
            return string.Format(IsCustom ? "a {0}" : "the {0} license", ToString());
        }

        public string ToStringFull()
        {
            var items = new List<string>();

            items.Add(Name);

            // Version
            if (Version.HasValue)
                items.Add("version " + Version.Value + "");

            // Abbreviation
            if (!string.IsNullOrEmpty(Abbreviation))
            {
                var abbr = Abbreviation;

                if (Version.HasValue)
                    abbr += "-" + Version.Value;

                items.Add(string.Format("({0})", abbr));
            }

            return string.Join(" ", items);
        }
    }
}
