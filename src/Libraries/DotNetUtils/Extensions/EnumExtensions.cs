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
