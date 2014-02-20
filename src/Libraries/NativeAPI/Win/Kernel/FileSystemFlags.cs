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

// ReSharper disable InconsistentNaming
namespace NativeAPI.Win.Kernel
{
    /// <summary>
    ///     Flags associated with a file system.
    ///     FS_FILE_COMPRESSION and FS_VOL_IS_COMPRESSED are mutually exclusive.
    /// </summary>
    [Flags]
    public enum FileSystemFlags : uint
    {
        /// <summary>
        ///     Null.
        /// </summary>
        NULL = 0x00000000,

        /// <summary>
        ///     The specified volume supports preserved case of file names when it places a name on disk.
        /// </summary>
        FILE_CASE_PRESERVED_NAMES = 0x00000002,

        /// <summary>
        ///     The specified volume supports case-sensitive file names.
        /// </summary>
        FILE_CASE_SENSITIVE_SEARCH = 0x00000001,

        /// <summary>
        ///     The specified volume supports file-based compression.
        /// </summary>
        FILE_FILE_COMPRESSION = 0x00000010,

        /// <summary>
        ///     The specified volume supports named streams.
        /// </summary>
        FILE_NAMED_STREAMS = 0x00040000,

        /// <summary>
        ///     The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.
        /// </summary>
        FILE_PERSISTENT_ACLS = 0x00000008,

        /// <summary>
        ///     The specified volume is read-only.
        /// </summary>
        FILE_READ_ONLY_VOLUME = 0x00080000,

        /// <summary>
        ///     The specified volume supports a single sequential write.
        /// </summary>
        FILE_SEQUENTIAL_WRITE_ONCE = 0x00100000,

        /// <summary>
        ///     The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.
        /// </summary>
        FILE_SUPPORTS_ENCRYPTION = 0x00020000,

        /// <summary>
        ///     <para>
        ///         The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can associate with a file and is not part of the file's data.
        ///     </para>
        ///     <para>
        ///         Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported until Windows Server 2008 R2 and Windows 7.
        ///     </para>
        /// </summary>
        FILE_SUPPORTS_EXTENDED_ATTRIBUTES = 0x00800000,

        /// <summary>
        ///     <para>
        ///         The specified volume supports hard links. For more information, see Hard Links and Junctions.
        ///     </para>
        ///     <para>
        ///         Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported until Windows Server 2008 R2 and Windows 7.
        ///     </para>
        /// </summary>
        FILE_SUPPORTS_HARD_LINKS = 0x00400000,

        /// <summary>
        ///     The specified volume supports object identifiers.
        /// </summary>
        FILE_SUPPORTS_OBJECT_IDS = 0x00010000,

        /// <summary>
        ///     <para>
        ///         The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
        ///     </para>
        ///     <para>
        ///         Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported until Windows Server 2008 R2 and Windows 7.
        ///     </para>
        /// </summary>
        FILE_SUPPORTS_OPEN_BY_FILE_ID = 0x01000000,

        /// <summary>
        ///     <para>
        ///         The specified volume supports reparse points.
        ///     </para>
        ///     <para>
        ///         ReFS:  ReFS supports reparse points but does not index them so FindFirstVolumeMountPoint and FindNextVolumeMountPoint will not function as expected.
        ///     </para>
        /// </summary>
        FILE_SUPPORTS_REPARSE_POINTS = 0x00000080,

        /// <summary>
        ///     The specified volume supports sparse files.
        /// </summary>
        FILE_SUPPORTS_SPARSE_FILES = 0x00000040,

        /// <summary>
        ///     The specified volume supports transactions. For more information, see About KTM.
        /// </summary>
        FILE_SUPPORTS_TRANSACTIONS = 0x00200000,

        /// <summary>
        ///     <para>
        ///         The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.
        ///     </para>
        ///     <para>
        ///         Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported until Windows Server 2008 R2 and Windows 7.
        ///     </para>
        /// </summary>
        FILE_SUPPORTS_USN_JOURNAL = 0x02000000,

        /// <summary>
        ///     The specified volume supports Unicode in file names as they appear on disk.
        /// </summary>
        FILE_UNICODE_ON_DISK = 0x00000004,

        /// <summary>
        ///     The specified volume is a compressed volume, for example, a DoubleSpace volume.
        /// </summary>
        FILE_VOLUME_IS_COMPRESSED = 0x00008000,

        /// <summary>
        ///     The specified volume supports disk quotas.
        /// </summary>
        FILE_VOLUME_QUOTAS = 0x00000020,
    }
}
