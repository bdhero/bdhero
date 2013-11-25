using System.Reflection;
using System.IO;
using DotNetUtils.FS;
using NUnit.Framework;

namespace DotNetUtilsUnitTests
{
    /// <summary>
    /// This is a test class for FileUtilsTest and is intended
    /// to contain all FileUtilsTest Unit Tests
    /// </summary>
    [TestFixture]
    public class FileUtilsTest
    {
        /// <summary>
        ///A test for ContainsFileName
        ///</summary>
        [Test]
        public void ContainsFileNameTest()
        {
            Assert.IsTrue(FileUtils.ContainsFileName(@"C:\some\dir\a.out"));
            Assert.IsTrue(FileUtils.ContainsFileName(@"..\dir\a.out"));
            Assert.IsTrue(FileUtils.ContainsFileName(@"dir\a.out"));
            Assert.IsTrue(FileUtils.ContainsFileName(@"\a.out"));
            Assert.IsTrue(FileUtils.ContainsFileName(@"a.out"));
            Assert.IsTrue(FileUtils.ContainsFileName(@"file.c"));
            Assert.IsTrue(FileUtils.ContainsFileName(@"file.php3"));
            Assert.IsTrue(FileUtils.ContainsFileName(@"file.3"));

            Assert.IsFalse(FileUtils.ContainsFileName(@"C:\some\dir\aout"));
            Assert.IsFalse(FileUtils.ContainsFileName(@"..\dir\aout"));
            Assert.IsFalse(FileUtils.ContainsFileName(@"dir\aout"));
            Assert.IsFalse(FileUtils.ContainsFileName(@"\aout"));
            Assert.IsFalse(FileUtils.ContainsFileName(@"aout"));
            Assert.IsFalse(FileUtils.ContainsFileName(@"C:\"));
            Assert.IsFalse(FileUtils.ContainsFileName(@"C:"));
            Assert.IsFalse(FileUtils.ContainsFileName(@""));
        }

        /// <summary>
        ///A test for CreateDirectory
        ///</summary>
        [Test]
        public void CreateDirectoryTest()
        {
            var pwd = Directory.GetCurrentDirectory();
            var root = FileUtils.CreateTemporaryDirectory();
            Directory.SetCurrentDirectory(root);

            var dirs = new[] { @"a\b\c", @"d\e", @"f" };
            foreach (var dir in dirs)
            {
                FileUtils.CreateDirectory(dir);
                Assert.IsTrue(Directory.Exists(dir));
            }

            var files = new[] { @"a\b\c\d.txt", @"a\b\c.txt", @"a\b.txt" };
            foreach (var file in files)
            {
                FileUtils.CreateDirectory(file);
                Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(file)));
            }

            Directory.SetCurrentDirectory(pwd);
            Directory.Delete(root, true);
        }

        /// <summary>
        ///A test for SanitizeFileName
        ///</summary>
        [Test]
        public void SanitizeFileNameTest()
        {
            Assert.AreEqual("My File.txt", FileUtils.SanitizeFileName(@"My File.txt"));
            Assert.AreEqual("My - File.txt", FileUtils.SanitizeFileName(@"My: File.txt"));
            Assert.AreEqual("My - File.txt", FileUtils.SanitizeFileName(@"My: ? File.txt"));
            Assert.AreEqual("My - File.txt", FileUtils.SanitizeFileName(@"My/\:? File.txt"));
            Assert.AreEqual("My - File.txt", FileUtils.SanitizeFileName(@"My / \ : ? File.txt"));
            Assert.AreEqual("My - File.txt", FileUtils.SanitizeFileName(@"My / -- \ : -- ? File.txt"));
        }

        /// <summary>
        ///A test for IsValidFilename
        ///</summary>
        [Test]
        public void IsValidFilenameTest()
        {
            var filenameTemplates = new[] { "a{0}b.txt" };
            foreach (var tpl in filenameTemplates)
            {
                foreach (var ch in Path.GetInvalidFileNameChars())
                {
                    var goodFileName = string.Format(tpl, string.Empty);
                    var badFileName = string.Format(tpl, ch);
                    Assert.IsTrue(FileUtils.IsValidFilename(goodFileName));
                    Assert.IsFalse(FileUtils.IsValidFilename(badFileName));
                }
            }
        }

        /// <summary>
        ///A test for IsFile
        ///</summary>
        [Test]
        public void IsFileTest()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            Assert.True(FileUtils.IsFile(location));
            Assert.False(FileUtils.IsFile(Path.GetDirectoryName(location)));
        }

        /// <summary>
        ///A test for IsDirectory
        ///</summary>
        [Test]
        public void IsDirectoryTest()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            Assert.False(FileUtils.IsDirectory(location));
            Assert.True(FileUtils.IsDirectory(Path.GetDirectoryName(location)));
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [Test]
        public void IsEmptyTest()
        {
            var root = new DirectoryInfo(FileUtils.CreateTemporaryDirectory());
            Assert.True(FileUtils.IsEmpty(root));

            // Test directory with single subdirectory
            var dir = Directory.CreateDirectory(Path.Combine(root.FullName, "subdir"));
            Assert.False(FileUtils.IsEmpty(root));

            // Test directory with single file
            Assert.True(FileUtils.IsEmpty(dir));
            var file = new FileInfo(Path.Combine(dir.FullName, "file.txt"));
            File.WriteAllText(file.FullName, "test");
            Assert.False(FileUtils.IsEmpty(dir));

            Directory.Delete(root.FullName, true);
        }

        /// <summary>
        ///A test for NormalizeFileExtension
        ///</summary>
        [Test]
        public void NormalizeFileExtensionTest()
        {
            Assert.AreEqual(".ext", FileUtils.NormalizeFileExtension("ext"));
            Assert.AreEqual(".ext", FileUtils.NormalizeFileExtension("ExT"));
            Assert.AreEqual(".ext", FileUtils.NormalizeFileExtension("EXT"));
            Assert.AreEqual(".ext", FileUtils.NormalizeFileExtension(".ext"));
            Assert.AreEqual(".ext", FileUtils.NormalizeFileExtension(".ExT"));
            Assert.AreEqual(".ext", FileUtils.NormalizeFileExtension(".EXT"));
        }

        /// <summary>
        ///A test for NormalizeFileExtensions
        ///</summary>
        [Test]
        public void NormalizeFileExtensionsTest()
        {
            var input = new[] { "ext", "ExT", "EXT", ".ext", ".ExT", ".EXT", " .exT " };
            var expected = new[] { ".ext", ".ext", ".ext", ".ext", ".ext", ".ext", ".ext" };
            Assert.AreEqual(expected, FileUtils.NormalizeFileExtensions(input));
        }

        /// <summary>
        ///A test for FileHasExtension
        ///</summary>
        [Test]
        public void FileHasExtensionTest()
        {
            var inputExtensions = new[] { ".abc", ".DEF", "ghi" };
            Assert.True(FileUtils.FileHasExtension("test.abc", inputExtensions));
            Assert.True(FileUtils.FileHasExtension("test.def", inputExtensions));
            Assert.True(FileUtils.FileHasExtension("test.ghi", inputExtensions));
            Assert.False(FileUtils.FileHasExtension("test.xyz", inputExtensions));
        }

        /// <summary>
        ///A test for CreateTemporaryDirectory
        ///</summary>
        [Test]
        public void CreateTemporaryDirectoryTest()
        {
            var temporaryDirectory = FileUtils.CreateTemporaryDirectory();
            Assert.IsTrue(Directory.Exists(temporaryDirectory));
        }
    }
}
