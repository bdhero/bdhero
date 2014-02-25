// Copyright 2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using BDHero;
using BDHero.ErrorReporting;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using DotNetUtils.TaskUtils;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BDHeroGUI.Dialogs
{
    public class Windows7ErrorDialog
    {
        /// <summary>
        ///     Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get { return TaskDialog.IsPlatformSupported; }
        }

        public readonly IList<IErrorReportResultVisitor> ReportResultVisitors = new List<IErrorReportResultVisitor>();

        private readonly string _title;
        private readonly Exception _exception;
        private readonly bool _isReportable;

        public Windows7ErrorDialog(string title, Exception exception, bool isReportable)
        {
            _title = title;
            _exception = exception;
            _isReportable = isReportable;
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            var copyDetailsLinkHref = "copy_details";
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

                             FooterText = string.Format("<a href=\"{0}\">Copy to clipboard</a>", copyDetailsLinkHref),

                             OwnerWindowHandle = owner.Handle
                         };

            IErrorReportResult result = null;
            var sendButton = new TaskDialogCommandLink("sendButton", "&Report This Error\nNo questions asked!");
            sendButton.Click += delegate
                                {
                                    new TaskBuilder()
                                        .OnCurrentThread()
                                        .DoWork((invoker, token) => result = ErrorReporter.Report(_exception))
                                        .Succeed(() => ReportExceptionCompleted(owner, result))
                                        .Build()
                                        .Start();
                                    dialog.Close(TaskDialogResult.Yes);
                                };

            var dontSendButton = new TaskDialogCommandLink("dontSendButton", "&No Thanks\nI don't feel like being helpful");
            dontSendButton.Click += delegate
                                    {
                                        dialog.Close(TaskDialogResult.No);
                                    };

            dialog.HyperlinkClick += delegate(object sender, TaskDialogHyperlinkClickedEventArgs args)
                                     {
                                         if (args.LinkText == copyDetailsLinkHref)
                                         {
                                             Clipboard.SetText(_exception.ToString());
                                             MessageBox.Show(owner, "Error details copied to clipboard.", "Copied!",
                                                             MessageBoxButtons.OK, MessageBoxIcon.Information);
                                             return;
                                         }
                                         if (args.LinkText == editReportLinkHref)
                                         {
                                             MessageBox.Show(owner, "Edit report contents");
                                         }
                                     };

            if (_isReportable)
            {
                dialog.Controls.Add(sendButton);
                dialog.Controls.Add(dontSendButton);
                dialog.FooterText = string.Format("<a href=\"{0}\">View report contents</a> - {1}", editReportLinkHref, dialog.FooterText);
            }

            return dialog.Show().ToDialogResult();
        }

        private void ReportExceptionCompleted(IWin32Window owner, [NotNull] IErrorReportResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            foreach (var visitor in ReportResultVisitors)
            {
                result.Accept(visitor);
            }

#if false
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
#endif
        }

#if false
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
#endif
    }
}
