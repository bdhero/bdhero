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

using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotNetUtils.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetUtils
{
    /// <summary>
    ///     Wrapper for <see cref="JsonConvert"/> that provides automatic snake_case property naming.
    /// </summary>
    public static class SmartJsonConvert
    {
        private static readonly JsonSerializerSettings SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new SnakeCaseContractResolver()
            };

        /// <summary>
        ///     Serializes the specified object to a JSON string using the specified <paramref name="formatting"/>.
        /// </summary>
        /// <param name="obj">
        ///     The object to serialize.
        /// </param>
        /// <param name="formatting">
        ///     Indicates how the output is formatted.
        /// </param>
        /// <returns>
        ///     A JSON string representation of the object.
        /// </returns>
        public static string SerializeObject(Object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting, SerializerSettings);
        }

        /// <summary>
        ///     Deserializes the JSON to the specified .NET type using JsonSerializerSettings.
        /// </summary>
        /// <param name="json">
        ///     The object to deserialize.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the object to deserialize to.
        /// </typeparam>
        /// <returns>
        ///     The deserialized object from the JSON string.
        /// </returns>
        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, SerializerSettings);
        }
    }

    /// <summary>
    ///     Example for a custom ContractResolver that honors <see cref="JsonPropertyAttribute"/> (shouldn't this be default?).
    /// </summary>
    /// <seealso cref="http://stackoverflow.com/questions/12749046/camelcase-only-if-propertyname-not-explicitly-set-in-json-net"/>
    /// <seealso cref="http://stackoverflow.com/a/8877076/467582"/>
    internal class SnakeCaseContractResolver : DefaultContractResolver
    {
        private static readonly Regex PascalCaseRegex = new Regex("([a-z]|[A-Z]+)([A-Z])");

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            var attrs = member.GetJsonPropertyAttributes();

            SetPropertyName(member, prop, attrs);

            // Support [JsonIgnore] attribute on interfaces
            foreach (var interfaceProperty in member.GetAllInterfaceProperties())
            {
                if (interfaceProperty.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                {
                    prop.Ignored = true;
                    return prop;
                }

                var interfaceAttrs = interfaceProperty.GetJsonPropertyAttributes();
                if (interfaceAttrs.Any())
                {
                    SetPropertyName(member, prop, interfaceAttrs);
                    return prop;
                }
            }

            return prop;
        }

        private static void SetPropertyName(MemberInfo member, JsonProperty prop, JsonPropertyAttribute[] attrs)
        {
            prop.PropertyName = attrs.Any() ? attrs.First().PropertyName : PascalCaseRegex.Replace(member.Name, "$1_$2").ToLower();
        }
    }

    internal static class InternalMemberInfoExtensions
    {
        public static JsonPropertyAttribute[] GetJsonPropertyAttributes(this MemberInfo member, bool inherit = true)
        {
            return member.GetCustomAttributes<JsonPropertyAttribute>(inherit)
                         .Where(attribute => !string.IsNullOrEmpty(attribute.PropertyName))
                         .ToArray();
        }
    }
}
