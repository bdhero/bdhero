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

using DotNetUtils.FS;

namespace DotNetUtils.Net
{
    /// <summary>
    /// Represents the current progess state of a <see cref="FileDownloader"/>.
    /// </summary>
    public class FileDownloadProgress
    {
        /// <summary>
        /// Gets or sets the state of the file download.
        /// </summary>
        public readonly FileDownloadState State;

        /// <summary>
        /// Number of bytes downloaded so far.
        /// </summary>
        public readonly int BytesDownloaded;

        /// <summary>
        /// Total expected file size of the download based on the server's Content-Length HTTP response header.
        /// </summary>
        public readonly long ContentLength;

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        public readonly double PercentComplete;

        /// <summary>
        /// Download speed measured in bits per second.
        /// </summary>
        public readonly double BitsPerSecond;

        /// <summary>
        /// Download speed measured in bytes per second.
        /// </summary>
        public readonly double BytesPerSecond;

        /// <summary>
        /// Human-readable download speed in bytes per second (e.g., "29.8 KiB/s", "2.4 MiB/s").
        /// </summary>
        public readonly string HumanSpeed;

        /// <summary>
        /// Gets whether the download has completed successfully.
        /// </summary>
        public readonly bool IsComplete;

        /// <summary>
        /// Initializes a new <c>FileDownloadProgress</c> object in the <see cref="FileDownloadState.Ready"/> state
        /// with all other values initialized to zero (<c>0</c>).
        /// </summary>
        public FileDownloadProgress() : this(FileDownloadState.Ready, 0, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new <c>FileDownloadProgress</c> object with the given values.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="bytesDownloaded"></param>
        /// <param name="contentLength"></param>
        /// <param name="bitsPerSecond"></param>
        public FileDownloadProgress(FileDownloadState state, int bytesDownloaded, long contentLength, double bitsPerSecond)
        {
            State = state;
            BytesDownloaded = bytesDownloaded;
            ContentLength = contentLength;
            BitsPerSecond = bitsPerSecond;
            BytesPerSecond = BitsPerSecond / 8;
            PercentComplete = 100.0 * ((double)BytesDownloaded / ContentLength);
            HumanSpeed = string.Format("{0}/s", FileUtils.HumanFriendlyFileSize((long)BytesPerSecond));
            IsComplete = (BytesDownloaded == ContentLength);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1:N0} of {2:N0} bytes downloaded ({3:P}) @ {4:N0} bits/sec ({5})",
                                 State,
                                 BytesDownloaded,
                                 ContentLength,
                                 PercentComplete / 100.0,
                                 BitsPerSecond,
                                 HumanSpeed
                );
        }
    }
}