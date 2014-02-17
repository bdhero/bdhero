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

namespace WinAPI.ShellLightWeight
{
    /// <summary>
    ///     Provides information to the IQueryAssociations interface methods.
    /// </summary>
    [Flags]
    public enum AssocF
    {
        /// <summary>
        ///     None of the following options are set.
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     Instructs IQueryAssociations interface methods not to map CLSID values to ProgID values.
        /// </summary>
        InitNoRemapCLSID = 0x1,

        /// <summary>
        ///     Identifies the value of the pwszAssoc parameter of IQueryAssociations::Init as an executable file name.
        ///     If this flag is not set, the root key will be set to the ProgID associated with the .exe key instead of
        ///     the executable file's ProgID.
        /// </summary>
        InitByExeName = 0x2,

        /// <summary>
        ///     Identical to ASSOCF_INIT_BYEXENAME.
        /// </summary>
        OpenByExeName = 0x2,

        /// <summary>
        ///     Specifies that when an IQueryAssociations method does not find the requested value under the root key,
        ///     it should attempt to retrieve the comparable value from the * subkey.
        /// </summary>
        InitDefaultToStar = 0x4,

        /// <summary>
        ///     Specifies that when a IQueryAssociations method does not find the requested value under the root key,
        ///     it should attempt to retrieve the comparable value from the Folder subkey.
        /// </summary>
        InitDefaultToFolder = 0x8,

        /// <summary>
        ///     Specifies that only HKEY_CLASSES_ROOT should be searched, and that HKEY_CURRENT_USER should be ignored.
        /// </summary>
        NoUserSettings = 0x10,

        /// <summary>
        ///     Specifies that the return string should not be truncated. Instead, return an error value and the required size for the complete string.
        /// </summary>
        NoTruncate = 0x20,

        /// <summary>
        ///     Instructs IQueryAssociations methods to verify that data is accurate. This setting allows IQueryAssociations methods to read data from the user's hard disk for verification. For example, they can check the friendly name in the registry against the one stored in the .exe file. Setting this flag typically reduces the efficiency of the method.
        /// </summary>
        Verify = 0x40,

        /// <summary>
        ///     Instructs IQueryAssociations methods to ignore Rundll.exe and return information about its target. Typically IQueryAssociations methods return information about the first .exe or .dll in a command string. If a command uses Rundll.exe, setting this flag tells the method to ignore Rundll.exe and return information about its target.
        /// </summary>
        RemapRunDll = 0x80,

        /// <summary>
        ///     Instructs IQueryAssociations methods not to fix errors in the registry, such as the friendly name of a function not matching the one found in the .exe file.
        /// </summary>
        NoFixUps = 0x100,

        /// <summary>
        ///     Specifies that the BaseClass value should be ignored.
        /// </summary>
        IgnoreBaseClass = 0x200,

        /// <summary>
        ///     Introduced in Windows 7. Specifies that the "Unknown" ProgID should be ignored; instead, fail.
        /// </summary>
        InitIgnoreunknown,

        /// <summary>
        ///     Introduced in Windows 8.
        /// </summary>
        InitFixedProgid,

        /// <summary>
        ///     Introduced in Windows 8.
        /// </summary>
        IsProtocol,

        /// <summary>
        ///     Introduced in Windows 8.1.
        /// </summary>
        InitForFile,
    }
}