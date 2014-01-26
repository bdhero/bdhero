using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            new LicenseImporter().Import();
        }
    }
}
