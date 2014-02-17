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
using DotNetUtils;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace LicenseUtils
{
    /// <summary>
    ///     Collection of <see cref="Work"/>s distributed with BDHero.
    /// </summary>
    [UsedImplicitly]
    public class Works
    {
        /// <summary>
        ///     Gets the concatenation of <see cref="Derivatives"/>, <see cref="Originals"/>, <see cref="Snippets"/>,
        ///     <see cref="Packages"/>, <see cref="Libraries"/>, and <see cref="Binaries"/>.
        /// </summary>
        [JsonIgnore]
        public Work[] All
        {
            get
            {
                var works = new List<Work>();
                works.AddRange(Derivatives);
                works.AddRange(Originals);
                works.AddRange(Binaries);
                works.AddRange(Libraries);
                works.AddRange(Packages);
                works.AddRange(Snippets);
                return works.ToArray();
            }
        }

        /// <summary>
        ///     Works derived from <see cref="Originals"/>.
        /// </summary>
        /// <example>
        ///     BDHero.
        /// </example>
        [JsonProperty("derivatives")]
        public Work[] Derivatives;

        /// <summary>
        ///     Original works that were not derived from another source.
        /// </summary>
        /// <example>
        ///     BDInfo.
        /// </example>
        [JsonProperty("originals")]
        public Work[] Originals;

        /// <summary>
        ///     Short code snippets or individual standalone source files.
        /// </summary>
        /// <example>
        ///     Plug-ins in C#, Detecting USB Drive Removal in a C# Program, JobObject.cs.
        /// </example>
        [JsonProperty("snippets")]
        public Work[] Snippets;

        /// <summary>
        ///     NuGet packages.
        /// </summary>
        /// <example>
        ///     Json.NET.
        /// </example>
        [JsonProperty("packages")]
        public Work[] Packages;

        /// <summary>
        ///     Compiled libraries (DLLs).
        /// </summary>
        /// <example>
        ///     Windows API Code Pack.
        /// </example>
        [JsonProperty("libraries")]
        public Work[] Libraries;

        /// <summary>
        ///     Executable binary software packages (EXEs).
        /// </summary>
        /// <example>
        ///     FFmpeg, MKVToolNix.
        /// </example>
        [JsonProperty("binaries")]
        public Work[] Binaries;

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
}
