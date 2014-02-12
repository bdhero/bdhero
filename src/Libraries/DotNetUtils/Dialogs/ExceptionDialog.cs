using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

        public void ShowDialog(IWin32Window owner, Action reportAction)
        {
            var isLogicError = !IsID10TError(_exception);

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

                             OwnerWindowHandle = owner.Handle
                         };

            var sendButton = new TaskDialogCommandLink("sendButton", "&Report This Error\nQuick and painless - promise!");
            sendButton.Click += delegate
                                {
                                    new TaskBuilder()
                                        .OnCurrentThread()
                                        .DoWork((invoker, token) => reportAction())
                                        .Fail(args => ReportExceptionFail(owner, args))
                                        .Build()
                                        .Start();
                                    dialog.Close();
                                };

            var dontSendButton = new TaskDialogCommandLink("dontSendButton", "&No Thanks\nI don't feel like being helpful");
            dontSendButton.Click += delegate
                                    {
                                        dialog.Close();
                                    };

            if (isLogicError)
            {
                dialog.Controls.Add(sendButton);
                dialog.Controls.Add(dontSendButton);
            }

            dialog.Show();
        }

        private static void ReportExceptionFail(IWin32Window owner, ExceptionEventArgs args)
        {
            if (args.Exception == null)
                return;

            var title = "Error Reporting Failed";
            var stackTrace = args.Exception.ToString();

            var control = Control.FromHandle(owner.Handle);
            if (control == null || control.IsDisposed)
            {
                MessageBox.Show(stackTrace,
                                title,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(owner,
                                stackTrace,
                                title,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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
