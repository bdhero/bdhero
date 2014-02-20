using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BDHero;
using BDHero.ErrorReporting;
using NUnit.Framework;

namespace DotNetTests
{
    [TestFixture]
    public class ErrorReporterTest
    {
        [Test]
        public void TestThrowsWin32Exception()
        {
            try
            {
                throw new WebException("Blah blah blah");
            }
            catch (Exception e)
            {
                ErrorReporter.Report(e);
            }
        }
    }
}
