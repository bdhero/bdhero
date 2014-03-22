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
using I18N;

namespace BDHero.BDROM
{
    public class Chapter
    {
        private string _title;

        #region DB Fields

        /// <summary>
        /// Gets the 1-based index of the chapter.
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Gets the chapter's entry point.
        /// </summary>
        public TimeSpan StartTime { get; private set; }

        /// <summary>
        /// Gets or sets the language of the chapter title.
        /// </summary>
        public Language Language;

        /// <summary>
        /// Gets or sets the descriptive name of the chapter.
        /// </summary>
        public string Title
        {
            get { return _title ?? "Chapter " + Number; }
            set { _title = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets whether the chapter should be included in the resulting MKV file.
        /// </summary>
        public bool Keep = true;

        #region Constructor

        public Chapter(int number, double seconds)
        {
            Number = number;
            StartTime = new TimeSpan((long)(seconds * TimeSpan.TicksPerSecond));
        }

        #endregion

        #region Non-DB Properties (StartTimeXmlFormat)

        /// <summary>
        /// StartTime in Matroska XML format (e.g., "HH:MM:SS:mmm"
        /// </summary>
        public string StartTimeXmlFormat
        {
            get
            {
                return string.Format(
                        "{0}:{1}:{2}.{3}",
                        StartTime.Hours.ToString("00"),
                        StartTime.Minutes.ToString("00"),
                        StartTime.Seconds.ToString("00"),
                        StartTime.Milliseconds.ToString("000")
                    );
            }
        }

        #endregion

        #region Method overrides

        public override string ToString()
        {
            return string.Format("[{0}] {1:D2} - {2:G}: {3}", Keep ? "x" : "  ", Number, StartTime, Title);
        }

        #endregion
    }
}
