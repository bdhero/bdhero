using System;
using System.Linq;
using System.Reflection;

namespace DotNetUtils.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="MemberInfo"/> objects.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        ///     Wrapper around <see cref="MemberInfo.GetCustomAttributes(System.Type,bool)"/> that returns a strongly-typed
        ///     array of attributes that are assignable to <typeparamref name="TAttr"/>.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="inherit">
        ///     <c>true</c> to search this member's inheritance chain to find the attributes; otherwise, <c>false</c>. 
        ///     This parameter is ignored for properties and events; see Remarks.
        /// </param>
        /// <typeparam name="TAttr">
        ///     The type of attribute to search for. Only attributes that are assignable to this type are returned.
        /// </typeparam>
        /// <returns>
        ///     An array of custom attributes applied to this member, or an array with zero elements if no attributes
        ///     assignable to <typeparamref name="TAttr"/> have been applied.
        /// </returns>
        /// <exception cref="TypeLoadException">
        ///     A custom attribute type cannot be loaded.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     If <typeparamref name="TAttr"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     This member belongs to a type that is loaded into the reflection-only context.
        ///     See How to: Load Assemblies into the Reflection-Only Context.
        /// </exception>
        public static TAttr[] GetCustomAttributes<TAttr>(this MemberInfo member, bool inherit = true)
            where TAttr : Attribute
        {
            return member.GetCustomAttributes(typeof (TAttr), inherit)
                         .OfType<TAttr>()
                         .ToArray();
        }

        /// <summary>
        ///     Retrieves a list of all properties defined by this member which implement an interface.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetAllInterfaceProperties(this MemberInfo member)
        {
            var type = member.DeclaringType;

            if (type == null)
                return new PropertyInfo[0];

            var interfaces = type.GetInterfaces();
            var interfaceProperties =
                interfaces.SelectMany(@interface => @interface.GetProperties().Where(info => Matches(member, info)))
                          .ToArray();

            return interfaceProperties;
        }

        private static bool Matches(MemberInfo member, PropertyInfo interfaceProperty)
        {
            // This is weak: among other things, an implementation 
            // may be deliberately hiding an interface member
            return interfaceProperty.Name == member.Name &&
                   interfaceProperty.MemberType == member.MemberType;
        }
    }
}
