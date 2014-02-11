// Copyright 2014 Andrew C. Dvorak
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
    ///     Interface for a class that monitors the system's network status and checks for Internet connectivity.
    /// </summary>
    public interface INetworkStatusMonitor
    {
        /// <summary>
        ///     Gets whether the computer is connected to the Internet or not.
        /// </summary>
        bool IsOnline { get; }

        /// <summary>
        ///     Triggered whenever the system's network status changes.
        /// </summary>
        event NetworkStatusChangedEventHandler NetworkStatusChanged;

        /// <summary>
        ///     Asynchronously tests whether the system is connected to the Internet
        ///     and notifies observers via the <see cref="NetworkStatusChanged"/> event.
        /// </summary>
        void TestConnectionAsync();
    }
}