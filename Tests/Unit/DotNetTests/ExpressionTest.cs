using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
                                              var result1 = Try(() => CloseHandle(IntPtr.Zero), result => result);
                                          });
        }

        [Test]
        public void TestReturnsResult() {
            var jobObjectHandle = IntPtr.Zero;

            try
            {
                jobObjectHandle = Try(() => CreateJobObject(IntPtr.Zero, null), result => result != IntPtr.Zero);

                Assert.AreNotEqual(jobObjectHandle, IntPtr.Zero);
            }
            finally
            {
                if (jobObjectHandle != IntPtr.Zero)
                {
                    CloseHandle(jobObjectHandle);
                }
            }
        }

        private static T Try<T>(Expression<Func<T>> expression, Func<T, bool> successCondition)
        {
            var result = expression.Compile().Invoke();
            if (!successCondition(result))
            {
                var methodCallExpr = expression.Body as MethodCallExpression;
                var methodInfo = methodCallExpr != null ? methodCallExpr.Method : null;
                var errorCode = Marshal.GetLastWin32Error();
                var message = string.Format("P/Invoke of {0} failed with error code = {1}", methodInfo, errorCode);
                throw new Win32Exception(message);
            }
            return result;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle([In] IntPtr jobHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateJobObject(
            IntPtr jobAttributes,
            string name);
    }
}
