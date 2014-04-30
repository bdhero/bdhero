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

using System.Text.RegularExpressions;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.Extensions;
using DotNetUtils.FS;
using OSUtils.JobObjects;
using ProcessUtils;

namespace BDHero.Plugin.MkvMergeMuxer
{
    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
    /// TODO: Switch to BackgroundProcessWorker
// ReSharper disable LocalizableElement
// ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
// ReSharper restore RedundantNameQualifier
// ReSharper restore LocalizableElement
    public class MkvMerge : BackgroundProcessWorker
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITempFileRegistrar _tempFileRegistrar;

        public MkvMerge(Job job, Playlist playlist, string outputMKVPath, IJobObjectManager jobObjectManager, ITempFileRegistrar tempFileRegistrar)
            : base(jobObjectManager)
        {
            _tempFileRegistrar = tempFileRegistrar;

            var cli = new MkvMergeCLI(Arguments, tempFileRegistrar)
                .SetOutputPath(outputMKVPath)
                .SetSelectedTracks(playlist)
                .NoGlobalTags()
                .NoTrackTags()
                .SetInputPath(playlist.FullPath)
                .AttachCoverArt(job.SelectedReleaseMedium)
                .SetMovieTitle(job)
                .SetChapters(playlist.Chapters)
                ;

            ExePath = cli.ExePath;

            StdOut += HandleOutputLine;
            StdErr += HandleOutputLine;
            Exited += (state, code, exception, time) => OnExited(state, code, job.SelectedReleaseMedium, playlist, outputMKVPath);

//            CleanExit = false;
        }

        private void OnExited(NonInteractiveProcessState state, int exitCode, ReleaseMedium releaseMedium, Playlist playlist, string outputMKVPath)
        {
//            _tempFileRegistrar.DeleteTempFiles("", "", "");
        }

        private void HandleOutputLine(string line)
        {
            const string progressRegex = @"^Progress: ([\d\.]+)\%";
            const string errorRegex = @"^Error:";

            if (string.IsNullOrWhiteSpace(line))
                return;

            if (Regex.IsMatch(line, progressRegex))
            {
                var match = Regex.Match(line, progressRegex);
                match.Groups[1].Value.TryParseDoubleInvariant(out _progress);
            }
            else if (Regex.IsMatch(line, errorRegex))
            {
//                _errorMessages.Add(line);
                Logger.Error(line);
            }
        }
    }
}
