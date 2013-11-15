using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetUtils.Net
{
    /// <summary>
    /// HTTP file downloader that runs in a separate thread and reports its progress to observers.
    /// </summary>
    public class FileDownloader
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FileDownloadState State { get; private set; }

        public Exception Exception { get; private set; }

        /// <summary>
        /// URI of the remote Web resource to download.
        /// </summary>
        public string Uri;

        /// <summary>
        /// Path to where the download will be saved on disk.
        /// </summary>
        public string Path;

        /// <summary>
        /// Invoked just before all HTTP requests are made, allowing observers to modify requests before they are sent.
        ///  This can be useful to override the system's default proxy settings, set custom timeout values, etc.
        /// </summary>
        public event BeforeRequestEventHandler BeforeRequest;

        /// <summary>
        /// Execution context (a.k.a. thread) to invoke <see cref="ProgressChanged"/> notifications on.
        /// Defaults to the current thread's context if none is specified.
        /// </summary>
        public TaskScheduler CallbackThread;

        public CancellationToken CancellationToken;

        /// <summary>
        /// Invoked on the <c>TaskScheduler</c> specified by <see cref="CallbackThread"/> whenever the state or progress of the download changes.
        /// </summary>
        public event FileDownloadProgressChangedHandler ProgressChanged;

        private void NotifyBeforeRequest(HttpWebRequest request)
        {
            if (BeforeRequest != null)
                BeforeRequest(request);
        }

        /// <summary>
        /// Retrieves the value of the HTTP <c>Content-Length</c> response header
        /// by performing an HTTP <c>HEAD</c> request for <see cref="Uri"/>.
        /// </summary>
        /// <returns>Size of the download in bytes</returns>
        public long GetContentLength()
        {
            var request = HttpRequest.BuildRequest(HttpRequestMethod.Head, Uri);

            NotifyBeforeRequest(request);

            using (var response = request.GetResponse())
            {
                return response.ContentLength;
            }
        }

        /// <summary>
        /// Streams the remote resource to the local file.
        /// </summary>
        public void DownloadSync()
        {
            var request = HttpRequest.BuildRequest(HttpRequestMethod.Get, Uri);

            NotifyBeforeRequest(request);

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var fileStream = File.Open(Path, FileMode.Create))
            {
                // Default buffer size in .NET is 8 KiB.
                // http://stackoverflow.com/a/1863003/467582
                const int bufferSize = 1024 * 8;

                var buffer = new byte[bufferSize];
                int bytesRead;
                var fileSize = 0;

                Tick(fileSize, force: true);

                State = FileDownloadState.Downloading;
                NotifyProgressChanged(fileSize, response.ContentLength);

                // TODO: Wrap in try/catch and set Exception and State properties

                do
                {
                    Tick(fileSize);

                    if (CancellationToken.IsCancellationRequested)
                    {
                        State = FileDownloadState.Canceled;
                        NotifyProgressChanged(fileSize, response.ContentLength);
                        return;
                    }

                    bytesRead = responseStream.Read(buffer, 0, bufferSize);
                    fileSize += bytesRead;

                    fileStream.Write(buffer, 0, bytesRead);

                    if (bytesRead > 0)
                        NotifyProgressChanged(fileSize, response.ContentLength);
                } while (bytesRead > 0);

                fileStream.Close();

                if (fileSize != response.ContentLength)
                {
                    State = FileDownloadState.Error;
                    Exception = new IOException("Number of bytes received (" + fileSize + ") does not match Content-Length (" + response.ContentLength + ")");
                    throw Exception;
                }

                var length = new FileInfo(Path).Length;
                if (length != response.ContentLength)
                {
                    State = FileDownloadState.Error;
                    Exception = new IOException("Number of bytes written to disk (" + length + ") does not match Content-Length (" + response.ContentLength + ")");
                    throw Exception;
                }

                State = FileDownloadState.Success;
                NotifyProgressChanged(fileSize, response.ContentLength);
            }
        }

        private bool HasEnoughTimeElapsed { get { return (DateTime.Now - _lastTick).TotalMilliseconds > 100; } }

        private void Tick(int fileSize, bool force = false)
        {
            if (!HasEnoughTimeElapsed && !force) return;
            _lastTick = DateTime.Now;
            _lastFileSize = fileSize;
        }

        private DateTime _lastTick;
        private int _lastFileSize;

        private void NotifyProgressChanged(int fileSize, long contentLength)
        {
            var @continue = HasEnoughTimeElapsed || _lastFileSize == 0 || fileSize >= contentLength;
            if (!@continue) return;

            var fileSizeDelta = (fileSize - _lastFileSize);
            var timeSpan = (DateTime.Now - _lastTick);
            var bytesPerSecond = fileSizeDelta / timeSpan.TotalSeconds;

            if (timeSpan.TotalSeconds == 0)
            {
                Logger.Error("Not enough time between notifications to generate meaningful data");
                return;
            }

            var progress = new FileDownloadProgress(State, fileSize, contentLength, bytesPerSecond * 8);

            Console.WriteLine(progress);

            if (ProgressChanged != null)
            {
                var thread = CallbackThread
                                ?? (SynchronizationContext.Current != null
                                  ? TaskScheduler.FromCurrentSynchronizationContext()
                                  : TaskScheduler.Default);
                Task.Factory.StartNew(() => ProgressChanged(progress), CancellationToken.None, TaskCreationOptions.None, thread);
                ProgressChanged(progress);
            }
        }
    }
}
