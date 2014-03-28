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
using System.Collections.Generic;
using System.Windows.Forms;
using BDHero.ErrorReporting;
using BDHeroGUI.Forms;
using DotNetUtils.Annotations;
using DotNetUtils.TaskUtils;
using Microsoft.WindowsAPICodePack.Dialogs;
using OSUtils.Net;
using UILib.Extensions;

namespace BDHeroGUI.Dialogs
{
    public class Windows7ErrorDialog : IErrorDialog
    {
        private const string CopyDetailsHref = "copy_details";
        private const string EditReportHref = "edit_report";
        private const string SubmitButtonTextOnline = "&Report This Error\nNo questions asked!";
        private const string SubmitButtonTextOffline = "&Report This Error (Internet access required)\nNo questions asked!";

        /// <summary>
        ///     Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get { return TaskDialog.IsPlatformSupported; }
        }

        private readonly INetworkStatusMonitor _networkStatusMonitor;
        private readonly ErrorReport _report;
        private readonly IList<IErrorReportResultVisitor> _reportResultVisitors = new List<IErrorReportResultVisitor>();

        public Windows7ErrorDialog(INetworkStatusMonitor networkStatusMonitor, ErrorReport report)
        {
            _networkStatusMonitor = networkStatusMonitor;
            _report = report;

            // Defaults
            Title = "Error";
            Heading = "An unexpected error occured.";
            Message = _report.ExceptionMessageRaw;
            StackTrace = _report.ExceptionDetailRaw;
        }

        public void AddResultVisitor(IErrorReportResultVisitor visitor)
        {
            _reportResultVisitors.Add(visitor);
        }

        public void RemoveResultVisitor(IErrorReportResultVisitor visitor)
        {
            _reportResultVisitors.Remove(visitor);
        }

        private static TaskDialog CreateTaskDialog()
        {
            return new TaskDialog
                   {
                       Icon = TaskDialogStandardIcon.Error,
                       Cancelable = true,
                       DetailsExpanded = false,
                       HyperlinksEnabled = true,
                       StartupLocation = TaskDialogStartupLocation.CenterOwner,
                   };
        }

        private DialogResult ShowDialog([CanBeNull] IWin32Window owner, bool isReportable)
        {
            using (var dialog = CreateTaskDialog())
            {
                if (owner != null)
                    dialog.OwnerWindowHandle = owner.Handle;

                if (Title != null)
                    dialog.Caption = Title;

                if (Heading != null)
                    dialog.InstructionText = Heading;

                if (Message != null)
                    dialog.Text = Message;

                if (StackTrace != null)
                {
                    dialog.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;
                    dialog.DetailsCollapsedLabel = "Show &details";
                    dialog.DetailsExpandedLabel = "Hide &details";
                    dialog.DetailsExpandedText = StackTrace; // _exception.ToString(),
                    dialog.FooterText = string.Format("<a href=\"{0}\">Copy to clipboard</a>", CopyDetailsHref);
                }

                var linkOnClick = CreateDialogOnHyperlinkClickHandler(dialog, owner);

                dialog.HyperlinkClick += linkOnClick;

                NetworkStatusChangedEventHandler nwStatusOnChanged = null;
                EventHandler dialogOnOpened = null;

                if (isReportable)
                {
                    var submitButton = CreateSubmitButton(dialog);

                    dialog.Controls.Add(submitButton);
                    dialog.Controls.Add(CreateDeclineButton(dialog));

                    if (dialog.FooterText != null)
                        dialog.FooterText = string.Format("<a href=\"{0}\">View report contents</a> - {1}",
                                                          EditReportHref, dialog.FooterText);

                    nwStatusOnChanged = CreateNetworkStatusChangedEventHandler(submitButton);
                    _networkStatusMonitor.NetworkStatusChanged += nwStatusOnChanged;

                    // TaskButtonDialogBase.Enabled's documentation states: "The enabled state can cannot be changed before the dialog is shown."
                    // So we need to set a short timer to disable the button *after* the dialog is shown.
//                    var timer = new System.Timers.Timer(100);
//                    timer.Elapsed += (a, b) => 
//                    timer.AutoReset = false;
//                    timer.Start();

                    dialogOnOpened = CreateDialogOnOpened(submitButton);
                    dialog.Opened += dialogOnOpened;
                }

                var result = dialog.Show().ToDialogResult();

                #region Cleanup

                dialog.HyperlinkClick -= linkOnClick;

                if (nwStatusOnChanged != null)
                {
                    _networkStatusMonitor.NetworkStatusChanged -= nwStatusOnChanged;
                }

                if (dialogOnOpened != null)
                {
                    dialog.Opened -= dialogOnOpened;
                }

                #endregion

                return result;
            }
        }

