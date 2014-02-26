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
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using DotNetUtils.TaskUtils;
using Microsoft.WindowsAPICodePack.Dialogs;
using OSUtils.Net;

namespace BDHeroGUI.Dialogs
{
    public class Windows7ErrorDialog : IErrorDialog
    {
        private const string CopyDetailsHref = "copy_details";
        private const string EditReportHref = "edit_report";

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
        private TaskDialogCommandLink _submitButton;

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

        private DialogResult ShowDialog([CanBeNull] IWin32Window owner, bool isReportable)
        {
            var dialog = new TaskDialog
                         {
                             Icon = TaskDialogStandardIcon.Error,
                             Cancelable = true,
                             DetailsExpanded = false,
                             HyperlinksEnabled = true,
                             StartupLocation = TaskDialogStartupLocation.CenterOwner,
                         };

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

            dialog.HyperlinkClick += (sender, args) => OnHyperlinkClick(owner, args);

            if (isReportable)
            {
                _submitButton = CreateSubmitButton(dialog);

                dialog.Controls.Add(_submitButton);
                dialog.Controls.Add(CreateDeclineButton(dialog));

                if (dialog.FooterText != null)
                    dialog.FooterText = string.Format("<a href=\"{0}\">View report contents</a> - {1}", EditReportHref, dialog.FooterText);
            }

            _networkStatusMonitor.NetworkStatusChanged += OnNetworkStatusChanged;

            // TaskButtonDialogBase.Enabled's documentation states: "The enabled state can cannot be changed before the dialog is shown."
            // So we need to set a short timer to disable the button *after* the dialog is shown.
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += (a, b) => OnNetworkStatusChanged(_networkStatusMonitor.IsOnline);
            timer.AutoReset = false;
            timer.Start();

            var result = dialog.Show().ToDialogResult();

            _networkStatusMonitor.NetworkStatusChanged -= OnNetworkStatusChanged;

            return result;
        }

        private void OnNetworkStatusChanged(bool isConnectedToInternet)
        {
            if (_submitButton == null)
                return;

            _submitButton.Enabled = isConnectedToInternet;
        }

        private TaskDialogCommandLink CreateSubmitButton(TaskDialog dialog)
        {
            IErrorReportResult result = null;
            var sendButton = new TaskDialogCommandLink("submitButton", "&Report This Error\nNo questions asked!");
            sendButton.Click += delegate
                                {
                                    new TaskBuilder()
                                        .OnCurrentThread()
                                        .DoWork((invoker, token) => result = ErrorReporter.Report(_report))
                                        .Succeed(() => OnErrorReportCompleted(result))
                                        .Build()
                                        .Start();
                                    dialog.Close(TaskDialogResult.Yes);
                                };
            return sendButton;
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

        private void OnHyperlinkClick([CanBeNull] IWin32Window owner, TaskDialogHyperlinkClickedEventArgs args)
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
                MessageBox.Show(owner, "Edit report contents");
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
