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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OSUtils.JobObjects
{
    /// <summary>
    ///     Interface for a Windows Job Object.
    /// </summary>
    public interface IJobObject : IDisposable
    {
        /// <summary>
        ///     Assigns the given <paramref name="process"/> to this job object.
        /// </summary>
        /// <param name="process"></param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="process"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <paramref name="process"/> already belongs to another job group.
        ///     (See http://stackoverflow.com/a/4232259/3205 for more information.)
        /// </exception>
        /// <exception cref="Win32Exception">
        ///     Thrown if the operating system was unable to assign <paramref name="process"/> to this job object.
        /// </exception>
        void Assign(Process process);

        /// <summary>
        ///     Instructs the operating system to terminate all child processes belonging to this job object
        ///     when the parent process (i.e., the current process) terminates.
        /// </summary>
        void KillOnClose();
    }
}
