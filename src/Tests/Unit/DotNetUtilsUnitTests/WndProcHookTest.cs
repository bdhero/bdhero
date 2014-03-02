using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DotNetUtils.Forms;
using NUnit.Framework;

namespace DotNetUtilsUnitTests
{
    [TestFixture]
    public class WndProcHookTest
    {
        private WndProcHookTestForm _form;
        private WndProcHook _hook;
        private bool _hooked;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _form = new WndProcHookTestForm();
            _hook = new WndProcHook(_form);
        }

        [SetUp]
        public void SetUp()
        {
            _form.IsSetText = false;
            _hooked = false;
        }

        [Test]
        public void TestHandled()
        {
            _hook.WndProcMessage += Handle;

            _form.Show();
            _form.Text = "abc";

            Thread.Sleep(1000);

            _hook.WndProcMessage -= Handle;

            _form.Hide();

            Assert.IsTrue(_hooked, "WndProc was not hooked");
            Assert.IsFalse(_form.IsSetText, "Form should not have received WM_SETTEXT message");
        }

        [Test]
        public void TestUnhandled()
        {
            _hook.WndProcMessage += DoNotHandle;

            _form.Show();
            _form.Text = "abc";

            Thread.Sleep(1000);

            _hook.WndProcMessage -= DoNotHandle;

            _form.Hide();

            Assert.IsTrue(_hooked, "WndProc was not hooked");
            Assert.IsTrue(_form.IsSetText, "Form did not receive WM_SETTEXT message");
        }

        private void Handle(ref Message m, HandledEventArgs args)
        {
            if (m.Msg == (int) TestWindowMessageType.WM_SETTEXT)
            {
                _hooked = true;
                args.Handled = true;
            }
        }

        private void DoNotHandle(ref Message m, HandledEventArgs args)
        {
            if (m.Msg == (int) TestWindowMessageType.WM_SETTEXT)
            {
                _hooked = true;
            }
        }
    }

    [System.ComponentModel.DesignerCategory("Code")]
    internal class WndProcHookTestForm : Form
    {
        public bool IsSetText { get; set; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int) TestWindowMessageType.WM_SETTEXT)
            {
                IsSetText = true;
            }

            base.WndProc(ref m);
        }
    }

    internal enum TestWindowMessageType
    {
        WM_SETTEXT = 0x000c,
    }
}
