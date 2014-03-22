using BDHero.BDROM;
using IsanPlugin;
using NUnit.Framework;

namespace IsanPluginTests
{
    [TestFixture]
    public class IsanPluginTest
    {
        private IsanMetadataProvider _provider;

        // TODO: Test parsing

        [SetUp]
        public void SetUp()
        {
            _provider = new IsanMetadataProvider();
        }

        [Test]
        public void TestParseCheckedIsanWithNumericCheckDigits()
        {
            const string checkedNumber = "00000000E06600003000000009";

            var isan = Isan.TryParse(checkedNumber);

            Assert.IsNotNull(isan);
            Assert.AreEqual(isan.Root, "00000000E066");
            Assert.AreEqual(isan.Episode, "0000");
            Assert.AreEqual(isan.Version, "00000000");
        }

        [Test]
        public void TestParseCheckedIsanWithAlphaCheckDigits()
        {
            const string checkedNumber = "00000000E0660000A00000000S";

            var isan = Isan.TryParse(checkedNumber);

            Assert.IsNotNull(isan);
            Assert.AreEqual(isan.Root, "00000000E066");
            Assert.AreEqual(isan.Episode, "0000");
            Assert.AreEqual(isan.Version, "00000000");
        }

        [Test]
        public void TestParseUncheckedIsan()
        {
            const string uncheckedNumber = "00000000E0AA000000000001";

            var isan = Isan.TryParse(uncheckedNumber);

            Assert.IsNotNull(isan);
            Assert.AreEqual(isan.Root, "00000000E0AA");
            Assert.AreEqual(isan.Episode, "0000");
            Assert.AreEqual(isan.Version, "00000001");
        }

        [Test]
        public void TestPopulateEmptyTitle()
        {
            const string emptyTitleVIsan = "00000000E0AA000000000001";

            var vIsan = VIsan.TryParse(emptyTitleVIsan);

            _provider.Populate(vIsan);

            var parent = vIsan.Parent;

            Assert.IsNull(vIsan.Title);
            Assert.IsNull(vIsan.Year);
            Assert.IsNull(vIsan.LengthMin);
            Assert.IsNotNull(parent);
            Assert.AreEqual("Lord Of War", parent.Title);
            Assert.AreEqual(2004, parent.Year);
            Assert.AreEqual(100, parent.LengthMin);
        }
    }
}
