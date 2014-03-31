using Newtonsoft.Json;
using NUnit.Framework;

namespace UpdaterTests
{
    [TestFixture]
    public class JsonNullTest
    {
        [Test]
        public void TestEmpty()
        {
            var parent = JsonConvert.DeserializeObject<JsonNullTestParentEmpty>("{}");
            Assert.IsNull(parent.Child);
        }

        [Test]
        public void TestPopulated()
        {
            var parent = JsonConvert.DeserializeObject<JsonNullTestParentPopulated>("{}");
            Assert.IsNotNull(parent.Child);
        }
    }

    class JsonNullTestParentEmpty
    {
        public JsonNullTestChild Child { get; set; }
    }

    class JsonNullTestParentPopulated
    {
        public JsonNullTestChild Child { get; set; }

        public JsonNullTestParentPopulated()
        {
            Child = new JsonNullTestChild { Name = "Test" };
        }
    }

    class JsonNullTestChild
    {
        public string Name { get; set; }
    }
}
