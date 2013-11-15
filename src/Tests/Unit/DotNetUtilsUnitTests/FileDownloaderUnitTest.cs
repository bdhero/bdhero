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
            const string uri = "https://raw.github.com/bdhero/bdhero/master/Assets/Icons/bdhero_gui_512.png";
            var path = Environment.ExpandEnvironmentVariables(@"%TEMP%\bdhero_gui_512.png");
            var downloader = new FileDownloader
                {
                    Uri = uri,
                    Path = path
                };
            downloader.DownloadSync();

            const string expected = "945ec6311f160967a797eecd9648a38b10a662ec";
            var actual = Hash(path);

            Assert.AreEqual(expected, actual, "Downloaded file's SHA-1 hash does not match the expected value");
        }

        [Test]
        public void TestContentLength()
        {
            const string uri = "https://raw.github.com/bdhero/bdhero/master/Assets/Icons/bdhero_gui_512.png";
            var downloader = new FileDownloader
                {
                    Uri = uri
                };
            var actual = downloader.GetContentLength();
            const long expected = 89485;

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
