﻿using System;
using System.Net;
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
                ErrorReporter.Report(new ErrorReport(e));
            }
        }
    }
}