        private EventHandler<TaskDialogHyperlinkClickedEventArgs> CreateDialogOnHyperlinkClickHandler(
            TaskDialog dialog, [CanBeNull] IWin32Window owner)
        {
            return (s, e) => OnHyperlinkClick(dialog, owner, e);
        }

        private NetworkStatusChangedEventHandler CreateNetworkStatusChangedEventHandler(
            [CanBeNull] TaskDialogCommandLink submitButton)
        {
            return isConnectedToInternet => OnNetworkStatusChanged(submitButton, isConnectedToInternet);
        }

        private EventHandler CreateDialogOnOpened([CanBeNull] TaskDialogCommandLink submitButton)
        {
            return (o, args) => OnNetworkStatusChanged(submitButton, _networkStatusMonitor.IsOnline);
        }

        private void OnNetworkStatusChanged([CanBeNull] TaskDialogCommandLink submitButton, bool isConnectedToInternet)
        {
            if (submitButton == null)
                return;

            submitButton.Enabled = isConnectedToInternet;
        }

        private TaskDialogCommandLink CreateSubmitButton(TaskDialog dialog)
        {
            var sendButton = new TaskDialogCommandLink("submitButton", _networkStatusMonitor.IsOnline ? SubmitButtonTextOnline : SubmitButtonTextOffline);
            sendButton.Click += (sender, args) => Submit(dialog);
            return sendButton;
        }

        private void Submit(TaskDialog dialog)
        {
            IErrorReportResult result = null;
            new TaskBuilder()
                .OnCurrentThread()
                .DoWork((invoker, token) => result = ErrorReporter.Report(_report))
                .Succeed(() => OnErrorReportCompleted(result))
                .Build()
                .Start();
            dialog.Close(TaskDialogResult.Yes);
        }

        private void OnErrorReportCompleted([NotNull] IErrorReportResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            foreach (var visitor in _reportResultVisitors)
            {
                result.Accept(visitor);
            }
        }

        private static TaskDialogCommandLink CreateDeclineButton(TaskDialog dialog)
        {
            var dontSendButton = new TaskDialogCommandLink("declineButton", "&No Thanks\nI don't feel like being helpful");
            dontSendButton.Click += delegate { dialog.Close(TaskDialogResult.No); };
            return dontSendButton;
        }

        private void OnHyperlinkClick(TaskDialog dialog, [CanBeNull] IWin32Window owner, TaskDialogHyperlinkClickedEventArgs args)
        {
            if (args.LinkText == CopyDetailsHref)
            {
                Clipboard.SetText(_report.ExceptionDetailRaw);
                MessageBox.Show(owner, "Error details copied to clipboard.", "Copied!",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (args.LinkText == EditReportHref)
            {
                using (var form = new FormErrorReport(_report, _networkStatusMonitor))
                {
                    var result = form.ShowDialog(owner);
                    if (result == DialogResult.OK || result == DialogResult.Yes)
                    {
                        Submit(dialog);
                    }
                }
            }
        }

#if false
        private static bool IsFormValid(IWin32Window owner)
        {
            var control = Control.FromHandle(owner.Handle);
            return !(control == null || control.IsDisposed);
        }
#endif

        public string Title { get; set; }
        
        public string Heading { get; set; }
        
        public string Message { get; set; }
        
        public string StackTrace { get; set; }

        public void ShowReportable(IWin32Window owner = null)
        {
            ShowDialog(owner, true);
        }

        public void ShowNonReportable(IWin32Window owner = null)
        {
            ShowDialog(owner, false);
        }
    }
}
