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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NativeAPI;

namespace WinAPI.Kernel
{
    public static class VolumeAPI
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class Volume
        {
            public string Label;

            public FileSystem FileSystem;

            public uint SerialNumber;

            /// <summary>
            ///     The maximum length, in <c>TCHAR</c>s, of a file name component that a specified file system supports.
            /// </summary>
            public uint MaxFileNameLength;
        }

        public class FileSystem
        {
            public string Name;

            public FileSystemFlags Flags;
        }

//        [CanBeNull] // TODO
        public static Volume GetVolumeInformation(DirectoryInfo dir)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return null;

            uint serialNumber = 0;
            uint maxLength = 0;
            var volumeFlags = FileSystemFlags.NULL;
            var volumeLabel = new StringBuilder(256);
            var fileSystemName = new StringBuilder(256);

            try
            {
                PInvokeUtils.Try(() => GetVolumeInformation(dir.Root.FullName,
                                                            volumeLabel,
                                                            (uint) volumeLabel.Capacity,
                                                            ref serialNumber,
                                                            ref maxLength,
                                                            ref volumeFlags,
                                                            fileSystemName,
                                                            (uint) fileSystemName.Capacity));

                return new Volume
                       {
                           Label = volumeLabel.ToString(),
                           FileSystem = new FileSystem
                                        {
                                            Name = fileSystemName.ToString(),
                                            Flags = volumeFlags
                                        },
                           SerialNumber = serialNumber,
                           MaxFileNameLength = maxLength
                       };
            }
            catch (Win32Exception e)
            {
                Logger.Error("Invocation of native GetVolumeInformation() function threw a Win32 exception", e);
            }
            catch (DllNotFoundException e)
            {
                Logger.Warn("Unable to locate kernel32.dll - OS is probably not Windows", e);
            }
            catch (Exception e)
            {
                Logger.Error("GetVolumeInformation() threw an exception", e);
            }

