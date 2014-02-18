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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DotNetUtils.Annotations;

namespace DotNetUtils.FS
{
    /// <summary>
    ///     Utility methods for working with the filesystem.
    /// </summary>
    public static class FileUtils
    {
        #region Read

        /// <summary>
        ///     Reads an entire stream into memory and returns it as an array of bytes.
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/a/6586039/467582" />
        public static byte[] ReadStream(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Creates an <see cref="Image" /> object without locking the source file.
        /// </summary>
        /// <param name="path">
        ///     Path to the image file.
        /// </param>
        /// <returns>
        ///     An image object.
        /// </returns>
        /// <seealso cref="http://stackoverflow.com/a/1105330/467582" />
        public static Image ImageFromFile(string path)
        {
            return Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
        }

        #endregion

        #region Detect encoding

        /// <summary>
        ///     Detects the encoding of the file using .NET's <see cref="StreamReader"/> class.
        /// </summary>
        /// <param name="filePath">
        ///     Relative or absolute path to a text file.
        /// </param>
        /// <param name="contents">
        ///     Output string with the discovered encoding applied to the file.
        /// </param>
        /// <returns>
        ///     The detected encoding of the text file specified by <paramref name="filePath" />.
        /// </returns>
        /// <seealso cref="http://stackoverflow.com/a/8935635/467582"/>
        public static Encoding DetectEncodingAuto(string filePath, out string contents)
        {
            // open the file with the stream-reader:
            using (var reader = new StreamReader(filePath, true))
            {
                // read the contents of the file into a string
                contents = reader.ReadToEnd();

                // return the encoding.
                return reader.CurrentEncoding;
            }
        }

        /// <summary>
        ///     Detects the encoding of text files.
        ///     Able to detect UTF-7, UTF-8/16/32 (BOM, no BOM, little &amp; big endian),
        ///     local default codepage, and potentially other codepages.
        /// </summary>
        /// <param name="filePath">
        ///     Relative or absolute path to a text file.
        /// </param>
        /// <param name="contents">
        ///     Output string with the discovered encoding applied to the file.
        /// </param>
        /// <param name="taster">
        ///     Number of bytes to check in the file (to save processing).
        ///     Higher values are slower, but more reliable (especially UTF-8 with special characters later on
        ///     may appear to be ASCII initially).
        ///     If <paramref name="taster" /> = 0, then <paramref name="taster" /> becomes the length of the file
        ///     (for maximum reliability).
        /// </param>
        /// <returns>
        ///     The detected encoding of the text file specified by <paramref name="filePath" />.
        /// </returns>
        /// <seealso cref="http://stackoverflow.com/a/12853721/467582" />
        public static Encoding DetectEncodingManual(string filePath, out string contents, int taster = 1000)
        {
            byte[] b = File.ReadAllBytes(filePath);

            //////////////// First check the low hanging fruit by checking if a
            //////////////// BOM/signature exists (sourced from http://www.unicode.org/faq/utf_bom.html#bom4)
            if (b.Length >= 4 && b[0] == 0x00 && b[1] == 0x00 && b[2] == 0xFE && b[3] == 0xFF) { contents = Encoding.GetEncoding("utf-32BE").GetString(b, 4, b.Length - 4); return Encoding.GetEncoding("utf-32BE"); }  // UTF-32, big-endian 
            if (b.Length >= 4 && b[0] == 0xFF && b[1] == 0xFE && b[2] == 0x00 && b[3] == 0x00) { contents = Encoding.UTF32.GetString(b, 4, b.Length - 4); return Encoding.UTF32; }    // UTF-32, little-endian
            if (b.Length >= 2 && b[0] == 0xFE && b[1] == 0xFF) { contents = Encoding.BigEndianUnicode.GetString(b, 2, b.Length - 2); return Encoding.BigEndianUnicode; }     // UTF-16, big-endian
            if (b.Length >= 2 && b[0] == 0xFF && b[1] == 0xFE) { contents = Encoding.Unicode.GetString(b, 2, b.Length - 2); return Encoding.Unicode; }              // UTF-16, little-endian
            if (b.Length >= 3 && b[0] == 0xEF && b[1] == 0xBB && b[2] == 0xBF) { contents = Encoding.UTF8.GetString(b, 3, b.Length - 3); return Encoding.UTF8; } // UTF-8
            if (b.Length >= 3 && b[0] == 0x2b && b[1] == 0x2f && b[2] == 0x76) { contents = Encoding.UTF7.GetString(b, 3, b.Length - 3); return Encoding.UTF7; } // UTF-7

            //////////// If the code reaches here, no BOM/signature was found, so now
            //////////// we need to 'taste' the file to see if can manually discover
            //////////// the encoding. A high taster value is desired for UTF-8
            if (taster == 0 || taster > b.Length) taster = b.Length;    // Taster size can't be bigger than the filesize obviously.

            // Some text files are encoded in UTF8, but have no BOM/signature. Hence
            // the below manually checks for a UTF8 pattern. This code is based off
            // the top answer at: http://stackoverflow.com/questions/6555015/check-for-invalid-utf8
            // For our purposes, an unnecessarily strict (and terser/slower)
            // implementation is shown at: http://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c
            // For the below, false positives should be exceedingly rare (and would
            // be either slightly malformed UTF-8 (which would suit our purposes
            // anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot).
            int i = 0;
            bool utf8 = false;
            while (i < taster - 4)
            {
                if (b[i] <= 0x7F) { i += 1; continue; }     // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required' (and therefore the text is more desirable to be treated as the default codepage of the computer). Hence, there's no "utf8 = true;" code unlike the next three checks.
                if (b[i] >= 0xC2 && b[i] <= 0xDF && b[i + 1] >= 0x80 && b[i + 1] < 0xC0) { i += 2; utf8 = true; continue; }
                if (b[i] >= 0xE0 && b[i] <= 0xF0 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0) { i += 3; utf8 = true; continue; }
                if (b[i] >= 0xF0 && b[i] <= 0xF4 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0 && b[i + 3] >= 0x80 && b[i + 3] < 0xC0) { i += 4; utf8 = true; continue; }
                utf8 = false; break;
            }
            if (utf8)
            {
                contents = Encoding.UTF8.GetString(b);
                return Encoding.UTF8;
            }

            // The next check is a heuristic attempt to detect UTF-16 without a BOM.
            // We simply look for zeroes in odd or even byte places, and if a certain
            // threshold is reached, the code is 'probably' UF-16.
            const double threshold = 0.1; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
            int count = 0;
            for (int n = 0; n < taster; n += 2) if (b[n] == 0) count++;
            if (((double)count) / taster > threshold) { contents = Encoding.BigEndianUnicode.GetString(b); return Encoding.BigEndianUnicode; }
            count = 0;
            for (int n = 1; n < taster; n += 2) if (b[n] == 0) count++;
            if (((double)count) / taster > threshold) { contents = Encoding.Unicode.GetString(b); return Encoding.Unicode; } // (little-endian)

            // Finally, a long shot - let's see if we can find "charset=xyz" or
            // "encoding=xyz" to identify the encoding:
            for (int n = 0; n < taster - 9; n++)
            {
                if (
                    ((b[n + 0] == 'c' || b[n + 0] == 'C') && (b[n + 1] == 'h' || b[n + 1] == 'H') && (b[n + 2] == 'a' || b[n + 2] == 'A') && (b[n + 3] == 'r' || b[n + 3] == 'R') && (b[n + 4] == 's' || b[n + 4] == 'S') && (b[n + 5] == 'e' || b[n + 5] == 'E') && (b[n + 6] == 't' || b[n + 6] == 'T') && (b[n + 7] == '=')) ||
                    ((b[n + 0] == 'e' || b[n + 0] == 'E') && (b[n + 1] == 'n' || b[n + 1] == 'N') && (b[n + 2] == 'c' || b[n + 2] == 'C') && (b[n + 3] == 'o' || b[n + 3] == 'O') && (b[n + 4] == 'd' || b[n + 4] == 'D') && (b[n + 5] == 'i' || b[n + 5] == 'I') && (b[n + 6] == 'n' || b[n + 6] == 'N') && (b[n + 7] == 'g' || b[n + 7] == 'G') && (b[n + 8] == '='))
                    )
                {
                    if (b[n + 0] == 'c' || b[n + 0] == 'C') n += 8; else n += 9;
                    if (b[n] == '"' || b[n] == '\'') n++;
                    int oldn = n;
                    while (n < taster && (b[n] == '_' || b[n] == '-' || (b[n] >= '0' && b[n] <= '9') || (b[n] >= 'a' && b[n] <= 'z') || (b[n] >= 'A' && b[n] <= 'Z')))
                    { n++; }
                    byte[] nb = new byte[n - oldn];
                    Array.Copy(b, oldn, nb, 0, n - oldn);
                    try
                    {
                        string internalEnc = Encoding.ASCII.GetString(nb);
                        contents = Encoding.GetEncoding(internalEnc).GetString(b);
                        return Encoding.GetEncoding(internalEnc);
                    }
                    catch { break; }    // If C# doesn't recognize the name of the encoding, break.
                }
            }

            // If all else fails, the encoding is probably (though certainly not
            // definitely) the user's local codepage! One might present to the user a
            // list of alternative encodings as shown here: http://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
            // A full list can be found using Encoding.GetEncodings();
            contents = Encoding.Default.GetString(b);
            return Encoding.Default;
        }

        #endregion

        #region Create

        /// <summary>
        ///     Creates all directories in the specified path hierarchy if they do not already exist.
        ///     If the path contains a file name with an extension, the file's parent directory will be created.
        /// </summary>
        /// <param name="path">
        ///     Path to a file or directory.
        /// </param>
        public static void CreateDirectory(string path)
        {
            // TODO: Does this check make sense?
            // Path will resolve to current working directory
            if (string.IsNullOrEmpty(path))
                return;

            // More accurate checks first
            if (File.Exists(path) || Directory.Exists(path))
                return;

            var dirPath = path;

            if (ContainsFileName(dirPath))
            {
                dirPath = Path.GetDirectoryName(dirPath);
            }

            if (!string.IsNullOrEmpty(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        ///     Creates and returns a path to a random temporary directory
        /// </summary>
        /// <returns>
        ///     Path to a random temporary directory.
        /// </returns>
        public static string CreateTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        /// <summary>
        ///     .NET implementation of the Unix <c>touch</c> command.
        ///     Updates the last write time of <paramref name="filePath"/>.
        ///     If <paramref name="filePath"/> does not already exist, it will be created along with all of its parent
        ///     directories.  This method does not alter the contents of <paramref name="filePath"/> in any way.
        /// </summary>
        /// <param name="filePath">Path to the file to touch.</param>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required permission.
        ///     -or-
        ///     <paramref name="filePath"/> specified a file that is read-only.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="filePath"/> is a zero-length string, contains only white space, or contains one or more
        ///     invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     The specified <paramref name="filePath"/>, file name, or both exceed the system-defined maximum length.
        ///     For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be
        ///     less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified <paramref name="filePath"/> is invalid (for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occurred while creating the file.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     <paramref name="filePath"/> is in an invalid format.
        /// </exception>
        public static void TouchFile(string filePath)
        {
            var dirPath = new FileInfo(filePath).DirectoryName;
            if (dirPath == null)
            {
                throw new DirectoryNotFoundException(string.Format("File \"{0}\" has no parent directory", filePath));
            }
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            File.SetLastWriteTimeUtc(filePath, DateTime.UtcNow);
        }

        #endregion

        #region Format file size

        /// <summary>
        ///     Generates a human-friendly file size string (e.g., <c>"2.1 GiB"</c>).
        /// </summary>
        /// <param name="byteCount">File size in bytes.</param>
        /// <returns>
        ///     Human-friendly string representation of the file size specified by <paramref name="byteCount"/>.
        /// </returns>
        public static string HumanFriendlyFileSize(long byteCount)
        {
            return HumanFriendlyFileSize((ulong) byteCount);
        }

        /// <summary>
        ///     Generates a human-friendly file size string (e.g., <c>"2.1 GiB"</c>).
        /// </summary>
        /// <param name="byteCount">File size in bytes.</param>
        /// <returns>
        ///     Human-friendly string representation of the file size specified by <paramref name="byteCount"/>.
        /// </returns>
        /// <seealso cref="http://stackoverflow.com/a/4975942/467582"/>
        public static string HumanFriendlyFileSize(ulong byteCount)
        {
            string[] suf = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" }; // Longs run out around EB
            if (byteCount == 0)
                return "0 " + suf[0];
            int place = Convert.ToInt32(Math.Floor(Math.Log(byteCount, 1024)));
            double num = Math.Round(byteCount / Math.Pow(1024, place), 1);
            string fmt = place == 0 ? "0" : "0.0";
            return string.Format("{0} {1}", num.ToString(fmt), suf[place]);
        }

        public static string FormatFileSize(long filesize, ushort maxFractionalDigits = 1)
        {
            string[] sizes = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" };
            double len = filesize;
            var order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            var fractionalFormat = maxFractionalDigits > 0 ? "." + new string('#', maxFractionalDigits) : "";

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0" + fractionalFormat + "} {1}", len, sizes[order]);
        }

        #endregion

        #region Tests

        /// <summary>
        ///     Determines if the given <paramref name="path"/> is a file.
        /// </summary>
        /// <param name="path">
        ///     Relative or absolute path to a file or directory.
        /// </param>
        /// <returns>
        ///     <c>true</c> if <paramref name="path"/> is a file; otherwise <c>false</c>.
        /// </returns>
        public static bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        /// <summary>
        ///     Determines if the given <paramref name="path"/> is a directory.
        /// </summary>
        /// <param name="path">
        ///     Relative or absolute path to a file or directory.
        /// </param>
        /// <returns>
        ///     <c>true</c> if <paramref name="path"/> is a directory; otherwise <c>false</c>.
        /// </returns>
        public static bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }

        /// <summary>
        ///     Determines whether the given path <em>likely</em> contains (ends with) a standard Windows filename (i.e., one that
        ///     has an extension).
        /// </summary>
        /// <param name="path">Relative or absolute path to a file or directory</param>
        /// <returns><c>true</c> if the path ends in a period followed by at least one letter; otherwise <c>false</c></returns>
        /// <example>
        ///     <code>
        /// "C:\some\dir\a.out" => true
        /// "a.out" => true
        /// "file.c" => true
        /// "file.php3" => true
        /// "file.3" => true
        /// "file" => false
        /// "C:\some\dir\file" => false
        /// "C:\some\dir" => false
        /// "C:\" => false
        /// "" => false
        /// </code>
        /// </example>
        public static bool ContainsFileName([NotNull] string path)
        {
            return new Regex(@"[^/\\]\.\w+$").IsMatch(path);
        }

        /// <summary>
        ///     Determines if the given <paramref name="fileName"/> is a valid filename under the current operating system.
        /// </summary>
        /// <param name="fileName">
        ///     Filename to validate.
        /// </param>
        /// <returns>
        ///     <c>true</c> if <paramref name="fileName"/> is a valid file name; otherwise <c>false</c>.
        /// </returns>
        /// <seealso cref="http://stackoverflow.com/questions/62771/how-check-if-given-string-is-legal-allowed-file-name-under-windows"/>
        public static bool IsValidFilename(string fileName)
        {
            var containsABadCharacter = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
            return !containsABadCharacter.IsMatch(fileName);
        }

        /// <summary>
        ///     Determines if the specified <paramref name="path" /> ends with at least one of the given
        ///     <paramref name="extensions" />.
        /// </summary>
        /// <param name="path">
        ///     Relative or absolute path to a file or directory.
        /// </param>
        /// <param name="extensions">
        ///     Zero or more file extensions (will be automatically normalized).
        /// </param>
        /// <returns>
        ///     <c>true</c> if the specified <paramref name="path" /> ends with at least one of the given
        ///     <paramref name="extensions" />; otherwise <c>false</c>.
        /// </returns>
        public static bool FileHasExtension(string path, IEnumerable<string> extensions)
        {
            var extension = Path.GetExtension(path);
            var normalized = NormalizeFileExtensions(extensions);
            return extension != null && normalized.Contains(NormalizeFileExtension(extension));
        }

        /// <summary>
        ///     Determines if the given <paramref name="directory"/> contains any files or subdirectories.
        /// </summary>
        /// <param name="directory">
        ///     Parent directory to test.
        /// </param>
        /// <returns>
        ///     <c>true</c> if <paramref name="directory"/> contains at least one file or subdirectory;
        ///     otherwise <c>false</c>.
        /// </returns>
        public static bool IsEmpty(DirectoryInfo directory)
        {
            return directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0;
        }

        #endregion

        #region Validate / normalize / sanitize

        /// <summary>
        ///     Sanitizes the given <paramref name="fileName"/> by removing all invalid characters.
        /// </summary>
        /// <param name="fileName">
        ///     Filename to sanitize.
        /// </param>
        /// <returns>
        ///     <paramref name="fileName"/> with all invalid characters removed.
        /// </returns>
        public static string SanitizeFileName(string fileName)
        {
            var badCharRegex = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]+");
            var multiBadCharRegex = new Regex(@"-(?:\s*-)+");
            var multiSpaceRegex = new Regex(@"\s{2,}");

            var sanitizedFileName = fileName;
            sanitizedFileName = badCharRegex.Replace(sanitizedFileName, " - ");
            sanitizedFileName = multiBadCharRegex.Replace(sanitizedFileName, "-");
            sanitizedFileName = multiSpaceRegex.Replace(sanitizedFileName, " ");

            return sanitizedFileName;
        }

        /// <summary>
        ///     Converts a file extension to lowercase and ensures that it starts with a period.
        /// </summary>
        /// <example>
        ///     <code>
        /// "ext" => ".ext"
        /// "EXT" => ".ext"
        /// ".ext" => ".ext"
        /// ".EXT" => ".ext"
        /// </code>
        /// </example>
        /// <param name="ext">
        ///     File extension, with or without a leading period, in any mix of upper or lower case.
        /// </param>
        /// <returns>
        ///     <paramref name="ext" /> normalized to lowercase with a leading period.
        /// </returns>
        public static string NormalizeFileExtension([NotNull] string ext)
        {
            // Make sure every extension starts with a period (e.g., ".ext")
            var extNorm = ext.Trim().ToLower();
            if (!extNorm.StartsWith("."))
                extNorm = "." + extNorm;
            return extNorm;
        }

        /// <summary>
        ///     Converts a collection of file extensions to lowercase and ensures that they start with a period.
        /// </summary>
        /// <example>
        ///     <code>
        /// "ext" => ".ext"
        /// "EXT" => ".ext"
        /// ".ext" => ".ext"
        /// ".EXT" => ".ext"
        /// </code>
        /// </example>
        /// <param name="extensions">
        ///     File extensions, with or without leading periods, in any mix of upper or lower case.
        /// </param>
        /// <returns>
        ///     <paramref name="extensions" /> normalized to lowercase with leading periods.
        /// </returns>
        public static ICollection<string> NormalizeFileExtensions(IEnumerable<string> extensions)
        {
            return extensions.Select(NormalizeFileExtension).ToList();
        }

        #endregion

        #region Open file/folder/URL

        /// <summary>
        ///     Opens the given file in its default program as if the user had double-clicked on it in Windows Explorer.
        /// </summary>
        /// <param name="filePath">
        ///     Relative or absolute path to a file to open.
        /// </param>
        /// <param name="control">
        ///     Optional Windows form control.  If specified and the given <paramref name="filePath" /> does not exist,
        ///     a <see cref="MessageBox" /> will be displayed as a modal window with <paramref name="control" />'s parent form as
        ///     the owner.
        /// </param>
        public static void OpenFile([NotNull] string filePath, [CanBeNull] Control control = null)
        {
            if (!EnsureExists(new FileInfo(filePath), control))
                return;

#if __MonoCS__
            OpenUtility(filePath);
#else
            Process.Start(filePath);
#endif
        }

        /// <summary>
        ///     Opens the given file's parent folder and highlights (selects) the file in Windows Explorer.
        /// </summary>
        /// <param name="filePath">
        ///     Relative or absolute path to a file.
        /// </param>
        /// <param name="control">
        ///     Optional Windows form control.  If specified and the given <paramref name="filePath" /> does not exist,
        ///     a <see cref="MessageBox" /> will be displayed as a modal window with <paramref name="control" />'s parent form as
        ///     the owner.
        /// </param>
        public static void ShowInFolder([NotNull] string filePath, [CanBeNull] Control control = null)
        {
            if (!EnsureExists(new FileInfo(filePath), control))
                return;

#if !__MonoCS__
            OpenUtility("-R", filePath);
#else
            // combine the arguments together
            // it doesn't matter if there is a space after ','
            Process.Start("explorer.exe", "/select, \"" + filePath + "\"");
#endif
        }

        /// <summary>
        ///     Opens the given file's folder in Windows Explorer.
        /// </summary>
        /// <param name="folderPath">
        ///     Relative or absolute path to a folder.
        /// </param>
        /// <param name="control">
        ///     Optional Windows form control.  If specified and the given <paramref name="folderPath" /> does not exist,
        ///     a <see cref="MessageBox" /> will be displayed as a modal window with <paramref name="control" />'s parent form as
        ///     the owner.
        /// </param>
        public static void OpenFolder([NotNull] string folderPath, [CanBeNull] Control control = null)
        {
            if (!EnsureExists(new DirectoryInfo(folderPath), control))
                return;

#if __MonoCS__
            OpenUtility("-R", folderPath);
#else
            Process.Start(folderPath);
#endif
        }

        private static void OpenUtility(string filePath)
        {
            OpenUtility("", filePath);
        }

        private static void OpenUtility(string leadingArgs, string filePath)
        {
            // Abort if we're not on Mac OS X
            if (!File.Exists("/usr/bin/open")) return;

            // http://stackoverflow.com/a/2283716/467582
            var filePathEscaped = filePath.Replace("'", @"\'");
            Process.Start(new ProcessStartInfo("open", string.Format("{0} '{1}'", leadingArgs, filePathEscaped)) { UseShellExecute = false });
        }

        /// <summary>
        ///     Launches the given <paramref name="url"/> in the user's default Web browser.
        /// </summary>
        /// <param name="url">
        ///     Web address to launch.
        /// </param>
        public static void OpenUrl(string url)
        {
            Process.Start(url);
        }

        private static bool EnsureExists([NotNull] FileSystemInfo info, [CanBeNull] Control control = null)
        {
            if (!info.Exists && control != null)
            {
                var form = (control as Form) ?? control.FindForm();
                var type = (info is DirectoryInfo) ? "Directory" : "File";
                var message = string.Format("{0} \"{1}\" does not exist", type, info.FullName);
                var title = string.Format("{0} Not Found", type);
                MessageBox.Show(form, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return info.Exists;
        }

        #endregion
    }
}
