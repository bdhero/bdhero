// Copyright 2012-2014 Andrew C. Dvorak
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
using System.Linq;
using DotNetUtils;
using DotNetUtils.Concurrency;
using DotNetUtils.Extensions;
using DotNetUtils.FS;
using DotNetUtils.Net;

namespace UpdateLib.V1
{
    public class UpdateHelperV1
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISet<IUpdateObserverV1> _observerSet = new HashSet<IUpdateObserverV1>();
        private readonly IList<IUpdateObserverV1> _observerList = new List<IUpdateObserverV1>();

        private readonly UpdaterV1 _updater;
        private readonly Version _currentVersion;

        private UpdateButtonClickEventHandler _updatesButtonClickAction;

        public bool AllowDownload = true;
        public bool AllowInstallUpdate = true;

        public bool CanInstallUpdate
        {
            get { return _updater.HasChecked && _updater.IsUpdateAvailable && _updater.IsUpdateReadyToInstall && AllowInstallUpdate; }
        }

        public bool ShouldInstallUpdate
        {
            get { return !_observerList.Any() || _observerList.Last().ShouldInstallUpdate(_updater.LatestUpdate); }
        }

        public UpdateHelperV1(UpdaterV1 updater, Version currentVersion)
        {
            _updater = updater;
            _currentVersion = currentVersion;

            _updatesButtonClickAction = CheckForUpdatesAsync;
        }

        #region Observers

        public void RegisterObserver(IUpdateObserverV1 observer)
        {
            if (_observerSet.Contains(observer)) return;
            _observerSet.Add(observer);
            _observerList.Add(observer);
        }

        public void UnregisterObserver(IUpdateObserverV1 observer)
        {
            if (!_observerSet.Contains(observer)) return;
            _observerSet.Remove(observer);
            _observerList.Remove(observer);
        }

        private void Notify(Action<IUpdateObserverV1> action)
        {
            _observerList.Reverse().ForEach(action);
        }

        #endregion

        public void Click()
        {
            if (_updatesButtonClickAction != null)
                _updatesButtonClickAction();
        }

        private void CheckForUpdatesAsync()
        {
            // Prevent user from checking for updates while another check is already in progress
            _updatesButtonClickAction = null;

            new EmptyPromise()
                .Before(OnBeforeStart)
                .Work(OnDoWork)
                .Fail(OnFail)
                .Canceled(OnFail)
                .Done(OnSucceed)
                .Start()
                ;
        }

        public void InstallUpdateIfAvailable()
        {
            InstallUpdateIfAvailable(false);
        }

        public void InstallUpdateIfAvailable(bool silent)
        {
            if (!CanInstallUpdate) return;

            var doInstall = silent || ShouldInstallUpdate;
            if (!doInstall) return;

            Notify(observer => observer.OnBeforeInstallUpdate(_updater.LatestUpdate));
            _updater.InstallUpdate();
        }

        private void OnBeforeStart(IPromise<Nil> promise)
        {
            Logger.Info("Checking for updates");
            Notify(observer => observer.OnBeforeCheckForUpdate());
        }

        private void OnUpdateReadyToDownload()
        {
            Logger.Info(string.Format("Version {0} is available", _updater.LatestUpdate.Version));
            Notify(observer => observer.OnUpdateReadyToDownload(_updater.LatestUpdate));
        }

        private void OnBeforeDownload()
        {
            Logger.Info(string.Format("Downloading version {0}", _updater.LatestUpdate.Version));
            Notify(observer => observer.OnBeforeDownloadUpdate(_updater.LatestUpdate));
        }

        private void OnDoWork(IPromise<Nil> promise)
        {
            _updater.CheckForUpdate(_currentVersion);

            if (!_updater.IsUpdateAvailable)
                return;

            promise.UIInvoker.InvokeSync(OnUpdateReadyToDownload);

            if (!AllowDownload)
                return;

            promise.UIInvoker.InvokeSync(OnBeforeDownload);

            _updater.DownloadProgressChanged += progress => promise.UIInvoker.InvokeSync(() => ProgressChanged(progress));

            promise.Canceled(_ => _updater.CancelDownload());

            _updater.DownloadUpdate();
        }

        private void ProgressChanged(FileDownloadProgress progress)
        {
            var message =
                string.Format(
                    "Downloading version {0}: {1} of {2} @ {3} ({4:P})",
                    _updater.LatestUpdate.Version,
                    FileUtils.HumanFriendlyFileSize(progress.BytesDownloaded),
                    FileUtils.HumanFriendlyFileSize(progress.ContentLength),
                    progress.HumanSpeed,
                    progress.PercentComplete / 100.0);
            Logger.Debug(message);
            Notify(observer => observer.OnUpdateDownloadProgressChanged(_updater.LatestUpdate, progress));
        }

        private void OnFail(IPromise<Nil> promise)
        {
            _updatesButtonClickAction = CheckForUpdatesAsync;
            Logger.Error("Error checking for update", promise.LastException);
            Notify(observer => observer.OnUpdateException(promise.LastException));
        }

        private void OnSucceed(IPromise<Nil> promise)
        {
            if (_updater.IsUpdateAvailable)
            {
                if (AllowDownload && AllowInstallUpdate)
                {
                    _updatesButtonClickAction = InstallUpdateIfAvailable;
                    Logger.Info("Update is ready to install");
                    Notify(observer => observer.OnUpdateReadyToInstall(_updater.LatestUpdate));
                }
                else
                {
                    _updatesButtonClickAction = LaunchBrowser;
                    Logger.Info("Update is available");
                    Notify(observer => observer.OnUpdateReadyToDownload(_updater.LatestUpdate));
                }
            }
            else
            {
                _updatesButtonClickAction = CheckForUpdatesAsync;
                Logger.Info(string.Format("Application is already up to date"));
                Notify(observer => observer.OnNoUpdateAvailable());
            }
        }

        private void LaunchBrowser()
        {
            FileUtils.OpenUrl("http://bdhero.org/");
        }
    }

    public delegate void UpdateButtonClickEventHandler();
    public delegate void BeforeInstallUpdateEventHandler(Update update);
}
