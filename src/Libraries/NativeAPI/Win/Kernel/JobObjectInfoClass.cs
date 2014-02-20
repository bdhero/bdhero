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

namespace NativeAPI.Win.Kernel
{
    /// <summary>
    ///     Information class for the limits to be set. This parameter can be one of
    ///     the following values.
    /// </summary>
    public enum JobObjectInfoClass
    {
        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_ACCOUNTING_INFORMATION structure.
        /// </summary>
        BasicAccountingInformation = 1,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        BasicLimitInformation = 2,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_PROCESS_ID_LIST structure.
        /// </summary>
        BasicProcessIdList = 3,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_UI_RESTRICTIONS structure.
        /// </summary>
        BasicUIRestrictions = 4,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_SECURITY_LIMIT_INFORMATION structure.
        ///     The hJob handle must have the JOB_OBJECT_SET_SECURITY_ATTRIBUTES
        ///     access right associated with it.
        /// </summary>
        SecurityLimitInformation = 5,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_END_OF_JOB_TIME_INFORMATION structure.
        /// </summary>
        EndOfJobTimeInformation = 6,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_ASSOCIATE_COMPLETION_PORT structure.
        /// </summary>
        AssociateCompletionPortInformation = 7,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_AND_IO_ACCOUNTING_INFORMATION structure.
        /// </summary>
        BasicAndIoAccountingInformation = 8,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure.
        /// </summary>
        ExtendedLimitInformation = 9
    }
}