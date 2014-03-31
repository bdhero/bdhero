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
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Concurrency;
using Microsoft.WindowsAPICodePack.Dialogs;
using OSUtils.Net;
using UILib.Extensions;
using UpdateLib;

namespace BDHeroGUI.Dialogs
{
    public class Windows7ErrorDialog : IErrorDialog
    {
        private const string CopyDetailsHref = "copy_details";
        private const string EditReportHref  = "edit_report";
        private const string SubmitButtonTextOnline  = "&Report This Error\nNo questions asked!";
        private const string SubmitButtonTextOffline = "&Report This Error\nInternet access required";
        private const string SubmitButtonTextUpdate  = "&Report This Error\nProgram update required";

        /// <summary>
        ///     Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get { return TaskDialog.IsPlatformSupported; }
        }

        private readonly ErrorReport _report;
        private readonly INetworkStatusMonitor _networkStatusMonitor;
        private readonly UpdateClient _updateClient;

        private readonly IList<IErrorReportResultVisitor> _reportResultVisitors = new List<IErrorReportResultVisitor>();

        private IErrorReportResult _errorReportResult;

        public Windows7ErrorDialog(ErrorReport report, INetworkStatusMonitor networkStatusMonitor, UpdateClient updateClient)
        {
            _report = report;
            _networkStatusMonitor = networkStatusMonitor;
            _updateClient = updateClient;

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
                    dialog.DetailsExpandedText = StackTrace;
                    dialog.FooterText = string.Format("<a href=\"{0}\">Copy to clipboard</a>", CopyDetailsHref);
                }

                var linkOnClick = CreateDialogOnHyperlinkClickHandler(dialog, owner);

                dialog.HyperlinkClick += linkOnClick;

                NetworkStatusChangedEventHandler nwStatusOnChanged = null;
                EventHandler dialogOnOpened = null;

                if (isReportable)
                {
                    var submitButton = CreateSubmitButton(dialog);
                    var declineButton = CreateDeclineButton(dialog);

                    dialog.Controls.Add(submitButton);
                    dialog.Controls.Add(declineButton);

                    if (dialog.FooterText != null)
                        dialog.FooterText = string.Format("<a href=\"{0}\">View report contents</a> - {1}",
                                                          EditReportHref, dialog.FooterText);

                    nwStatusOnChanged = CreateNetworkStatusChangedEventHandler(submitButton);
                    _networkStatusMonitor.NetworkStatusChanged += nwStatusOnChanged;

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
            // TaskButtonDialogBase.Enabled's documentation states: "The enabled state can cannot be changed before the dialog is shown."
            return (o, args) => OnNetworkStatusChanged(submitButton, _networkStatusMonitor.IsOnline);
        }

        private void OnNetworkStatusChanged([CanBeNull] TaskDialogCommandLink submitButton, bool isConnectedToInternet)
        {
            if (submitButton == null)
                return;

            submitButton.Enabled = isConnectedToInternet && !_updateClient.IsUpdateAvailable;
        }

        private TaskDialogCommandLink CreateSubmitButton(TaskDialog dialog)
        {
            var text = _updateClient.IsUpdateAvailable ? SubmitButtonTextUpdate :
                       _networkStatusMonitor.IsOnline  ? SubmitButtonTextOnline :
                                                         SubmitButtonTextOffline;

            var sendButton = new TaskDialogCommandLink("submitButton", text);
            sendButton.Click += (sender, args) => Submit(dialog);
            return sendButton;
        }

        private void Submit(TaskDialog dialog)
        {
            new EmptyPromise()
                .Work(ErrorReportOnDoWork)
                .Done(ErrorReportOnSucceed)
                .Start();
            dialog.Close(TaskDialogResult.Yes);
        }

        private void ErrorReportOnDoWork(IPromise<Nil> promise)
        {
            _errorReportResult = ErrorReporter.Report(_report);
        }

        private void ErrorReportOnSucceed(IPromise<Nil> promise)
        {
            OnErrorReportCompleted(_errorReportResult);
        }

        private void OnErrorReportCompleted([NotNull] IErrorReportResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            foreach (var visitor in _reportResultVisitors)
            {
                result.Accept(visitor);
            }

            _reportResultVisitors.Clear();
        }

        private static TaskDialogCommandLink CreateDeclineButton(TaskDialog dialog)
        {
            var dontSendButton = new TaskDialogCommandLink("declineButton", "&No Thanks\nI don't feel like being helpful");
            dontSendButton.Click += delegate { dialog.Close(TaskDialogResult.No); };
            return dontSendButton;
        }

        private void OnHyperlinkClick(TaskDialog dialog, [CanBeNull] IWin32Window owner, TaskDialogHyperlinkClickedEventArgs args)
        {
            switch (args.LinkText)
            {
                case CopyDetailsHref:
                    CopyDetailsToClipboard(owner);
                    return;

                case EditReportHref:
                    EditErrorReport(dialog, owner);
                    return;
            }
        }

        private void CopyDetailsToClipboard(IWin32Window owner)
        {
            Clipboard.SetText(_report.ExceptionDetailRaw);
            MessageBox.Show(owner, "Error details copied to clipboard.", "Copied!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EditErrorReport(TaskDialog dialog, IWin32Window owner)
        {
            DialogResult result;

            using (var form = new FormErrorReport(_report, _networkStatusMonitor, _updateClient))
            {
                result = form.ShowDialog(owner);
            }

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                Submit(dialog);
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
