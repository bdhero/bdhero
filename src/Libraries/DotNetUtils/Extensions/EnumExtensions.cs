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
using System.Linq;
using DotNetUtils.Annotations;

namespace DotNetUtils.Extensions
{
    /// <summary>
    ///     Extension methods for enumerations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the value of a property of an attribute attached to the given enum <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TResult">The type of the attribute's property.</typeparam>
        /// <param name="value">Enum value.</param>
        /// <param name="expression">Function expression that returns the value of the desired attribute property.</param>
        /// <returns>The value of the attribute's property returned by <paramref name="expression"/>.</returns>
        /// <seealso cref="http://stackoverflow.com/a/4877704/467582"/>
        [CanBeNull]
        public static TResult GetAttributeProperty<TAttribute, TResult>(this Enum value, Func<TAttribute, TResult> expression)
            where TAttribute : Attribute
        {
            var attribute = value.GetAttribute<TAttribute>();
            return attribute == null ? default(TResult) : expression(attribute);
        }

        /// <summary>
        ///     Gets the value of an attribute attached to the given enum <paramref name="value" />.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="value">Enum value.</param>
        /// <returns>The attribute value.</returns>
        /// <seealso cref="http://stackoverflow.com/a/4877704/467582" />
        [CanBeNull]
        public static T GetAttribute<T>(this Enum value)
            where T : Attribute
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();

            if (member == null)
                return default(T);

            T attr = member.GetCustomAttributes(typeof (T), false)
                           .Cast<T>()
                           .SingleOrDefault();

            return attr;
        }
    }
}
