using LicenseUtils;
using NUnit.Framework;

namespace LicenseUtilsTests
{
    [TestFixture]
    public class LicenseUtilsTest
    {
        [Test]
        public void Test()
        {
            LicenseImporter.Import();
        }
    }
}
