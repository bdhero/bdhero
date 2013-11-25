using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetUtils.Net;
using NUnit.Framework;

namespace DotNetUtilsUnitTests
{
    /// <summary>
    /// This is a test class for FileDownloader and is intended
    /// to contain all FileDownloader Unit Tests
    /// </summary>
    [TestFixture]
    public class FileDownloaderUnitTest
    {
        /// <summary>
        ///A test for ContainsFileName
        ///</summary>
        [Test]
        public void Test()
        {
            const string uri = "http://www.gravatar.com/avatar/77a0d71856a3815b527de22e233a8827.png";
            var path = Environment.ExpandEnvironmentVariables(Path.Combine("%TEMP%", "bdhero_gui_80.png"));
            var downloader = new FileDownloader
                {
                    Uri = uri,
                    Path = path
                };
            downloader.DownloadSync();

            const string expected = "e770bb640d0d97fcb29c0e7ee25844a6c302301f";
            var actual = Hash(path);

            Assert.AreEqual(expected, actual, "Downloaded file's SHA-1 hash does not match the expected value");
        }

        [Test]
        public void TestContentLength()
        {
            const string uri = "http://www.gravatar.com/avatar/77a0d71856a3815b527de22e233a8827.png";
            var downloader = new FileDownloader
                {
                    Uri = uri
                };
            var actual = downloader.GetContentLength();
            const long expected = 5036;

            Assert.AreEqual(expected, actual, "Incorrect Content-Length value");
        }

        /// <summary>
        /// Computes the SHA-1 hash (lowercase) of the specified file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string Hash(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                var hash = sha1.ComputeHash(stream);
                var sb = new StringBuilder();
                for (var i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
