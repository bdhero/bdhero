using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetUtils.Concurrency;
using NUnit.Framework;

namespace DotNetUtilsUnitTests.Concurrency
{
    [TestFixture]
    public class ConcurrentMultiValueDictionaryTest
    {
        [Test]
        public void TestEmpty()
        {
            var dictionary = new ConcurrentMultiValueDictionary<int, string>();
            String value;
            Assert.IsFalse(dictionary.TryDequeue(1, out value));
        }

        [Test]
        public void TestEnqueue()
        {
            var dictionary = new ConcurrentMultiValueDictionary<int, string>();

            dictionary.Enqueue(1, "a");
            dictionary.Enqueue(1, "b");
            dictionary.Enqueue(1, "c");

            dictionary.Enqueue(2, "d");
            dictionary.Enqueue(2, "e");
            dictionary.Enqueue(2, "f");

            List<string> values1 = dictionary.GetValues(1);
            List<string> values2 = dictionary.GetValues(2);

            Assert.AreEqual(values1.Count, 3);
            Assert.AreEqual(values1[0], "a");
            Assert.AreEqual(values1[1], "b");
            Assert.AreEqual(values1[2], "c");

            Assert.AreEqual(values2.Count, 3);
            Assert.AreEqual(values2[0], "d");
            Assert.AreEqual(values2[1], "e");
            Assert.AreEqual(values2[2], "f");
        }

        [Test]
        public void TestDequeue()
        {
            var dictionary = new ConcurrentMultiValueDictionary<int, string>();

            dictionary.Enqueue(1, "a");
            dictionary.Enqueue(1, "b");
            dictionary.Enqueue(1, "c");

            String value;

            Assert.AreEqual(dictionary.GetValues(1).Count, 3);

            Assert.IsTrue(dictionary.TryDequeue(1, out value));
            Assert.AreEqual(value, "a");

            Assert.AreEqual(dictionary.GetValues(1).Count, 2);

            Assert.IsTrue(dictionary.TryDequeue(1, out value));
            Assert.AreEqual(value, "b");

            Assert.AreEqual(dictionary.GetValues(1).Count, 1);

            Assert.IsTrue(dictionary.TryDequeue(1, out value));
            Assert.AreEqual(value, "c");

            Assert.AreEqual(dictionary.GetValues(1).Count, 0);

            Assert.IsFalse(dictionary.TryDequeue(1, out value));
            Assert.IsNull(value);

            Assert.IsFalse(dictionary.TryDequeue(99, out value));
            Assert.IsNull(value);
        }

        [Test]
        public void TestGetKeys()
        {
            var dictionary = new ConcurrentMultiValueDictionary<int, string>();

            dictionary.Enqueue(4, "d");
            dictionary.Enqueue(4, "e");
            dictionary.Enqueue(4, "f");

            Assert.AreEqual(dictionary.GetKeys().Count, 1);
            Assert.AreEqual(dictionary.GetKeys()[0], 4);

            dictionary.Enqueue(5, "g");
            dictionary.Enqueue(5, "h");
            dictionary.Enqueue(5, "i");

            Assert.AreEqual(dictionary.GetKeys().Count, 2);
            Assert.AreEqual(dictionary.GetKeys()[0], 4);
            Assert.AreEqual(dictionary.GetKeys()[1], 5);
        }
    }
}
