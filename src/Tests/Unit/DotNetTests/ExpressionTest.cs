using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsOSUtils;
using WindowsOSUtils.WinAPI.Kernel;
using NUnit.Framework;

namespace DotNetTests
{
    [TestFixture]
    public class ExpressionTest
    {
        [Test]
        public void TestThrowsWin32Exception()
        {
            Assert.Throws<Win32Exception>(delegate
                                          {
                                              var result1 = PInvokeUtils.Try(() => JobObjectAPI.CloseHandle(IntPtr.Zero), result => result);
                                          });
        }

        [Test]
        public void TestReturnsResult() {
            var jobObjectHandle = IntPtr.Zero;

            try
            {
                jobObjectHandle = PInvokeUtils.Try(() => JobObjectAPI.CreateJobObject(IntPtr.Zero, null), result => result != IntPtr.Zero);

                Assert.AreNotEqual(jobObjectHandle, IntPtr.Zero);
            }
            finally
            {
                if (jobObjectHandle != IntPtr.Zero)
                {
                    JobObjectAPI.CloseHandle(jobObjectHandle);
                }
            }
        }
    }
}
