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
