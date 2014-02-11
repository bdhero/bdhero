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

namespace DotNetUtils.Net
{
    /// <summary>
    /// Represents the current state of a <see cref="FileDownloader"/>.
    /// </summary>
    public enum FileDownloadState
    {
        /// <summary>
        /// Download has not started yet.
        /// </summary>
        Ready,

        /// <summary>
        /// Download is in progress.
        /// </summary>
        Downloading,

        /// <summary>
        /// Download is in progress, but is currently paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Download was canceled by the user.
        /// </summary>
        Canceled,

        /// <summary>
        /// An exception occurred while downloading the file.
        /// </summary>
        Error,

        /// <summary>
        /// The download completed successfully.
        /// </summary>
        Success
    }
}