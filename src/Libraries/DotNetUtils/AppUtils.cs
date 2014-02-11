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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNetUtils
{
    /// <summary>
    ///     Static helper class that provides application assembly meta information,
    ///     such as app name, app version, build date, and product name.
    /// </summary>
    public static class AppUtils
    {
        #region Assembly Attribute Accessors

        /// <summary>
        ///     Gets the human-friendly name of the application.
        /// </summary>
        /// <example>
        ///     <code>"BDHero GUI"</code>
        /// </example>
        public static string AppName
        {
            get
            {
                var titleAttribute = GetAttribute<AssemblyTitleAttribute>();
                if (titleAttribute != null && !string.IsNullOrWhiteSpace(titleAttribute.Title))
                {
                    return titleAttribute.Title;
                }
                return Path.GetFileNameWithoutExtension(AssemblyUtils.AssemblyOrDefault().CodeBase);
            }
        }

        /// <summary>
        ///     Gets the internal name of the application assembly.
        /// </summary>
        /// <example>
        ///     <code>"bdhero-gui"</code>
        /// </example>
        public static string AssemblyName
        {
            get { return AssemblyUtils.GetAssemblyName(); }
        }

        /// <summary>
        ///     Gets the application's version number.
        /// </summary>
        public static Version AppVersion
        {
            get { return AssemblyUtils.GetAssemblyVersion(); }
        }

        /// <summary>
        ///     Gets the human-friendly description of the application.
        /// </summary>
        /// <example>
        ///     <code>"BDHero graphical interface"</code>
        /// </example>
        public static string AppDescription
        {
            get { return GetAttributeValue<AssemblyDescriptionAttribute>(attr => attr.Description); }
        }

        /// <summary>
        ///     Gets the product name of the application.
        /// </summary>
        /// <example>
        ///     <code>"BDHero"</code>
        /// </example>
        public static string ProductName
        {
            get { return GetAttributeValue<AssemblyProductAttribute>(attr => attr.Product); }
        }

        /// <summary>
        ///     Gets the application's copyright string.
        /// </summary>
        /// <example>
        ///     <code>"Copyright © 2013"</code>
        /// </example>
        public static string Copyright
        {
            get { return GetAttributeValue<AssemblyCopyrightAttribute>(attr => attr.Copyright); }
        }

        /// <summary>
        ///     Gets the name of the company that developed the application.
        /// </summary>
        /// <example>
        ///     <code>"BDHero"</code>
        /// </example>
        public static string Company
        {
            get { return GetAttributeValue<AssemblyCompanyAttribute>(attr => attr.Company); }
        }

        /// <summary>
        ///     Gets the date and time the application was built.
        /// </summary>
        public static DateTime BuildDate
        {
            get { return AssemblyUtils.GetLinkerTimestamp(); }
        }

        /// <summary>
        ///     Gets the first assembly attribute of type <typeparamref name="T" /> in the entry assembly.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the assembly attribute to get.
        ///     E.G., <see cref="AssemblyTitleAttribute" />, <see cref="AssemblyDescriptionAttribute" />.
        /// </typeparam>
        /// <returns>
        ///     The first assembly attribute of type <typeparamref name="T" /> in the entry assembly, or <c>null</c>
        ///     if the entry assembly does not contain an attribute of type <typeparamref name="T" />.
        /// </returns>
        /// <example>
        ///     <code>
        /// AssemblyTitleAttribute titleAttr = GetAttribute&lt;AssemblyTitleAttribute&gt;();
        /// </code>
        /// </example>
        private static T GetAttribute<T>() where T : Attribute
        {
            return AssemblyUtils.AssemblyOrDefault().GetCustomAttributes(typeof (T), false).FirstOrDefault() as T;
        }

        /// <summary>
        ///     Gets the <b>value</b> of the first assembly attribute of type <typeparamref name="T" /> in the
        ///     entry assembly by evaluating the given <paramref name="expr" /> against the attribute (if it exists).
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the assembly attribute to get.
        ///     E.G., <see cref="AssemblyTitleAttribute" />, <see cref="AssemblyDescriptionAttribute" />.
        /// </typeparam>
        /// <param name="expr">
        ///     An expression that takes the first entry assembly attribute of type <typeparamref name="T" />
        ///     and returns a <see cref="String"/> containing its value.  The argument passed to
        ///     <paramref name="expr"/> will <b>never</b> be null.
        /// </param>
        /// <returns>
        ///     The value of the first assembly attribute of type <typeparamref name="T" /> in the entry assembly,
        ///     or an empty string (<c>""</c>) if the entry assembly does not contain an attribute of type
        ///     <typeparamref name="T" />.
        /// </returns>
        /// <example>
        ///     <code>
        /// AssemblyTitleAttribute titleAttr = GetAttribute&lt;AssemblyTitleAttribute&gt;();
        /// </code>
        /// </example>
        private static string GetAttributeValue<T>(Expression<Func<T, string>> expr) where T : Attribute
        {
            var attr = GetAttribute<T>();
            return attr != null ? expr.Compile().Invoke(attr) : "";
        }

        #endregion
    }
}
