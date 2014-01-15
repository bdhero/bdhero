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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BDHero.JobQueue;
using BDHero.Plugin;
using DotNetUtils;
using DotNetUtils.TaskUtils;

namespace BDHero
{
    public interface IController
    {
        #region Properties

        Job Job { get; }

        IList<IPlugin> PluginsByType { get; }

        #endregion

        #region Events

        /// <summary>
        /// Invoked immediately before the scanning stage starts.
        /// </summary>
        event TaskStartedEventHandler ScanStarted;

        /// <summary>
        /// Invoked when the scanning stage completes successfully.
        /// </summary>
        event TaskSucceededEventHandler ScanSucceeded;

        /// <summary>
        /// Invoked when the scanning stage aborts because of an error.
        /// </summary>
        event ExceptionEventHandler ScanFailed;

        /// <summary>
        /// Invoked when the scanning stage completes, regardless of whether the it succeeded or failed.
        /// This event always occurs after the <see cref="ScanSucceeded"/> and <see cref="ScanFailed"/> events.
        /// </summary>
        event TaskCompletedEventHandler ScanCompleted;

        /// <summary>
        /// Invoked immediately before the conversion stage starts.
        /// </summary>
        event TaskStartedEventHandler ConvertStarted;

        /// <summary>
        /// Invoked when the conversion stage completes successfully.
        /// </summary>
        event TaskSucceededEventHandler ConvertSucceeded;

        /// <summary>
        /// Invoked when the conversion stage aborts because of an error.
        /// </summary>
        event ExceptionEventHandler ConvertFailed;

        /// <summary>
        /// Invoked when the conversion stage completes, regardless of whether the it succeeded or failed.
        /// This event always occurs after the <see cref="ConvertSucceeded"/> and <see cref="ConvertFailed"/> events.
        /// </summary>
        event TaskCompletedEventHandler ConvertCompleted;

        /// <summary>
        /// Invoked whenever an <see cref="IPlugin"/>'s state or progress changes.
        /// </summary>
        event PluginProgressHandler PluginProgressUpdated;

        /// <summary>
        /// Invoked when an exception is thrown by an <see cref="IPlugin"/>.
        /// </summary>
        event UnhandledExceptionEventHandler UnhandledException;

        #endregion

        #region Methods

        /// <summary>
        /// Sets the <c>TaskScheduler</c> that will be used to invoke event callbacks.
        /// This ensures that events are always invoked from the appropriate thread.
        /// </summary>
        /// <param name="scheduler">Scheduler to use for event callbacks.  If none is specified, the calling thread's scheduler will be used.</param>
        void SetEventScheduler(TaskScheduler scheduler = null);

        /// <summary>
        /// Runs all <see cref="INameProviderPlugin"/>s synchronously.
        /// </summary>
        /// <param name="mkvPath"></param>
        void RenameSync(string mkvPath);

        /// <summary>
        /// Retrieves metadata, auto-detects the type of each playlist and track, and renames tracks and output file names.
        /// Same as <see cref="CreateScanTask"/>, except this method doesn't re-scan the BD-ROM, and it accepts custom callbacks.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="start">Callback that will be invoked before the task starts</param>
        /// <param name="fail">Callback that will be invoked if the task fails</param>
        /// <param name="succeed">Callback that will be invoked if the task succeeds</param>
        /// <param name="mkvPath">Optional path to the output directory or MKV file</param>
        /// <returns>Task that returns <c>true</c> if the task succeeded; otherwise <c>false</c></returns>
        Task<bool> CreateMetadataTask(CancellationToken cancellationToken, TaskStartedEventHandler start, ExceptionEventHandler fail, TaskSucceededEventHandler succeed, string mkvPath = null);

        /// <summary>
        /// Scans a BD-ROM, retrieves metadata, auto-detects the type of each playlist and track, and renames tracks and output file names.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="bdromPath">Path to the BD-ROM directory</param>
        /// <param name="mkvPath">Optional path to the output directory or MKV file</param>
        /// <returns>Task that returns <c>true</c> if the scan succeeded; otherwise <c>false</c></returns>
        Task<bool> CreateScanTask(CancellationToken cancellationToken, string bdromPath, string mkvPath = null);

        /// <summary>
        /// Muxes the BD-ROM to MKV and runs any post-processing plugins.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="mkvPath">Optional path to the MKV output file or directory (if overridden by the user)</param>
        /// <returns>Task that returns <c>true</c> if all muxing plugins succeeded; otherwise <c>false</c></returns>
        Task<bool> CreateConvertTask(CancellationToken cancellationToken, string mkvPath = null);

        #endregion
    }
}
