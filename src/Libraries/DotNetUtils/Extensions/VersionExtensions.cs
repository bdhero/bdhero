// Copyright 2012-2014 Andrew C. Dvorak
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

using System;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Version"/> objects.
    /// </summary>
    public static class VersionExtensions
    {
        /// <summary>
        /// <para>
        /// Converts the <c>Version</c> to a signed integer representation suitable for use in the
        /// <c>&lt;versionId&gt;</c> tag of a BitRock InstallBuilder update.xml file.
        /// </para>
        /// <para>
        /// Each octet in <paramref name="version"/> is converted to 2 decimal digits and concatenated in
        /// descending order of significance.  Therefore, the value of each octet must not exceed 99.
        /// </para>
        /// </summary>
        /// <example><code>new Version(1, 2, 3, 4).GetId() == 1020304</code></example>
        /// <example><code>new Version(0, 8, 0, 1).GetId() ==   80001</code></example>
        /// <param name="version"></param>
        /// <returns>The value of <paramref name="version"/> as a signed <c>Int32</c></returns>
        /// <seealso cref="http://installbuilder.bitrock.com/docs/installbuilder-userguide/ar01s23.html">BitRock InstallBuilder update.xml file</seealso>
        public static int GetId(this Version version)
        {
            var v = version;
            return int.Parse(string.Format("{0:D2}{1:D2}{2:D2}{3:D2}", v.Major, v.Minor, v.Build, v.Revision));
        }
    }
}