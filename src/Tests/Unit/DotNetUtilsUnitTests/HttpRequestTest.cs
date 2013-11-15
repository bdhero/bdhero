using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetUtils.Net;
using NUnit.Framework;

namespace DotNetUtilsUnitTests
{
    [TestFixture]
    public class HttpRequestTest
    {
        [Test]
        public void Test()
        {
            for (var i = 0; i < 10; i++)
            {
                var req = HttpRequest.BuildRequest(HttpRequestMethod.Get, "http://www.google.com/search?q=" + i);
                var html = HttpRequest.Get(req);
                Assert.IsNotNull(html);
                Assert.Greater(html.Length, 0);
            }
        }
    }
}
