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
    internal class SnakeCaseContractResolver : DefaultContractResolver
    {
        private static readonly Regex PascalCaseRegex = new Regex("([a-z])([A-Z])");

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var res = base.CreateProperty(member, memberSerialization);
            var attrs = member.GetCustomAttributes(typeof(JsonPropertyAttribute), true);

            if (attrs.Any())
            {
                var attr = (attrs[0] as JsonPropertyAttribute);
                if (res.PropertyName != null && attr != null)
                    res.PropertyName = attr.PropertyName;
            }
            else
            {
                res.PropertyName = PascalCaseRegex.Replace(member.Name, "$1_$2").ToLower();
            }
 
            return res;
        }
    }
}
