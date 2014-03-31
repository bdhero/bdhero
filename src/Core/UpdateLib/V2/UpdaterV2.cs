using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using DotNetUtils;
using RestSharp;

namespace UpdateLib.V2
{
    public class UpdaterV2
    {
//        private const string DefaultUpdateManifestBaseUrl = "http://update.bdhero.org";
        private const string DefaultUpdateManifestBaseUrl = "http://192.168.0.151:8080";
        private const string DefaultUpdateManifestFileUrl = "/update.json";

        #region Public properties

        public string UpdateManifestBaseUrl { get; set; }

        public string UpdateManifestFileUrl { get; set; }

        public Version CurrentVersion { get; set; }

        public bool IsPortable { get; set; }

        public Update LatestUpdate { get; private set; }

        public bool IsUpdateAvailable { get { return LatestUpdate != null && LatestUpdate.Version > CurrentVersion; } }

        #endregion

        #region Public events

        public event UpdaterV2CheckingForUpdateEventHandler Checking;

        public event UpdaterV2UpdateFoundEventHandler UpdateFound;

        public event UpdaterV2UpdateNotFoundEventHandler UpdateNotFound;

        public event UpdaterV2ErrorCheckingForUpdateEventHandler Error;

        #endregion

        private readonly ManualResetEventSlim _isChecking = new ManualResetEventSlim();

        public UpdaterV2()
        {
            UpdateManifestBaseUrl = DefaultUpdateManifestBaseUrl;
            UpdateManifestFileUrl = DefaultUpdateManifestFileUrl;
            CurrentVersion = new Version();
            IsPortable = false;
        }

        public void CheckForUpdateAsync()
        {
            // Already checking for an update
            if (_isChecking.IsSet)
                return;

            var client = new RestClient(UpdateManifestBaseUrl);
            var request = new RestRequest(UpdateManifestFileUrl);
            var asyncRequestHandle = client.ExecuteAsync(request, OnResponse);

            if (Checking != null)
                Checking(this);

            _isChecking.Set();
        }

        // TODO: UI callback thread?
        private void OnResponse(IRestResponse response, RestRequestAsyncHandle asyncHandle)
        {
            _isChecking.Wait(); // TODO: Add timeout?

            try
            {
                if (response.StatusCode == HttpStatusCode.OK && response.ErrorException == null)
                {
                    HandleSuccess(response);
                }
                else
                {
                    HandleError(response);
                }
            }
            finally
            {
                _isChecking.Reset();
            }
        }

        private void HandleError(IRestResponse response)
        {
            if (Error != null)
                Error(this, response.ErrorException);
        }

        private void HandleSuccess(IRestResponse response)
        {
            var updateResponse = SmartJsonConvert.DeserializeObject<UpdateResponse>(response.Content);

            LatestUpdate = Update.FromResponse(updateResponse, IsPortable);

            if (IsUpdateAvailable)
            {
                if (UpdateFound != null)
                    UpdateFound(this);
            }
            else
            {
                if (UpdateNotFound != null)
                    UpdateNotFound(this);
            }
        }
    }

    public delegate void UpdaterV2CheckingForUpdateEventHandler(UpdaterV2 updater);
    public delegate void UpdaterV2UpdateFoundEventHandler(UpdaterV2 updater);
    public delegate void UpdaterV2UpdateNotFoundEventHandler(UpdaterV2 updater);
    public delegate void UpdaterV2ErrorCheckingForUpdateEventHandler(UpdaterV2 updater, Exception exception);
}
