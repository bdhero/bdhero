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

using DotNetUtils;
using DotNetUtils.Annotations;

namespace LicenseUtils
{
    /// <summary>
    ///     Common URLs for a <see cref="Work"/>.
    /// </summary>
    [UsedImplicitly]
    public class Urls
    {
        /// <summary>
        ///     URL of the article, blog or forum post that linked to (or contained) the source code.
        /// </summary>
        /// <example>
        ///     <c>"http://www.codeproject.com/Articles/6334/Plug-ins-in-C"</c>
        /// </example>
        public string Article;

        /// <summary>
        ///     NuGet package URL.
        /// </summary>
        /// <example>
        ///     <c>"http://www.nuget.org/packages/Ninject/"</c>
        /// </example>
        public string Package;

        /// <summary>
        ///     Project homepage URL.
        /// </summary>
        /// <example>
        ///     <c>"http://www.ninject.org/"</c>
        /// </example>
        public string Project;

        /// <summary>
        ///     URL where the source code can be obtained.
        /// </summary>
        /// <example>
        ///     <c>"https://github.com/ninject/ninject"</c>
        /// </example>
        public string Source;

        public override string ToString()
        {
            return new ToStringBuilder<Urls>(this)
                .Append(urls => urls.Article)
                .Append(urls => urls.Package)
                .Append(urls => urls.Project)
                .Append(urls => urls.Source)
                .ToString();
        }
    }
}