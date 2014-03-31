using System;
using System.Net;
using System.Threading;
using DotNetUtils;
using DotNetUtils.Annotations;
using RestSharp;

namespace UpdateLib.V2
{
    /// <summary>
    ///     Update client that checks for application updates.
    /// </summary>
    public class UpdaterV2
    {
//        public const string DefaultUpdateManifestBaseUrl = "http://update.bdhero.org";
        public const string DefaultUpdateManifestBaseUrl = "http://192.168.0.151:8080";
        public const string DefaultUpdateManifestFilePath = "/update.json";

        #region Public properties

        /// <summary>
        ///     Gets or sets the base URL for the update manifest file.  Defaults to <see cref="DefaultUpdateManifestBaseUrl"/>.
        /// </summary>
        /// <example>
        ///     <code>"http://update.bdhero.org"</code>
        /// </example>
        public string UpdateManifestBaseUrl { get; set; }

        /// <summary>
        ///     Gets or sets the path portion of the update manifest file URL.  Defaults to <see cref="DefaultUpdateManifestFilePath"/>.
        /// </summary>
        /// <example>
        ///     <code>"http://update.bdhero.org"</code>
        /// </example>
        public string UpdateManifestFilePath { get; set; }

        /// <summary>
        ///     Gets or sets the current application version.
        /// </summary>
        public Version CurrentVersion { get; set; }

        /// <summary>
        ///     Gets or sets whether the application is portable or not.
        /// </summary>
        public bool IsPortable { get; set; }

        /// <summary>
        ///     <para>
        ///         Gets the latest update available for the current operating system and package type (portable vs. installed).
        ///     </para>
        ///     <para>
        ///         <strong>NOTE:</strong> The presence of a non-<c>null</c> value does <strong>NOT</strong> indicate that
        ///         a newer version is available; use <see cref="IsUpdateAvailable"/> for that.
        ///     </para>
        /// </summary>
        [CanBeNull]
        public Update LatestUpdate { get; private set; }

        /// <summary>
        ///     Gets whether a newer version of the application is available to download for the current operating system
        ///     and package type (portable vs. installed).
        /// </summary>
        public bool IsUpdateAvailable { get { return LatestUpdate != null && LatestUpdate.Version > CurrentVersion; } }

        #endregion

        #region Public events

        /// <summary>
        ///     Triggered <i>after</i> an update check is initiated by <see cref="CheckForUpdateAsync"/>, but <i>before</i>
        ///     any response is received from the server.
        /// </summary>
        /// <seealso cref="CheckForUpdateAsync"/>
        public event UpdaterV2CheckingForUpdateEventHandler Checking;

        /// <summary>
        ///     Triggered when a newer version of the application is available to download for the current operating system
        ///     and package type (portable vs. installed).
        /// </summary>
        /// <seealso cref="CheckForUpdateAsync"/>
        public event UpdaterV2UpdateFoundEventHandler UpdateFound;

        /// <summary>
        ///     Triggered when a newer version of the application is not available for the current operating system
        ///     and package type (portable vs. installed).
        /// </summary>
        /// <seealso cref="CheckForUpdateAsync"/>
        public event UpdaterV2UpdateNotFoundEventHandler UpdateNotFound;

        /// <summary>
        ///     Triggered when an update check initiated by <see cref="CheckForUpdateAsync"/> completes successfully,
        ///     regardless of whether a new version is available or not.  Does <b>not</b> trigger if an error occurs.
        /// </summary>
        /// <seealso cref="CheckForUpdateAsync"/>
        public event UpdaterV2CheckedEventHandler Checked;

        /// <summary>
        ///     Triggered whenever an error occurs while checking for an update.
        /// </summary>
        /// <seealso cref="CheckForUpdateAsync"/>
        public event UpdaterV2ErrorCheckingForUpdateEventHandler Error;

        #endregion

        private readonly ManualResetEventSlim _isChecking = new ManualResetEventSlim();

        /// <summary>
        ///     Constructs a new <see cref="UpdaterV2"/> instance.
        /// </summary>
        public UpdaterV2()
        {
            UpdateManifestBaseUrl = DefaultUpdateManifestBaseUrl;
            UpdateManifestFilePath = DefaultUpdateManifestFilePath;
            CurrentVersion = new Version();
            IsPortable = false;
        }

        /// <summary>
        ///     Checks for a newer version of the application for the current operating system
        ///     and package type (portable vs. installed) asynchronously.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Order of events:
        ///     </para>
        ///     <para>
        ///         <see cref="Checking"/> => [ <see cref="UpdateFound"/> | <see cref="UpdateNotFound"/> ] => [ <see cref="Checked"/> | <see cref="Error"/> ]
        ///     </para>
        /// </remarks>
        public void CheckForUpdateAsync()
        {
            // Already checking for an update
            if (_isChecking.IsSet)
                return;

            var client = new RestClient(UpdateManifestBaseUrl);
            var request = new RestRequest(UpdateManifestFilePath);
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

            if (Checked != null)
                Checked(this);
        }

        private void HandleError(IRestResponse response)
        {
            if (Error != null)
                Error(this, response.ErrorException);
        }
    }

    public delegate void UpdaterV2CheckingForUpdateEventHandler(UpdaterV2 updater);
    public delegate void UpdaterV2UpdateFoundEventHandler(UpdaterV2 updater);
    public delegate void UpdaterV2UpdateNotFoundEventHandler(UpdaterV2 updater);
    public delegate void UpdaterV2CheckedEventHandler(UpdaterV2 updater);
    public delegate void UpdaterV2ErrorCheckingForUpdateEventHandler(UpdaterV2 updater, Exception exception);
}
