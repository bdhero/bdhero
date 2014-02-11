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

using System.Threading;

namespace BDHero.Plugin
{
    /// <summary>
    /// <para>
    /// Wrapper for an <see cref="IPluginHost"/>, <see cref="IPlugin"/>, and <see cref="CancellationToken"/>
    /// that allows all 3 to be passed around as a single object instead of being passed separately.
    /// </para>
    /// </summary>
    public class ProgressToken
    {
        private readonly CancellationToken _token;

        private readonly IPlugin _plugin;
        private readonly IPluginHost _host;

        /// <summary>
        /// Gets whether the user has requested to cancel the current operation.
        /// </summary>
        public bool IsCancelled { get { return _token.IsCancellationRequested; } }

        /// <summary>
        /// Constructs a new ProgressToken that does <strong>not</strong> report its progress
        /// and <strong>cannot</strong> be cancelled.
        /// </summary>
        public ProgressToken() : this(null, null, CancellationToken.None)
        {
        }

        /// <summary>
        /// Constructs a new ProgressToken that does reports its progress to the <paramref name="host"/>
        /// and may be cancelled via the given <paramref name="token"/>.
        /// </summary>
        public ProgressToken(IPluginHost host, IPlugin plugin, CancellationToken token)
        {
            _host = host;
            _token = token;
            _plugin = plugin;
        }

        public void ReportProgress(double percentComplete, string status)
        {
            if (_host != null)
                _host.ReportProgress(_plugin, percentComplete, status);
        }
    }
}