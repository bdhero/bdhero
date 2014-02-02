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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotNetUtils
{
    /// <summary>
    ///     Utility class for quickly generating string representations of classes
    ///     for their <see cref="Object.ToString()"/> method.
    /// </summary>
    /// <typeparam name="TClass">
    ///     Data type of the class containing the <see cref="Object.ToString()"/>
    ///     method being generated.
    /// </typeparam>
    public sealed class ToStringBuilder<TClass>
        where TClass : class
    {
        private readonly TClass _obj;
        private readonly IList<Pair> _props;

        /// <summary>
        ///     Constructs a new <see cref="ToStringBuilder{TClass}"/> object
        ///     that will generate <see cref="Object.ToString()"/> output for
        ///     the given <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">
        ///     Object whose <see cref="Object.ToString()"/> method is being generated.
        /// </param>
        public ToStringBuilder(TClass obj)
        {
            _obj = obj;
            _props = _props = new List<Pair>();
        }

        /// <summary>
        ///     Appends the given property name and value to the output
        ///     <abbr title="if and only if">iff</abbr> the the value is not
        ///     <c>null</c> or empty.
        /// </summary>
        /// <param name="expression">
        ///     A simple property expression that returns the value of a
        ///     property from the target object.
        /// </param>
        /// <typeparam name="TProperty">
        ///     Data type of the property returned by <paramref name="expression"/>.
        /// </typeparam>
        /// <returns>
        ///     A reference to this <see cref="ToStringBuilder{TClass}"/> object.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="expression"/> is not a simple property expression.
        /// </exception>
        public ToStringBuilder<TClass> Append<TProperty>
            (Expression<Func<TClass, TProperty>> expression)
        {
            string propertyName;
            if (!TryGetPropertyName(expression, out propertyName))
            {
                throw new ArgumentException("Expression must be a simple property expression.");
            }

            Func<TClass, TProperty> func = expression.Compile();

            var value = func(_obj);

            if (!Equals(value, default(TProperty)))
            {
                var strValue = string.Format("{0}", value);
                var collection = value as IEnumerable<Object>;

                if (collection != null)
                {
                    strValue = string.Format("[ {0} ]", string.Join(", ", collection));
                }

                if (!string.IsNullOrWhiteSpace(strValue))
                {
                    _props.Add(new Pair(propertyName, strValue));
                }
            }

            return this;
        }

        private static bool TryGetPropertyName<TProperty>
            (Expression<Func<TClass, TProperty>> expression, out string propertyName)
        {
            propertyName = default(string);

            var propertyExpression = expression.Body as MemberExpression;
            if (propertyExpression == null)
            {
                return false;
            }

            propertyName = propertyExpression.Member.Name;
            return true;
        }

        public override string ToString()
        {
            return string.Format("{{ {0} }}", string.Join(", ", _props));
        }

        private class Pair
        {
            public readonly string Key;
            public readonly string Value;

            public Pair(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Key, Value);
            }
        }
    }
}
