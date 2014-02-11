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
using System.Threading;

namespace DotNetUtils.TaskUtils
{
    /// <summary>
    /// Defines methods to invoke actions on a specific thread.
    /// </summary>
    public interface IThreadInvoker
    {
        /// <summary>
        /// Invokes the given action on the UI thread and blocks until the action completes executing.
        /// </summary>
        /// <param name="action">Action to perform on the UI thread</param>
        void InvokeOnUIThreadSync(ThreadAction action);

        /// <summary>
        /// Invokes the given action on the UI thread and returns immediately without waiting for the action to complete executing.
        /// </summary>
        /// <param name="action">Action to perform on the UI thread</param>
        void InvokeOnUIThreadAsync(ThreadAction action);
    }
}