            return null;
        }

        /// <summary>
        ///     <para>
        ///         Retrieves information about the file system and volume associated with the specified root directory.
        ///     </para>
        ///     <para>
        ///         To specify a handle when retrieving this information, use the <c>GetVolumeInformationByHandleW</c> function.
        ///     </para>
        ///     <para>
        ///         To retrieve the current compression state of a file or directory, use <c>FSCTL_GET_COMPRESSION</c>.
        ///     </para>
        /// </summary>
        /// <param name="lpRootPathName">
        ///     <para>
        ///         A pointer to a string that contains the root directory of the volume to be described.
        ///     </para>
        ///     <para>
        ///         If this parameter is <c>NULL</c>, the root of the current directory is used. A trailing backslash is required.
        ///         For example, you specify <c>\\MyServer\MyShare</c> as <c>"\\MyServer\MyShare\"</c>, or the C drive as <c>"C:\"</c>.
        ///     </para>
        /// </param>
        /// <param name="lpVolumeNameBuffer">
        ///     A pointer to a buffer that receives the name of a specified volume. The buffer size is specified by the
        ///     nVolumeNameSize parameter.
        /// </param>
        /// <param name="nVolumeNameSize">
        ///     <para>
        ///         The length of a volume name buffer, in <c>TCHAR</c>s. The maximum buffer size is <c>MAX_PATH+1</c>.
        ///     </para>
        ///     <para>
        ///         This parameter is ignored if the volume name buffer is not supplied.
        ///     </para>
        /// </param>
        /// <param name="lpVolumeSerialNumber">
        ///     <para>
        ///         A pointer to a variable that receives the volume serial number.
        ///     </para>
        ///     <para>
        ///         This parameter can be <c>NULL</c> if the serial number is not required.
        ///     </para>
        ///     <para>
        ///         This function returns the volume serial number that the operating system assigns when a hard disk
        ///         is formatted. To programmatically obtain the hard disk's serial number that the manufacturer assigns,
        ///         use the Windows Management Instrumentation (WMI) <c>Win32_PhysicalMedia</c> property <c>SerialNumber</c>.
        ///     </para>
        /// </param>
        /// <param name="lpMaximumComponentLength">
        ///     <para>
        ///         A pointer to a variable that receives the maximum length, in <c>TCHAR</c>s, of a file name component that
        ///         a specified file system supports.
        ///     </para>
        ///     <para>
        ///         A file name component is the portion of a file name between backslashes.
        ///     </para>
        ///     <para>
        ///         The value that is stored in the variable that <c>*lpMaximumComponentLength</c> points to is used to indicate
        ///         that a specified file system supports long names. For example, for a FAT file system that supports
        ///         long names, the function stores the value 255, rather than the previous 8.3 indicator. Long names
        ///         can also be supported on systems that use the NTFS file system.
        ///     </para>
        /// </param>
        /// <param name="lpFileSystemFlags">
        ///     <para>
        ///         A pointer to a variable that receives flags associated with the specified file system.
        ///     </para>
        ///     <para>
        ///         This parameter can be one or more of the following flags. However, <c>FS_FILE_COMPRESSION</c> and
        ///         <c>FS_VOL_IS_COMPRESSED</c> are mutually exclusive.
        ///     </para>
        /// </param>
        /// <param name="lpFileSystemNameBuffer">
        ///     A pointer to a buffer that receives the name of the file system, for example, the FAT file system or
        ///     the NTFS file system. The buffer size is specified by the nFileSystemNameSize parameter.
        /// </param>
        /// <param name="nFileSystemNameSize">
        ///     <para>
        ///         The length of the file system name buffer, in <c>TCHAR</c>s. The maximum buffer size is <c>MAX_PATH+1</c>.
        ///     </para>
        ///     <para>
        ///         This parameter is ignored if the file system name buffer is not supplied.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         If all the requested information is retrieved, the return value is nonzero.
        ///     </para>
        ///     <para>
        ///         If not all the requested information is retrieved, the return value is zero. To get extended error
        ///         information, call GetLastError.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When a user attempts to get information about a floppy drive that does not have a floppy disk, or
        ///         a CD-ROM drive that does not have a compact disc, the system displays a message box for the user to
        ///         insert a floppy disk or a compact disc, respectively. To prevent the system from displaying this
        ///         message box, call the <c>SetErrorMode</c> function with <c>SEM_FAILCRITICALERRORS</c>.
        ///     </para>
        ///     <para>
        ///         The <c>FS_VOL_IS_COMPRESSED</c> flag is the only indicator of volume-based compression. The file system
        ///         name is not altered to indicate compression, for example, this flag is returned set on a DoubleSpace
        ///         volume. When compression is volume-based, an entire volume is compressed or not compressed.
        ///     </para>
        ///     <para>
        ///         The <c>FS_FILE_COMPRESSION</c> flag indicates whether a file system supports file-based compression.
        ///         When compression is file-based, individual files can be compressed or not compressed.
        ///     </para>
        ///     <para>
        ///         The <c>FS_FILE_COMPRESSION</c> and <c>FS_VOL_IS_COMPRESSED</c> flags are mutually exclusive. Both bits cannot be
        ///         returned set.
        ///     </para>
        ///     <para>
        ///         The maximum component length value that is stored in lpMaximumComponentLength is the only indicator
        ///         that a volume supports longer-than-normal FAT file system (or other file system) file names.
        ///         The file system name is not altered to indicate support for long file names.
        ///     </para>
        ///     <para>
        ///         The <c>GetCompressedFileSize</c> function obtains the compressed size of a file. The <c>GetFileAttributes</c> function
        ///         can determine whether an individual file is compressed.
        ///     </para>
        ///     <para>
        ///         Symbolic link behavior—
        ///     </para>
        ///     <para>
        ///         If the path points to a symbolic link, the function returns volume information for the target.
        ///     </para>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetVolumeInformation(
            [In] string lpRootPathName,
            [Out] StringBuilder lpVolumeNameBuffer,
            [In] uint nVolumeNameSize,
            [In, Out] ref uint lpVolumeSerialNumber,
            [In, Out] ref uint lpMaximumComponentLength,
            [In, Out] [MarshalAs(UnmanagedType.U4)] ref FileSystemFlags lpFileSystemFlags,
            [Out] StringBuilder lpFileSystemNameBuffer,
            [In] uint nFileSystemNameSize);
    }
}
