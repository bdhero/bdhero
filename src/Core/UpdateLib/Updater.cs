// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using DotNetUtils.Crypto;
using DotNetUtils.Net;
using Newtonsoft.Json;
using OSUtils;
using log4net;
using OSUtils.Info;

namespace UpdateLib
{
    public class Updater
    {
        private readonly ILog _logger;

        private Update _latestUpdate;
        private string _latestInstallerPath;

        private volatile UpdaterClientState _state;

        private volatile bool _hasChecked;

        private CancellationTokenSource _cancellationTokenSource;

        public event FileDownloadProgressChangedHandler DownloadProgressChanged;

        public Version CurrentVersion;

        public bool IsPortable = true;

        public Updater(ILog logger)
        {
            _logger = logger;
        }

        public bool HasChecked
        {
            get { return _hasChecked; }
        }

        public Update LatestUpdate
        {
            get
            {
                EnsureChecked();
                return _latestUpdate;
            }
        }

        /// <exception cref="InvalidOperationException">Thrown if the caller hasn't checked for updates yet</exception>
        public bool IsUpdateAvailable
        {
            get
            {
                EnsureChecked();
                return _latestUpdate != null && _latestUpdate.Version > CurrentVersion;
            }
        }

        /// <exception cref="InvalidOperationException">Thrown if the caller hasn't checked for updates yet</exception>
        public bool IsUpdateReadyToInstall
        {
            get
            {
                EnsureChecked();
                return _latestInstallerPath != null && File.Exists(_latestInstallerPath);
            }
        }

        /// <summary>
        /// Invoked just before all HTTP requests are made, allowing observers to modify requests before they are sent.
        ///  This can be useful to override the system's default proxy settings, set custom timeout values, etc.
        /// </summary>
        public event BeforeRequestEventHandler BeforeRequest;

        public void CheckForUpdate(Version currentVersion)
        {
            CurrentVersion = currentVersion;
            GetLatestVersionSync();
        }

        public void DownloadUpdate()
        {
            DownloadUpdateSync(_latestUpdate);
        }

        // TODO: Move this to WindowsUpdaterClient class and make Updater an interface, abstract class, or composite class
        public void InstallUpdate()
        {
            _logger.Info("Installing update");
            using (var setup = Process.Start(_latestInstallerPath, "/VerySilent /CloseApplications"))
            {
                _logger.Debug("Waiting for application to exit...");
            }
        }

        // TODO: Move this to Windows-specific library
        private static Process CreateTaskKillProcess()
        {
            using (var currentProcess = Process.GetCurrentProcess())
            {
                var taskkill = CreateHiddenProcess();
                taskkill.StartInfo.FileName = "taskkill";
                taskkill.StartInfo.Arguments = "/PID " + currentProcess.Id + " /F";
                return taskkill;
            }
        }

        private static Process CreateHiddenProcess()
        {
            return new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };
        }

        public void CancelDownload()
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }

        private void NotifyBeforeRequest(HttpWebRequest request)
        {
            if (BeforeRequest != null)
                BeforeRequest(request);
        }

        private void CheckState()
        {
            if (_state == UpdaterClientState.Checking)
                throw new InvalidOperationException("Already checking for updates");
            if (_state == UpdaterClientState.Downloading)
                throw new InvalidOperationException("Already downloading update");
            if (_state == UpdaterClientState.Paused)
                throw new InvalidOperationException("Already downloading update (paused)");
        }

        /// <exception cref="InvalidOperationException">Thrown if the caller hasn't checked for updates yet</exception>
        private void EnsureChecked()
        {
            if (CurrentVersion == null)
                throw new InvalidOperationException("No current version was specified; unable to tell if there's a new version!");
            if (!_hasChecked)
                throw new InvalidOperationException("You need to check for updates first!");
        }

        private void GetLatestVersionSync()
        {
            CheckState();

            try
            {
                _state = UpdaterClientState.Checking;
                HttpRequest.BeforeRequestGlobal += NotifyBeforeRequest;
                var json = HttpRequest.Get("http://update.bdhero.org/update.json");
                var response = JsonConvert.DeserializeObject<UpdateResponse>(json);
                _latestUpdate = FromResponse(response);
                _state = UpdaterClientState.Ready;
                _hasChecked = true;
            }
            catch (Exception e)
            {
                _state = UpdaterClientState.Error;
                _logger.Error("Error occurred while checking for application update", e);
                throw;
            }
            finally
            {
                HttpRequest.BeforeRequestGlobal -= NotifyBeforeRequest;
            }
        }

        private Update FromResponse(UpdateResponse response)
        {
            var mirror = response.Mirrors.First();
            var platform = GetPlatform(response);
            var package = GetPackage(platform);

            // No package available for the user's OS
            if (package == null)
            {
                return null;
            }

            var version = response.Version;
            var filename = package.FileName;
            var uri = mirror + filename;

            return new Update(version, filename, uri, package.SHA1, package.Size);
        }

        private static Platform GetPlatform(UpdateResponse response)
        {
            var platforms = response.Platforms;
            var osType = SystemInfo.Instance.OS.Type;
            if (OSType.Mac == osType)
                return platforms.Mac;
            if (OSType.Linux == osType)
                return platforms.Linux;
            return platforms.Windows;
        }

        private Package GetPackage(Platform platform)
        {
            return IsPortable ? platform.Packages.Zip : platform.Packages.Setup;
        }

        /// <exception cref="IOException">
        /// Thrown if a network error occurs or the SHA-1 hash of the downloaded file
        /// does not match the expected value in the update manifest.
        /// </exception>
        private void DownloadUpdateSync(Update update)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var path = Path.Combine(Path.GetTempPath(), update.FileName);
            var downloader = new FileDownloader
                {
                    Uri = update.Uri,
                    Path = path,
                    CancellationToken = _cancellationTokenSource.Token
                };

            downloader.BeforeRequest += NotifyBeforeRequest;
            downloader.ProgressChanged += DownloaderOnProgressChanged;

            downloader.DownloadSync();

            if (downloader.State != FileDownloadState.Success)
                return;

            var hash = new SHA1Algorithm().ComputeFile(path);

            if (!String.Equals(hash, update.SHA1, StringComparison.OrdinalIgnoreCase))
            {
                _logger.ErrorFormat(
                    "Unable to verify integrity of \"{0}\" via SHA-1 hash: expected {1}, but found {2}",
                    path, update.SHA1, hash);
                throw new IOException("Update file is corrupt or has been tampered with; SHA-1 hash is incorrect");
            }

            _latestInstallerPath = path;
        }

        private void DownloaderOnProgressChanged(FileDownloadProgress fileDownloadProgress)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(fileDownloadProgress);
        }
    }

    enum UpdaterClientState
    {
        Ready,
        Checking,
        Downloading,
        Paused,
        Error
    }
}
