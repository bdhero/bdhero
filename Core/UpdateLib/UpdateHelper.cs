using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetUtils;
using DotNetUtils.Extensions;
using DotNetUtils.Net;
using DotNetUtils.TaskUtils;

namespace UpdateLib
{
    public class UpdateHelper
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISet<IUpdateObserver> _observerSet = new HashSet<IUpdateObserver>();
        private readonly IList<IUpdateObserver> _observerList = new List<IUpdateObserver>();

        private readonly Updater _updater;
        private readonly Version _currentVersion;

        private UpdateButtonClickEventHandler _updatesButtonClickAction;

        public bool AllowInstallUpdate = true;

        public bool CanInstallUpdate
        {
            get { return _updater.HasChecked && _updater.IsUpdateAvailable && _updater.IsUpdateReadyToInstall && AllowInstallUpdate; }
        }

        public bool ShouldInstallUpdate
        {
            get { return !_observerList.Any() || _observerList.Last().ShouldInstallUpdate(_updater.LatestUpdate); }
        }

        public UpdateHelper(Updater updater, Version currentVersion)
        {
            _updater = updater;
            _currentVersion = currentVersion;

            _updatesButtonClickAction = CheckForUpdatesAsync;
        }

        #region Observers

        public void RegisterObserver(IUpdateObserver observer)
        {
            if (_observerSet.Contains(observer)) return;
            _observerSet.Add(observer);
            _observerList.Add(observer);
        }

        public void UnregisterObserver(IUpdateObserver observer)
        {
            if (!_observerSet.Contains(observer)) return;
            _observerSet.Remove(observer);
            _observerList.Remove(observer);
        }

        private void Notify(Action<IUpdateObserver> action)
        {
            _observerList.Reverse().ForEach(action);
        }

        #endregion

        public void CheckForUpdates()
        {
            if (_updatesButtonClickAction != null)
                _updatesButtonClickAction();
        }

        private void CheckForUpdatesAsync()
        {
            // Prevent user from checking for updates while another check is already in progress
            _updatesButtonClickAction = null;

            var task =
                new TaskBuilder()
                    .OnCurrentThread()
                    .BeforeStart(OnBeforeStart)
                    .DoWork(OnDoWork)
                    .Fail(OnFail)
                    .Succeed(OnSucceed)
                    .Build();

            task.Start();
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

        private void OnBeforeStart()
        {
            Logger.Info("Checking for updates");
            Notify(observer => observer.OnBeforeCheckForUpdate());
        }

        private void OnBeforeDownload(CancellationToken cancellationToken)
        {
            Logger.Info(string.Format("Downloading version {0}", _updater.LatestUpdate.Version));
            Notify(observer => observer.OnBeforeDownloadUpdate(_updater.LatestUpdate));
        }

        private void OnDoWork(IThreadInvoker invoker, CancellationToken token)
        {
            _updater.CheckForUpdate(_currentVersion);

            if (!_updater.IsUpdateAvailable)
            {
                return;
            }

            invoker.InvokeOnUIThreadSync(OnBeforeDownload);

            _updater.DownloadProgressChanged += delegate(FileDownloadProgress progress)
                {
                    invoker.InvokeOnUIThreadSync(cancellationToken => ProgressChanged(progress));
                };

            token.Register(_updater.CancelDownload);

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

        private void OnFail(ExceptionEventArgs args)
        {
            _updatesButtonClickAction = CheckForUpdatesAsync;
            Logger.Error("Error checking for update", args.Exception);
            Notify(observer => observer.OnUpdateException(args.Exception));
        }

        private void OnSucceed()
        {
            if (_updater.IsUpdateAvailable)
            {
                _updatesButtonClickAction = InstallUpdateIfAvailable;
                Logger.Info("Update is ready to install");
                Notify(observer => observer.OnUpdateReadyToInstall(_updater.LatestUpdate));
            }
            else
            {
                _updatesButtonClickAction = CheckForUpdatesAsync;
                Logger.Info(string.Format("Application is already up to date"));
                Notify(observer => observer.OnNoUpdateAvailable());
            }
        }
    }

    public delegate void UpdateButtonClickEventHandler();
    public delegate void BeforeInstallUpdateEventHandler(Update update);
}
