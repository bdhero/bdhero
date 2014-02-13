using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetUtils.Extensions;
using DotNetUtils.TaskUtils;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DotNetUtils.Dialogs
{
    public class ExceptionDialog
    {
        /// <summary>
        ///     Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get { return TaskDialog.IsPlatformSupported; }
        }

        private readonly string _title;
        private readonly Exception _exception;

        public ExceptionDialog(string title, Exception exception)
        {
            _title = title;
            _exception = exception;
        }

        public DialogResult ShowDialog(IWin32Window owner, Action reportAction)
        {
            var isLogicError = !IsID10TError(_exception);

            var editReportLinkHref = "edit_report";

            var dialog = new TaskDialog
                         {
                             Cancelable = true,
                             DetailsExpanded = false,
                             HyperlinksEnabled = true,
                             ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter,
                             StartupLocation = TaskDialogStartupLocation.CenterOwner,

                             Icon = TaskDialogStandardIcon.Error,
                             Caption = _title,
                             InstructionText = "An unexpected error occured.",
                             Text = _exception.Message,
                             DetailsExpandedText = _exception.ToString(),

                             DetailsCollapsedLabel = "Show &details",
                             DetailsExpandedLabel = "Hide &details",

                             FooterText = string.Format("<a href=\"{0}\">Edit report contents</a>", editReportLinkHref),

                             OwnerWindowHandle = owner.Handle
                         };

            var sendButton = new TaskDialogCommandLink("sendButton", "&Report This Error\nFast and painless - I promise!");
            sendButton.Click += delegate
                                {
                                    new TaskBuilder()
                                        .OnCurrentThread()
                                        .DoWork((invoker, token) => reportAction())
                                        .Fail(args => ReportExceptionFail(owner, args))
                                        .Succeed(() => ReportExceptionSucceed(owner))
                                        .Build()
                                        .Start();
                                    dialog.Close(TaskDialogResult.Yes);
                                };

            var dontSendButton = new TaskDialogCommandLink("dontSendButton", "&No Thanks\nI don't feel like being helpful");
            dontSendButton.Click += delegate
                                    {
                                        dialog.Close(TaskDialogResult.No);
                                    };

            dialog.HyperlinkClick += (sender, args) => MessageBox.Show(owner, args.LinkText);

            if (true || isLogicError)
            {
                dialog.Controls.Add(sendButton);
                dialog.Controls.Add(dontSendButton);
            }

            return dialog.Show().ToDialogResult();
        }

        private static void ReportExceptionSucceed(IWin32Window owner)
        {
            var dialog = new TaskDialog
                         {
                             Cancelable = true,
                             StartupLocation = TaskDialogStartupLocation.CenterOwner,

                             Icon = TaskDialogStandardIcon.Information,
                             Caption = "Cool!",
                             InstructionText = "Thanks for submitting an error report!",

                            FooterCheckBoxChecked = false,
                            FooterCheckBoxText = "&Don't show this message again"
                         };

            if (IsFormValid(owner))
                dialog.OwnerWindowHandle = owner.Handle;

            dialog.Show();

            if (dialog.FooterCheckBoxChecked.GetValueOrDefault())
                MessageBox.Show(owner, "OK, we won't show this again");
            else
                MessageBox.Show(owner, "Prepare to see more of me, bitch!");
        }

        private static bool IsFormValid(IWin32Window owner)
        {
            var control = Control.FromHandle(owner.Handle);
            return !(control == null || control.IsDisposed);
        }

        private static void ReportExceptionFail(IWin32Window owner, ExceptionEventArgs args)
        {
            if (args.Exception == null)
                return;

            var icon = MessageBoxIcon.Error;
            var title = "Error Reporting Failed";
            var stackTrace = args.Exception.ToString();

            ShowResultMessage(owner, title, stackTrace, icon);
        }

        private static void ShowResultMessage(IWin32Window owner, string title, string message, MessageBoxIcon icon)
        {
            if (IsFormValid(owner))
            {
                MessageBox.Show(owner,
                                message,
                                title,
                                MessageBoxButtons.OK,
                                icon);
            }
            else
            {
                MessageBox.Show(message,
                                title,
                                MessageBoxButtons.OK,
                                icon);
            }
        }

        /// <summary>
        ///     Determines if the given exception is likely due to user error (ID10T).
        /// </summary>
        /// <param name="e">
        ///     Exception that was thrown elsewhere in the application.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the given exception is likely due to user error; otherwise <c>false</c>.
        /// </returns>
        private static bool IsID10TError(Exception e)
        {
            return (e is System.IO.DirectoryNotFoundException ||
                    e is System.IO.DriveNotFoundException ||
                    e is System.IO.FileNotFoundException ||
                    e is System.IO.PathTooLongException ||
                    e is System.Net.WebException);
        }
    }
}
