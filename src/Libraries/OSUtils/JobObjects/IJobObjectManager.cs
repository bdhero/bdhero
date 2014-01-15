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

using System.Diagnostics;

namespace OSUtils.JobObjects
{
    /// <summary>
    ///     Interface for a high-level manager class that creates and queries Windows Job Objects.
    /// </summary>
    public interface IJobObjectManager
    {
        /// <summary>
        ///     Creates a new instance of a class that implements the <see cref="IJobObject"/> interface
        ///     for the current operating system.
        /// </summary>
        /// <returns>OS-specific <see cref="IJobObject"/> instance.</returns>
        IJobObject CreateJobObject();

        /// <summary>
        ///     Determines if the given <paramref name="process"/> is associated with a Windows Job Object.
        /// </summary>
        /// <param name="process">
        ///     The process to check.
        /// </param>
        /// <returns>
        ///     <c>true</c> if <paramref name="process"/> belongs to a Job Object; otherwise <c>false</c>.
        /// </returns>
        bool IsAssignedToJob(Process process);

        /// <summary>
        ///     Checks if the current process belongs to a Job Object and, if so, attempts to start a new process
        ///     that does not belong to a Job Object.
        /// </summary>
        /// <param name="args">
        ///     Arguments that were passed to the startup program's <c>Main()</c> method.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the current process belongs to a Job Object and a new child process
        ///     was successfully started outside the current Job; otherwise <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This method can be used to break the current process out of a PCA (Program Compatibility Assistant)
        ///         Job Object that is automatically created by the OS when an application runs on a newer
        ///         version of Windows than its application manifest indicates compatibility with
        ///         by spawning a new child process that does not belong to the current Job.
        ///         The spawned process will not inherit the current parent process' Job Object,
        ///         nor will it belong to any Job Object at all.
        ///     </para>
        ///     <para>
        ///         This method should be called from the application's <c>Main()</c> method
        ///         <em>before</em> executing any business logic.
        ///     </para>
        ///     <para>
        ///         The caller should <strong>immediately terminate</strong> the application if this method returns <c>true</c>.
        ///     </para>
        ///     <para>
        ///         This method will <em>not</em> work with Visual Studio's <c>vhost.exe</c> debugging process.
        ///     </para>
        /// </remarks>
        /// <seealso cref="http://blogs.msdn.com/b/cjacks/archive/2009/07/10/how-to-work-around-program-compatibility-assistant-pca-jobobjects-interfering-with-your-jobobjects.aspx"/>
        /// <seealso cref="http://blogs.msdn.com/b/alejacma/archive/2012/03/09/why-is-my-process-in-a-job-if-i-didn-t-put-it-there.aspx"/>
        bool TryBypassPCA(string[] args);
    }
}
