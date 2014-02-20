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

using System.Runtime.InteropServices;
using System.Text;

namespace NativeAPI.Win.ShellLightWeight
{
    /// <summary>
    ///     Windows API functions for querying file and protocol association information from the registry.
    /// </summary>
    public static class AssociationAPI
    {
        /// <summary>
        ///     Searches for and retrieves a file or protocol association-related string from the registry.
        /// </summary>
        /// <param name="flags">
        ///     The flags that can be used to control the search. It can be any combination of <see cref="AssocF"/> values,
        ///     except that only one <c>ASSOCF_INIT</c> value can be included.
        /// </param>
        /// <param name="str">
        ///     The <see cref="AssocStr"/> value that specifies the type of string that is to be returned.
        /// </param>
        /// <param name="pszAssoc">
        ///     A pointer to a null-terminated string that is used to determine the root key. The following four types of strings can be used:
        ///     <list type="list">
        ///         <item><b>File name extension</b>: A file name extension, such as .txt.</item>
        ///         <item><b>CLSID</b>: A CLSID GUID in the standard "{GUID}" format.</item>
        ///         <item><b>ProgID</b>: An application's ProgID, such as Word.Document.8.</item>
        ///         <item><b>Executable name</b>: The name of an application's .exe file. The ASSOCF_OPEN_BYEXENAME flag must be set in flags.</item>
        ///     </list>
        /// </param>
        /// <param name="pszExtra">
        ///     An optional null-terminated string with additional information about the location of the string.
        ///     It is typically set to a Shell verb such as open. Set this parameter to NULL if it is not used.
        /// </param>
        /// <param name="pszOut">
        ///     Pointer to a null-terminated string that, when this function returns successfully, receives the requested string.
        ///     Set this parameter to NULL to retrieve the required buffer size.
        /// </param>
        /// <param name="pcchOut">
        ///     <para>
        ///         A pointer to a value that, when calling the function, is set to the number of characters in the pszOut buffer. When the function returns successfully, the value is set to the number of characters actually placed in the buffer.
        ///     </para>
        ///     <para>
        ///         If the ASSOCF_NOTRUNCATE flag is set in flags and the buffer specified in pszOut is too small, the function returns E_POINTER and the value is set to the required size of the buffer.
        ///     </para>
        ///     <para>
        ///         If pszOut is NULL, the function returns S_FALSE and pcchOut points to the required size, in characters, of the buffer.
        ///     </para>
        /// </param>
        /// <returns>
        ///     Returns a standard COM error value, including the following:
        ///     <list type="list">
        ///         <item><b>S_OK</b>: Success.</item>
        ///         <item><b>E_POINTER</b>: The pszOut buffer is too small to hold the entire string.</item>
        ///         <item><b>S_FALSE</b>: pszOut is NULL. pcchOut contains the required buffer size.</item>
        ///     </list>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This function is a wrapper for the IQueryAssociations interface. The AssocQueryString function is
        ///     intended to simplify the process of using IQueryAssociations interface.
        ///     </para>
        ///     <para>
        ///         Once an item is selected, the host must decide which (if any) preview handler is available for that
        ///         item. Preview handlers are typically registered on file name extensions or ProgID, but some preview
        ///         handlers are only instantiated for items within particular shell folders (the MAPI preview handler is
        ///         associated with any items that came from the MAPI Shell folder, for example). Thus, the host must use
        ///         IQueryAssociations to determine which preview handler to use. For further discussion of how the file
        ///         and protocol association functions work, see IQueryAssociations.
        ///     </para>
        /// </remarks>
        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, [In][Out] ref uint pcchOut);
    }
}
