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
using System.ComponentModel;
using System.Text.RegularExpressions;
using DotNetUtils.Extensions;
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
    public class MkvMerge
    {
        private readonly string _inputM2TsPath;
        private readonly string _inputMkvPath;
        private readonly string _inputChaptersPath;
        private readonly string _outputMkvPath;
        private readonly bool _keepM2TsAudio;

        private double _progress;
        private readonly List<String> _errorMessages = new List<string>();

        string Name { get { return "MkvMerge"; } }
        string Filename { get { return "mkvmerge.exe"; } }

        public MkvMerge(string inputM2TsPath, string inputMkvPath, string inputChaptersPath, string outputMkvPath, bool keepM2TsAudio = true)
        {
            _inputM2TsPath = inputM2TsPath;
            _inputMkvPath = inputMkvPath;
            _inputChaptersPath = inputChaptersPath;
            _outputMkvPath = outputMkvPath;
            _keepM2TsAudio = keepM2TsAudio;
        }

        public void Mux(object sender, DoWorkEventArgs e)
        {
            var inputM2TsFlags = _keepM2TsAudio ? null : "--no-audio";
            var inputMkvFlags = _keepM2TsAudio ? "--no-audio" : null;

            var args = new ArgumentList();

            // Chapter file
            args.AddIfAllNonEmpty("--chapters", _inputChaptersPath);

            // Output file
            args.AddAll("-o", _outputMkvPath);

            // Input M2TS file
            args.AddNonEmpty("--no-video", inputM2TsFlags, _inputM2TsPath);

            // If an input chapter file is specified, exclude chapters from the input MKV file
            if (!string.IsNullOrEmpty(_inputChaptersPath))
                args.Add("--no-chapters");

            // Input MKV file
            args.AddNonEmpty(inputMkvFlags, _inputMkvPath);
            
            Execute(args, sender, e);
        }

        private void Execute(ArgumentList args, object sender, DoWorkEventArgs e)
        {
        }

        protected void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            const string progressRegex = @"^Progress: ([\d\.]+)\%";
            const string errorRegex = @"^Error:";

            if (Regex.IsMatch(line, progressRegex))
            {
                var match = Regex.Match(line, progressRegex);
                match.Groups[1].Value.TryParseDoubleInvariant(out _progress);
            }
            else if (Regex.IsMatch(line, errorRegex))
            {
                _errorMessages.Add(line);
            }
        }

        protected ISet<string> GetOutputFilesImpl()
        {
            return new HashSet<string> { _outputMkvPath };
        }
    }
}
