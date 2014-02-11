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
using System.Runtime.InteropServices;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace WindowsOSUtils.JobObjects
{
    #region Win32 API calls (P/Invoke)

    /// <summary>
    ///     Public class that holds all the Windows API calls necessary to manage Windows Job Objects.
    /// </summary>
    /// <seealso cref="https://www-auth.cs.wisc.edu/lists/htcondor-users/2009-June/msg00106.shtml" />
    internal static class WinAPI
    {
        /// <summary>
        ///     The structures returned by Windows are different sizes depending on whether
        ///     the operating system is running in 32-bit or 64-bit mode.
        /// </summary>
        public static readonly bool Is32Bit = (IntPtr.Size == 4);

        /// <summary>
        ///     The IsProcessInJob function determines if the process is running in the specified job.
        /// </summary>
        /// <param name="processHandle">
        ///     Handle to the process to be tested.
        ///     The handle must have the PROCESS_QUERY_INFORMATION access right.
        ///     For more information, see Process Security and Access Rights.
        /// </param>
        /// <param name="jobHandle">
        ///     Handle to the job. If this parameter is NULL,
        ///     the function tests if the process is running under any job.  If this
        ///     parameter is not NULL, the handle must have the JOB_OBJECT_QUERY access
        ///     right. For more information, see Job Object Security and Access Rights.
        /// </param>
        /// <param name="result">
        ///     Pointer to a value that receives TRUE if the process
        ///     is running in the job, and FALSE otherwise.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero. To get extended error information,
        ///     call GetLastError.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool IsProcessInJob(
            IntPtr processHandle,
            IntPtr jobHandle,
            [Out] out bool result);

        /// <summary>
        ///     The CreateJobObject function creates or opens a job object.
        /// </summary>
        /// <param name="jobAttributes">
        ///     Pointer to a SECURITY_ATTRIBUTES structure that
        ///     specifies the security descriptor for the job object and determines whether
        ///     child processes can inherit the returned handle. If lpJobAttributes is NULL,
        ///     the job object gets a default security descriptor and the handle cannot be inherited.
        ///     The ACLs in the default security descriptor for a job object come from the primary
        ///     or impersonation token of the creator.
        /// </param>
        /// <param name="name">
        ///     Pointer to a null-terminated string specifying the name of the
        ///     job. The name is limited to MAX_PATH characters. Name comparison is case-sensitive.
        ///     If lpName is NULL, the job is created without a name.   If lpName matches the name
        ///     of an existing event, semaphore, mutex, waitable timer, or file-mapping object, the
        ///     function fails and the GetLastError function returns ERROR_INVALID_HANDLE. This
        ///     occurs because these objects share the same name space.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is a handle to the job object.
        ///     The handle has the JOB_OBJECT_ALL_ACCESS access right. If the object existed before
        ///     the function call, the function returns a handle to the existing job object and
        ///     GetLastError returns ERROR_ALREADY_EXISTS.  If the function fails, the return value
        ///     is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateJobObject(
            IntPtr jobAttributes,
            string name);

        /// <summary>
        ///     Creates or opens a job object.
        /// </summary>
        /// <param name="lpJobAttributes">
        ///     A pointer to a SECURITY_ATTRIBUTES structure that specifies the security descriptor for the job object
        ///     and determines whether child processes can inherit the returned handle. If lpJobAttributes is NULL,
        ///     the job object gets a default security descriptor and the handle cannot be inherited.
        ///     The ACLs in the default security descriptor for a job object come from the primary or impersonation token of the
        ///     creator.
        /// </param>
        /// <param name="lpName">
        ///     <para>The name of the job. The name is limited to MAX_PATH characters. Name comparison is case-sensitive.</para>
        ///     <para>If lpName is NULL, the job is created without a name.</para>
        ///     <para>
        ///         If lpName matches the name of an existing event, semaphore, mutex, waitable timer, or file-mapping object,
        ///         the function fails and the GetLastError function returns ERROR_INVALID_HANDLE. This occurs because these
        ///         objects share the same namespace.
        ///     </para>
        ///     <para>The object can be created in a private namespace. For more information, see Object Namespaces.</para>
        ///     <para>
        ///         Terminal Services:  The name can have a "Global\" or "Local\" prefix to explicitly create the object in the
        ///         global or session namespace. The remainder of the name can contain any character except the backslash character
        ///         (\). For more information, see Kernel Object Namespaces.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         If the function succeeds, the return value is a handle to the job object. The handle has the
        ///         JOB_OBJECT_ALL_ACCESS access right. If the object existed before the function call, the function returns a
        ///         handle to the existing job object and GetLastError returns ERROR_ALREADY_EXISTS.
        ///     </para>
        ///     <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When a job is created, its accounting information is initialized to zero, all limits are inactive, and there
        ///         are no associated processes. To assign a process to a job object, use the AssignProcessToJobObject function. To
        ///         set limits for a job, use the SetInformationJobObject function. To query accounting information, use the
        ///         QueryInformationJobObject function.
        ///     </para>
        ///     <para>
        ///         All processes associated with a job must run in the same session. A job is associated with the session of the
        ///         first process to be assigned to the job.
        ///     </para>
        ///     <para>Windows Server 2003 and Windows XP:  A job is associated with the session of the process that created it.</para>
        ///     <para>
        ///         To close a job object handle, use the CloseHandle function. The job is destroyed when its last handle has
        ///         been closed and all associated processes have exited. However, if the job has the
        ///         JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE flag specified, closing the last job object handle terminates all associated
        ///         processes and then destroys the job object itself.
        ///     </para>
        /// </remarks>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr CreateJobObject(
            [In] ref SECURITY_ATTRIBUTES lpJobAttributes,
            string lpName);

        /// <summary>
        ///     The AssignProcessToJobObject function assigns a process to an existing job object.
        /// </summary>
        /// <param name="jobHandle">
        ///     Handle to the job object to which the process will be
        ///     associated.  The CreateJobObject or OpenJobObject function returns this handle.
        ///     The handle must have the JOB_OBJECT_ASSIGN_PROCESS access right. For more
        ///     information, see Job Object Security and Access Rights.
        /// </param>
        /// <param name="processHandle">
        ///     Handle to the process to associate with the job object.
        ///     The process must not already be assigned to a job. The handle must have the
        ///     PROCESS_SET_QUOTA and PROCESS_TERMINATE access rights. For more information,
        ///     see Process Security and Access Rights.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.  If the function
        ///     fails, the return value is zero. To get extended error information, call
        ///     GetLastError.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AssignProcessToJobObject(
            IntPtr jobHandle,
            IntPtr processHandle);

        /// <summary>
        ///     The SetInformationJobObject function sets limits for a job object.
        /// </summary>
        /// <param name="jobHandle">
        ///     Handle to the job whose limits are being set.
        ///     The CreateJobObject or OpenJobObject function returns this handle. The handle must
        ///     have the JOB_OBJECT_SET_ATTRIBUTES access right. For more information, see Job
        ///     Object Security and Access Rights.
        /// </param>
        /// <param name="jobObjectInfoClass">
        ///     Information class for the limits to be set. This
        ///     parameter can be one of the following values.
        /// </param>
        /// <param name="jobObjectInfo">
        ///     Limits to be set for the job. The format of this data
        ///     depends on the value of JobObjectInfoClass.
        /// </param>
        /// <param name="jobObjectInfoLength">
        ///     Size of the job information being set, in
        ///     bytes.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.  If the function
        ///     fails, the return value is zero. To get extended error information,
        ///     call GetLastError.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetInformationJobObject(
            [In] IntPtr jobHandle,
            [In] JobObjectInfoClass jobObjectInfoClass,
            [In] ref JobObjectInfo jobObjectInfo,
            [In] uint jobObjectInfoLength);

        /// <summary>
        ///     <para>
        ///         Creates a new process and its primary thread. The new process runs in the security context of the calling
        ///         process.
        ///     </para>
        ///     <para>
        ///         If the calling process is impersonating another user, the new process uses the token for the calling process,
        ///         not the impersonation token. To run the new process in the security context of the user represented by the
        ///         impersonation token, use the CreateProcessAsUser or CreateProcessWithLogonW function.
        ///     </para>
        /// </summary>
        /// <param name="lpApplicationName">
        ///     <para>
        ///         The name of the module to be executed. This module can be a Windows-based application. It can be some other
        ///         type of module (for example, MS-DOS or OS/2) if the appropriate subsystem is available on the local computer.
        ///     </para>
        ///     <para>
        ///         The string can specify the full path and file name of the module to execute or it can specify a partial name.
        ///         In the case of a partial name, the function uses the current drive and current directory to complete the
        ///         specification. The function will not use the search path. This parameter must include the file name extension;
        ///         no default extension is assumed.
        ///     </para>
        ///     <para>
        ///         The lpApplicationName parameter can be NULL. In that case, the module name must be the first white
        ///         space–delimited token in the lpCommandLine string. If you are using a long file name that contains a space, use
        ///         quoted strings to indicate where the file name ends and the arguments begin; otherwise, the file name is
        ///         ambiguous. For example, consider the string "c:\program files\sub dir\program name". This string can be
        ///         interpreted in a number of ways. The system tries to interpret the possibilities in the following order:
        ///     </para>
        ///     <list>
        ///         <item>c:\program.exe files\sub dir\program name</item>
        ///         <item>c:\program files\sub.exe dir\program name</item>
        ///         <item>c:\program files\sub dir\program.exe name</item>
        ///         <item>c:\program files\sub dir\program name.exe</item>
        ///     </list>
        ///     <para>
        ///         If the executable module is a 16-bit application, lpApplicationName should be NULL, and the string pointed to
        ///         by lpCommandLine should specify the executable module as well as its arguments.
        ///     </para>
        ///     <para>
        ///         To run a batch file, you must start the command interpreter; set lpApplicationName to cmd.exe and set
        ///         lpCommandLine to the following arguments: /c plus the name of the batch file.
        ///     </para>
        /// </param>
        /// <param name="lpCommandLine">
        ///     <para>
        ///         The command line to be executed. The maximum length of this string is 32,768 characters, including the
        ///         Unicode terminating null character. If lpApplicationName is NULL, the module name portion of lpCommandLine is
        ///         limited to MAX_PATH characters.
        ///     </para>
        ///     <para>
        ///         The Unicode version of this function, CreateProcessW, can modify the contents of this string. Therefore, this
        ///         parameter cannot be a pointer to read-only memory (such as a const variable or a literal string). If this
        ///         parameter is a constant string, the function may cause an access violation.
        ///     </para>
        ///     <para>
        ///         The lpCommandLine parameter can be NULL. In that case, the function uses the string pointed to by
        ///         lpApplicationName as the command line.
        ///     </para>
        ///     <para>
        ///         If both lpApplicationName and lpCommandLine are non-NULL, the null-terminated string pointed to by
        ///         lpApplicationName specifies the module to execute, and the null-terminated string pointed to by lpCommandLine
        ///         specifies the command line. The new process can use GetCommandLine to retrieve the entire command line. Console
        ///         processes written in C can use the argc and argv arguments to parse the command line. Because argv[0] is the
        ///         module name, C programmers generally repeat the module name as the first token in the command line.
        ///     </para>
        ///     <para>
        ///         If lpApplicationName is NULL, the first white space–delimited token of the command line specifies the module
        ///         name. If you are using a long file name that contains a space, use quoted strings to indicate where the file
        ///         name ends and the arguments begin (see the explanation for the lpApplicationName parameter). If the file name
        ///         does not contain an extension, .exe is appended. Therefore, if the file name extension is .com, this parameter
        ///         must include the .com extension. If the file name ends in a period (.) with no extension, or if the file name
        ///         contains a path, .exe is not appended. If the file name does not contain a directory path, the system searches
        ///         for the executable file in the following sequence:
        ///     </para>
        ///     <list>
        ///         <item>The directory from which the application loaded.</item>
        ///         <item>The current directory for the parent process.</item>
        ///         <item>
        ///             The 32-bit Windows system directory. Use the GetSystemDirectory function to get the path of this
        ///             directory.
        ///         </item>
        ///         <item>
        ///             The 16-bit Windows system directory. There is no function that obtains the path of this directory, but it
        ///             is
        ///             searched. The name of this directory is System.
        ///         </item>
        ///         <item>The Windows directory. Use the GetWindowsDirectory function to get the path of this directory.</item>
        ///         <item>
        ///             The directories that are listed in the PATH environment variable. Note that this function does not search
        ///             the
        ///             per-application path specified by the App Paths registry key. To include this per-application path in the
        ///             search sequence, use the ShellExecute function.
        ///         </item>
        ///     </list>
        ///     <para>
        ///         The system adds a terminating null character to the command-line string to separate the file name from the
        ///         arguments. This divides the original string into two strings for public processing.
        ///     </para>
        /// </param>
        /// <param name="lpProcessAttributes">
        ///     <para>
        ///         A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle to the new process
        ///         object can be inherited by child processes. If lpProcessAttributes is NULL, the handle cannot be inherited.
        ///     </para>
        ///     <para>
        ///         The lpSecurityDescriptor member of the structure specifies a security descriptor for the new process. If
        ///         lpProcessAttributes is NULL or lpSecurityDescriptor is NULL, the process gets a default security descriptor.
        ///         The ACLs in the default security descriptor for a process come from the primary token of the creator.
        ///     </para>
        ///     <para>
        ///         Windows XP:  The ACLs in the default security descriptor for a process come from the primary or impersonation
        ///         token of the creator. This behavior changed with Windows XP with SP2 and Windows Server 2003.
        ///     </para>
        /// </param>
        /// <param name="lpThreadAttributes">
        ///     A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle to the new thread object
        ///     can be inherited by child processes. If lpThreadAttributes is NULL, the handle cannot be inherited.
        ///     <para>
        ///         The lpSecurityDescriptor member of the structure specifies a security descriptor for the main thread. If
        ///         lpThreadAttributes is NULL or lpSecurityDescriptor is NULL, the thread gets a default security descriptor. The
        ///         ACLs in the default security descriptor for a thread come from the process token.
        ///     </para>
        ///     <para>
        ///         Windows XP:  The ACLs in the default security descriptor for a thread come from the primary or impersonation
        ///         token of the creator. This behavior changed with Windows XP with SP2 and Windows Server 2003.
        ///     </para>
        /// </param>
        /// <param name="bInheritHandles">
        ///     If this parameter TRUE, each inheritable handle in the calling process is inherited by the new process. If the
        ///     parameter is FALSE, the handles are not inherited. Note that inherited handles have the same value and access
        ///     rights as the original handles.
        /// </param>
        /// <param name="dwCreationFlags">
        ///     The flags that control the priority class and the creation of the process. For a list of values, see Process
        ///     Creation Flags.
        ///     <para>
        ///         This parameter also controls the new process's priority class, which is used to determine the scheduling
        ///         priorities of the process's threads. For a list of values, see GetPriorityClass. If none of the priority class
        ///         flags is specified, the priority class defaults to NORMAL_PRIORITY_CLASS unless the priority class of the
        ///         creating process is IDLE_PRIORITY_CLASS or BELOW_NORMAL_PRIORITY_CLASS. In this case, the child process
        ///         receives the default priority class of the calling process.
        ///     </para>
        /// </param>
        /// <param name="lpEnvironment">
        ///     A pointer to the environment block for the new process. If this parameter is NULL, the new process uses the
        ///     environment of the calling process.
        ///     <para>
        ///         An environment block consists of a null-terminated block of null-terminated strings. Each string is in the
        ///         following form:
        ///     </para>
        ///     <code>name=value\0</code>
        ///     <para>Because the equal sign is used as a separator, it must not be used in the name of an environment variable.</para>
        ///     <para>
        ///         An environment block can contain either Unicode or ANSI characters. If the environment block pointed to by
        ///         lpEnvironment contains Unicode characters, be sure that dwCreationFlags includes CREATE_UNICODE_ENVIRONMENT. If
        ///         this parameter is NULL and the environment block of the parent process contains Unicode characters, you must
        ///         also ensure that dwCreationFlags includes CREATE_UNICODE_ENVIRONMENT.
        ///     </para>
        ///     <para>
        ///         The ANSI version of this function, CreateProcessA fails if the total size of the environment block for the
        ///         process exceeds 32,767 characters.
        ///     </para>
        ///     <para>
        ///         Note that an ANSI environment block is terminated by two zero bytes: one for the last string, one more to
        ///         terminate the block. A Unicode environment block is terminated by four zero bytes: two for the last string, two
        ///         more to terminate the block.
        ///     </para>
        /// </param>
        /// <param name="lpCurrentDirectory">
        ///     The full path to the current directory for the process. The string can also specify a UNC path.
        ///     <para>
        ///         If this parameter is NULL, the new process will have the same current drive and directory as the calling
        ///         process. (This feature is provided primarily for shells that need to start an application and specify its
        ///         initial drive and working directory.)
        ///     </para>
        /// </param>
        /// <param name="lpStartupInfo">
        ///     A pointer to a STARTUPINFO or STARTUPINFOEX structure.
        ///     <para>
        ///         To set extended attributes, use a STARTUPINFOEX structure and specify EXTENDED_STARTUPINFO_PRESENT in the
        ///         dwCreationFlags parameter.
        ///     </para>
        ///     <para>Handles in STARTUPINFO or STARTUPINFOEX must be closed with CloseHandle when they are no longer needed.</para>
        ///     <para>
        ///         Important  The caller is responsible for ensuring that the standard handle fields in STARTUPINFO contain
        ///         valid handle values. These fields are copied unchanged to the child process without validation, even when the
        ///         dwFlags member specifies STARTF_USESTDHANDLES. Incorrect values can cause the child process to misbehave or
        ///         crash. Use the Application Verifier runtime verification tool to detect invalid handles.
        ///     </para>
        /// </param>
        /// <param name="lpProcessInformation">
        ///     A pointer to a PROCESS_INFORMATION structure that receives identification information about the new process.
        ///     <para>Handles in PROCESS_INFORMATION must be closed with CloseHandle when they are no longer needed.</para>
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
        ///     <para>
        ///         Note that the function returns before the process has finished initialization. If a required DLL cannot be
        ///         located or fails to initialize, the process is terminated. To get the termination status of a process, call
        ///         GetExitCodeProcess.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The process is assigned a process identifier. The identifier is valid until the process terminates. It can be
        ///         used to identify the process, or specified in the OpenProcess function to open a handle to the process. The
        ///         initial thread in the process is also assigned a thread identifier. It can be specified in the OpenThread
        ///         function to open a handle to the thread. The identifier is valid until the thread terminates and can be used to
        ///         uniquely identify the thread within the system. These identifiers are returned in the PROCESS_INFORMATION
        ///         structure.
        ///     </para>
        ///     <para>
        ///         The name of the executable in the command line that the operating system provides to a process is not
        ///         necessarily identical to that in the command line that the calling process gives to the CreateProcess function.
        ///         The operating system may prepend a fully qualified path to an executable name that is provided without a fully
        ///         qualified path.
        ///     </para>
        ///     <para>
        ///         The calling thread can use the WaitForInputIdle function to wait until the new process has finished its
        ///         initialization and is waiting for user input with no input pending. This can be useful for synchronization
        ///         between parent and child processes, because CreateProcess returns without waiting for the new process to finish
        ///         its initialization. For example, the creating process would use WaitForInputIdle before trying to find a window
        ///         associated with the new process.
        ///     </para>
        ///     <para>
        ///         The preferred way to shut down a process is by using the ExitProcess function, because this function sends
        ///         notification of approaching termination to all DLLs attached to the process. Other means of shutting down a
        ///         process do not notify the attached DLLs. Note that when a thread calls ExitProcess, other threads of the
        ///         process are terminated without an opportunity to execute any additional code (including the thread termination
        ///         code of attached DLLs). For more information, see Terminating a Process.
        ///     </para>
        ///     <para>
        ///         A parent process can directly alter the environment variables of a child process during process creation.
        ///         This is the only situation when a process can directly change the environment settings of another process. For
        ///         more information, see Changing Environment Variables.
        ///     </para>
        ///     <para>
        ///         If an application provides an environment block, the current directory information of the system drives is
        ///         not automatically propagated to the new process. For example, there is an environment variable named =C: whose
        ///         value is the current directory on drive C. An application must manually pass the current directory information
        ///         to the new process. To do so, the application must explicitly create these environment variable strings, sort
        ///         them alphabetically (because the system uses a sorted environment), and put them into the environment block.
        ///         Typically, they will go at the front of the environment block, due to the environment block sort order.
        ///     </para>
        ///     <para>
        ///         One way to obtain the current directory information for a drive X is to make the following call:
        ///         GetFullPathName("X:", ...). That avoids an application having to scan the environment block. If the full path
        ///         returned is X:\, there is no need to pass that value on as environment data, since the root directory is the
        ///         default current directory for drive X of a new process.
        ///     </para>
        ///     <para>
        ///         When a process is created with CREATE_NEW_PROCESS_GROUP specified, an implicit call to
        ///         SetConsoleCtrlHandler(NULL,TRUE) is made on behalf of the new process; this means that the new process has
        ///         CTRL+C disabled. This lets shells handle CTRL+C themselves, and selectively pass that signal on to
        ///         sub-processes. CTRL+BREAK is not disabled, and may be used to interrupt the process/process group.
        ///     </para>
        ///     <para>Security Remarks</para>
        ///     <para>
        ///         The first parameter, lpApplicationName, can be NULL, in which case the executable name must be in the white
        ///         space–delimited string pointed to by lpCommandLine. If the executable or path name has a space in it, there is
        ///         a risk that a different executable could be run because of the way the function parses spaces. The following
        ///         example is dangerous because the function will attempt to run "Program.exe", if it exists, instead of
        ///         "MyApp.exe".
        ///     </para>
        ///     <code>
        ///         LPTSTR szCmdline = _tcsdup(TEXT("C:\\Program Files\\MyApp -L -S"));
        /// 	    CreateProcess(NULL, szCmdline, /* ... */);
        ///     </code>
        ///     <para>
        ///         If a malicious user were to create an application called "Program.exe" on a system, any program that
        ///         incorrectly calls CreateProcess using the Program Files directory will run this application instead of the
        ///         intended application.
        ///     </para>
        ///     <para>
        ///         To avoid this problem, do not pass NULL for lpApplicationName. If you do pass NULL for lpApplicationName, use
        ///         quotation marks around the executable path in lpCommandLine, as shown in the example below.
        ///     </para>
        ///     <code>
        /// 	    LPTSTR szCmdline[] = _tcsdup(TEXT("\"C:\\Program Files\\MyApp\" -L -S"));
        /// 	    CreateProcess(NULL, szCmdline, /*...*/);
        ///     </code>
        /// </remarks>
        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            ProcessCreationFlags dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        /// <summary>
        ///     Destroys a Job Object handle.
        /// </summary>
        /// <param name="jobHandle">
        ///     Handle to the job.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the function succeeded, or <c>false</c> if it failed.
        ///     To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The job is destroyed when its last handle has been closed and all associated processes have exited.
        ///     </para>
        ///     <para>
        ///         However, if the job has the <see cref="LimitFlags.LimitKillOnJobClose"/> flag specified,
        ///         closing the last job object handle terminates <b>all</b> associated processes
        ///         and then destroys the job object itself.
        ///     </para>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(
            [In] IntPtr jobHandle);
    }

    #endregion

    #region Structs, enums, and flags

    #region JobObjectInfoClass Enumeration

    /// <summary>
    ///     Information class for the limits to be set. This parameter can be one of
    ///     the following values.
    /// </summary>
    internal enum JobObjectInfoClass
    {
        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_ACCOUNTING_INFORMATION structure.
        /// </summary>
        BasicAccountingInformation = 1,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        BasicLimitInformation = 2,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_PROCESS_ID_LIST structure.
        /// </summary>
        BasicProcessIdList = 3,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_UI_RESTRICTIONS structure.
        /// </summary>
        BasicUIRestrictions = 4,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_SECURITY_LIMIT_INFORMATION structure.
        ///     The hJob handle must have the JOB_OBJECT_SET_SECURITY_ATTRIBUTES
        ///     access right associated with it.
        /// </summary>
        SecurityLimitInformation = 5,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_END_OF_JOB_TIME_INFORMATION structure.
        /// </summary>
        EndOfJobTimeInformation = 6,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_ASSOCIATE_COMPLETION_PORT structure.
        /// </summary>
        AssociateCompletionPortInformation = 7,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_BASIC_AND_IO_ACCOUNTING_INFORMATION structure.
        /// </summary>
        BasicAndIoAccountingInformation = 8,

        /// <summary>
        ///     The lpJobObjectInfo parameter is a pointer to a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure.
        /// </summary>
        ExtendedLimitInformation = 9
    }

    #endregion

    #region LimitFlags Enumeration

    /// <summary>
    ///     Limit flags that are in effect. This member is a bit field that determines
    ///     whether other structure members are used. Any combination of the following
    ///     values can be specified.
    /// </summary>
    [Flags]
    internal enum LimitFlags
    {
        /// <summary>
        ///     Causes all processes associated with the job to use the same minimum and maximum working set sizes.
        /// </summary>
        LimitWorkingSet = 0x00000001,

        /// <summary>
        ///     Establishes a user-mode execution time limit for each currently active process
        ///     and for all future processes associated with the job.
        /// </summary>
        LimitProcessTime = 0x00000002,

        /// <summary>
        ///     Establishes a user-mode execution time limit for the job. This flag cannot
        ///     be used with JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME.
        /// </summary>
        LimitJobTime = 0x00000004,

        /// <summary>
        ///     Establishes a maximum number of simultaneously active processes associated
        ///     with the job.
        /// </summary>
        LimitActiveProcesses = 0x00000008,

        /// <summary>
        ///     Causes all processes associated with the job to use the same processor
        ///     affinity.
        /// </summary>
        LimitAffinity = 0x00000010,

        /// <summary>
        ///     Causes all processes associated with the job to use the same priority class.
        ///     For more information, see Scheduling Priorities.
        /// </summary>
        LimitPriorityClass = 0x00000020,

        /// <summary>
        ///     Preserves any job time limits you previously set. As long as this flag is
        ///     set, you can establish a per-job time limit once, then alter other limits
        ///     in subsequent calls. This flag cannot be used with JOB_OBJECT_LIMIT_JOB_TIME.
        /// </summary>
        PreserveJobTime = 0x00000040,

        /// <summary>
        ///     Causes all processes in the job to use the same scheduling class.
        /// </summary>
        LimitSchedulingClass = 0x00000080,

        /// <summary>
        ///     Causes all processes associated with the job to limit their committed memory.
        ///     When a process attempts to commit memory that would exceed the per-process
        ///     limit, it fails. If the job object is associated with a completion port, a
        ///     JOB_OBJECT_MSG_PROCESS_MEMORY_LIMIT message is sent to the completion port.
        ///     This limit requires use of a JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure.
        ///     Its BasicLimitInformation member is a JOBOBJECT_BASIC_LIMIT_INFORMATION
        ///     structure.
        /// </summary>
        LimitProcessMemory = 0x00000100,

        /// <summary>
        ///     Causes all processes associated with the job to limit the job-wide sum of
        ///     their committed memory. When a process attempts to commit memory that would
        ///     exceed the job-wide limit, it fails. If the job object is associated with a
        ///     completion port, a JOB_OBJECT_MSG_JOB_MEMORY_LIMIT message is sent to the
        ///     completion port.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        LimitJobMemory = 0x00000200,

        /// <summary>
        ///     Forces a call to the SetErrorMode function with the SEM_NOGPFAULTERRORBOX
        ///     flag for each process associated with the job.  If an exception occurs and
        ///     the system calls the UnhandledExceptionFilter function, the debugger will
        ///     be given a chance to act. If there is no debugger, the functions returns
        ///     EXCEPTION_EXECUTE_HANDLER. Normally, this will cause termination of the
        ///     process with the exception code as the exit status.  This limit requires
        ///     use of a JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its
        ///     BasicLimitInformation member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        DieOnUnhandledException = 0x00000400,

        /// <summary>
        ///     If any process associated with the job creates a child process using the
        ///     CREATE_BREAKAWAY_FROM_JOB flag while this limit is in effect, the child
        ///     process is not associated with the job.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        LimitBreakawayOk = 0x00000800,

        /// <summary>
        ///     Allows any process associated with the job to create child processes
        ///     that are not associated with the job.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        LimitSilentBreakawayOk = 0x00001000,

        /// <summary>
        ///     Causes all processes associated with the job to terminate when the last
        ///     handle to the job is closed.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        ///     Windows 2000:  This flag is not supported.
        /// </summary>
        LimitKillOnJobClose = 0x00002000
    }

    #endregion

    #region IoCounters Structures

    /// <summary>
    ///     Various counters for different types of IO operations.  Don't believe
    ///     this is currently implemented by Windows.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct IoCounters32
    {
        /// <summary>
        ///     The number of read operations.
        /// </summary>
        [FieldOffset(0)]
        public ulong ReadOperationCount;

        /// <summary>
        ///     The number of write operations.
        /// </summary>
        [FieldOffset(8)]
        public ulong WriteOperationCount;

        /// <summary>
        ///     The number of other operations.
        /// </summary>
        [FieldOffset(16)]
        public ulong OtherOperationCount;

        /// <summary>
        ///     The number of read transfers.
        /// </summary>
        [FieldOffset(24)]
        public ulong ReadTransferCount;

        /// <summary>
        ///     The number of write transfers.
        /// </summary>
        [FieldOffset(32)]
        public ulong WriteTransferCount;

        /// <summary>
        ///     The number of other transfers.
        /// </summary>
        [FieldOffset(40)]
        public ulong OtherTransferCount;
    }

    /// <summary>
    ///     Various counters for different types of IO operations.  Don't believe
    ///     this is currently implemented by Windows.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct IoCounters64
    {
        /// <summary>
        ///     The number of read operations.
        /// </summary>
        [FieldOffset(0)]
        public ulong ReadOperationCount;

        /// <summary>
        ///     The number of write operations.
        /// </summary>
        [FieldOffset(8)]
        public ulong WriteOperationCount;

        /// <summary>
        ///     The number of other operations.
        /// </summary>
        [FieldOffset(16)]
        public ulong OtherOperationCount;

        /// <summary>
        ///     The number of read transfers.
        /// </summary>
        [FieldOffset(24)]
        public ulong ReadTransferCount;

        /// <summary>
        ///     The number of write transfers.
        /// </summary>
        [FieldOffset(32)]
        public ulong WriteTransferCount;

        /// <summary>
        ///     The number of other transfers.
        /// </summary>
        [FieldOffset(40)]
        public ulong OtherTransferCount;
    }

    #endregion

    #region BasicLimits Structures

    /// <summary>
    ///     The JOBOBJECT_BASIC_LIMIT_INFORMATION structure contains basic limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct BasicLimits32
    {
        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PROCESS_TIME, this member is
        ///     the per-process user-mode execution time limit, in 100-nanosecond ticks.
        ///     Otherwise, this member is ignored.  The system periodically checks to
        ///     determine whether each process associated with the job has accumulated
        ///     more user-mode time than the set limit. If it has, the process is terminated.
        /// </summary>
        [FieldOffset(0)]
        public long PerProcessUserTimeLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_JOB_TIME, this member is the
        ///     per-job user-mode execution time limit, in 100-nanosecond ticks. Otherwise,
        ///     this member is ignored. The system adds the current time of the processes
        ///     associated with the job to this limit. For example, if you set this limit
        ///     to 1 minute, and the job has a process that has accumulated 5 minutes of
        ///     user-mode time, the limit actually enforced is 6 minutes.  The system
        ///     periodically checks to determine whether the sum of the user-mode execution
        ///     time for all processes is greater than this end-of-job limit. If it is, the
        ///     action specified in the EndOfJobTimeAction member of the
        ///     JOBOBJECT_END_OF_JOB_TIME_INFORMATION structure is carried out. By default,
        ///     all processes are terminated and the status code is set to
        ///     ERROR_NOT_ENOUGH_QUOTA.
        /// </summary>
        [FieldOffset(8)]
        public long PerJobUserTimeLimit;

        /// <summary>
        ///     Limit flags that are in effect. This member is a bit field that determines
        ///     whether other structure members are used. Any combination LimitFlag values
        ///     can be specified.
        /// </summary>
        [FieldOffset(16)]
        public LimitFlags LimitFlags;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     minimum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MaximumWorkingSetSize is nonzero,
        ///     MinimumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(20)]
        public uint MinimumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     maximum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MinimumWorkingSetSize is nonzero,
        ///     MaximumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(24)]
        public uint MaximumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_ACTIVE_PROCESS, this member is the
        ///     active process limit for the job. Otherwise, this member is ignored.  If you
        ///     try to associate a process with a job, and this causes the active process
        ///     count to exceed this limit, the process is terminated and the association
        ///     fails.
        /// </summary>
        [FieldOffset(28)]
        public int ActiveProcessLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_AFFINITY, this member is the
        ///     processor affinity for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The affinity must be a proper subset of the system
        ///     affinity mask obtained by calling the GetProcessAffinityMask function. The
        ///     affinity of each thread is set to this value, but threads are free to
        ///     subsequently set their affinity, as long as it is a subset of the specified
        ///     affinity mask. Processes cannot set their own affinity mask.
        /// </summary>
        [FieldOffset(32)]
        public IntPtr Affinity;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PRIORITY_CLASS, this member is the
        ///     priority class for all processes associated with the job. Otherwise, this
        ///     member is ignored. Processes and threads cannot modify their priority class.
        ///     The calling process must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(36)]
        public int PriorityClass;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_SCHEDULING_CLASS, this member is
        ///     the scheduling class for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The valid values are 0 to 9. Use 0 for the least
        ///     favorable scheduling class relative to other threads, and 9 for the most
        ///     favorable scheduling class relative to other threads. By default, this
        ///     value is 5. To use a scheduling class greater than 5, the calling process
        ///     must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(40)]
        public int SchedulingClass;
    }

    /// <summary>
    ///     The JOBOBJECT_BASIC_LIMIT_INFORMATION structure contains basic limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct BasicLimits64
    {
        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PROCESS_TIME, this member is
        ///     the per-process user-mode execution time limit, in 100-nanosecond ticks.
        ///     Otherwise, this member is ignored.  The system periodically checks to
        ///     determine whether each process associated with the job has accumulated
        ///     more user-mode time than the set limit. If it has, the process is terminated.
        /// </summary>
        [FieldOffset(0)]
        public long PerProcessUserTimeLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_JOB_TIME, this member is the
        ///     per-job user-mode execution time limit, in 100-nanosecond ticks. Otherwise,
        ///     this member is ignored. The system adds the current time of the processes
        ///     associated with the job to this limit. For example, if you set this limit
        ///     to 1 minute, and the job has a process that has accumulated 5 minutes of
        ///     user-mode time, the limit actually enforced is 6 minutes.  The system
        ///     periodically checks to determine whether the sum of the user-mode execution
        ///     time for all processes is greater than this end-of-job limit. If it is, the
        ///     action specified in the EndOfJobTimeAction member of the
        ///     JOBOBJECT_END_OF_JOB_TIME_INFORMATION structure is carried out. By default,
        ///     all processes are terminated and the status code is set to
        ///     ERROR_NOT_ENOUGH_QUOTA.
        /// </summary>
        [FieldOffset(8)]
        public long PerJobUserTimeLimit;

        /// <summary>
        ///     Limit flags that are in effect. This member is a bit field that determines
        ///     whether other structure members are used. Any combination LimitFlag values
        ///     can be specified.
        /// </summary>
        [FieldOffset(16)]
        public LimitFlags LimitFlags;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     minimum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MaximumWorkingSetSize is nonzero,
        ///     MinimumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(24)]
        public ulong MinimumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     maximum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MinimumWorkingSetSize is nonzero,
        ///     MaximumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(32)]
        public ulong MaximumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_ACTIVE_PROCESS, this member is the
        ///     active process limit for the job. Otherwise, this member is ignored.  If you
        ///     try to associate a process with a job, and this causes the active process
        ///     count to exceed this limit, the process is terminated and the association
        ///     fails.
        /// </summary>
        [FieldOffset(40)]
        public int ActiveProcessLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_AFFINITY, this member is the
        ///     processor affinity for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The affinity must be a proper subset of the system
        ///     affinity mask obtained by calling the GetProcessAffinityMask function. The
        ///     affinity of each thread is set to this value, but threads are free to
        ///     subsequently set their affinity, as long as it is a subset of the specified
        ///     affinity mask. Processes cannot set their own affinity mask.
        /// </summary>
        [FieldOffset(48)]
        public IntPtr Affinity;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PRIORITY_CLASS, this member is the
        ///     priority class for all processes associated with the job. Otherwise, this
        ///     member is ignored. Processes and threads cannot modify their priority class.
        ///     The calling process must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(56)]
        public int PriorityClass;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_SCHEDULING_CLASS, this member is
        ///     the scheduling class for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The valid values are 0 to 9. Use 0 for the least
        ///     favorable scheduling class relative to other threads, and 9 for the most
        ///     favorable scheduling class relative to other threads. By default, this
        ///     value is 5. To use a scheduling class greater than 5, the calling process
        ///     must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(60)]
        public int SchedulingClass;
    }

    #endregion

    #region ExtendedLimits Structures

    /// <summary>
    ///     The JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure contains basic and extended limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ExtendedLimits32
    {
        /// <summary>
        ///     A JOBOBJECT_BASIC_LIMIT_INFORMATION structure that contains
        ///     basic limit information.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits32 BasicLimits;

        /// <summary>
        ///     Resereved.
        /// </summary>
        [FieldOffset(48)]
        public IoCounters32 IoInfo;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_PROCESS_MEMORY value, this member specifies
        ///     the limit for the virtual memory that can be committed by a process.
        ///     Otherwise, this member is ignored.
        /// </summary>
        [FieldOffset(96)]
        public uint ProcessMemoryLimit;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_JOB_MEMORY value, this member specifies the
        ///     limit for the virtual memory that can be committed for the job. Otherwise,
        ///     this member is ignored.
        /// </summary>
        [FieldOffset(100)]
        public uint JobMemoryLimit;

        /// <summary>
        ///     Peak memory used by any process ever associated with the job.
        /// </summary>
        [FieldOffset(104)]
        public uint PeakProcessMemoryUsed;

        /// <summary>
        ///     Peak memory usage of all processes currently associated with the job.
        /// </summary>
        [FieldOffset(108)]
        public uint PeakJobMemoryUsed;
    }

    /// <summary>
    ///     The JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure contains basic and extended limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct ExtendedLimits64
    {
        /// <summary>
        ///     A JOBOBJECT_BASIC_LIMIT_INFORMATION structure that contains
        ///     basic limit information.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits64 BasicLimits;

        /// <summary>
        ///     Resereved.
        /// </summary>
        [FieldOffset(64)]
        public IoCounters64 IoInfo;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_PROCESS_MEMORY value, this member specifies
        ///     the limit for the virtual memory that can be committed by a process.
        ///     Otherwise, this member is ignored.
        /// </summary>
        [FieldOffset(112)]
        public ulong ProcessMemoryLimit;

        /// <summary>
        ///     If the LimitFlags member of the JOBOBJECT_BASIC_LIMIT_INFORMATION structure
        ///     specifies the JOB_OBJECT_LIMIT_JOB_MEMORY value, this member specifies the
        ///     limit for the virtual memory that can be committed for the job. Otherwise,
        ///     this member is ignored.
        /// </summary>
        [FieldOffset(120)]
        public ulong JobMemoryLimit;

        /// <summary>
        ///     Peak memory used by any process ever associated with the job.
        /// </summary>
        [FieldOffset(128)]
        public ulong PeakProcessMemoryUsed;

        /// <summary>
        ///     Peak memory usage of all processes currently associated with the job.
        /// </summary>
        [FieldOffset(136)]
        public ulong PeakJobMemoryUsed;
    }

    #endregion

    #region JobObjectInfo Union

    /// <summary>
    ///     Union of different limit data structures that may be passed
    ///     to SetInformationJobObject / from QueryInformationJobObject.
    ///     This union also contains separate 32 and 64 bit versions of
    ///     each structure.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct JobObjectInfo
    {
        #region 32 bit structures

        /// <summary>
        ///     The BasicLimits32 structure contains basic limit information
        ///     for a job object on a 32bit platform.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits32 basicLimits32;

        /// <summary>
        ///     The ExtendedLimits32 structure contains extended limit information
        ///     for a job object on a 32bit platform.
        /// </summary>
        [FieldOffset(0)]
        public ExtendedLimits32 extendedLimits32;

        #endregion

        #region 64 bit structures

        /// <summary>
        ///     The BasicLimits64 structure contains basic limit information
        ///     for a job object on a 64bit platform.
        /// </summary>
        [FieldOffset(0)]
        public BasicLimits64 basicLimits64;

        /// <summary>
        ///     The ExtendedLimits64 structure contains extended limit information
        ///     for a job object on a 64bit platform.
        /// </summary>
        [FieldOffset(0)]
        public ExtendedLimits64 extendedLimits64;

        #endregion
    }

    #endregion

    #region SECURITY_ATTRIBUTES Structure

    /// <summary>
    ///     The SECURITY_ATTRIBUTES structure contains the security descriptor for an object and specifies
    ///     whether the handle retrieved by specifying this structure is inheritable.
    ///     This structure provides security settings for objects created by various functions,
    ///     such as CreateFile, CreatePipe, CreateProcess, RegCreateKeyEx, or RegSaveKeyEx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_ATTRIBUTES
    {
        /// <summary>
        ///     The size, in bytes, of this structure. Set this value to the size of the SECURITY_ATTRIBUTES structure.
        /// </summary>
        public int nLength;

        /// <summary>
        ///     <para>
        ///         A pointer to a SECURITY_DESCRIPTOR structure that controls access to the object. If the value of this member
        ///         is NULL, the object is assigned the default security descriptor associated with the access token of the calling
        ///         process. This is not the same as granting access to everyone by assigning a NULL discretionary access control
        ///         list (DACL). By default, the default DACL in the access token of a process allows access only to the user
        ///         represented by the access token.
        ///     </para>
        ///     <para>For information about creating a security descriptor, see Creating a Security Descriptor.</para>
        /// </summary>
        public IntPtr lpSecurityDescriptor;

        /// <summary>
        ///     A Boolean value that specifies whether the returned handle is inherited when a new process is created. If this
        ///     member is TRUE, the new process inherits the handle.
        /// </summary>
        public bool bInheritHandle;
    }

    #endregion

    #region STARTF Enumeration (start flags)

    /// <summary>
    ///     A bitfield that determines whether certain STARTUPINFO members are used when the process creates a window.
    /// </summary>
    [Flags]
    internal enum STARTF
    {
        /// <summary>
        ///     Indicates that the cursor is in feedback mode for two seconds after CreateProcess is called. The Working in
        ///     Background cursor is displayed (see the Pointers tab in the Mouse control panel utility).
        ///     If during those two seconds the process makes the first GUI call, the system gives five more seconds to the
        ///     process. If during those five seconds the process shows a window, the system gives five more seconds to the process
        ///     to finish drawing the window.
        ///     The system turns the feedback cursor off after the first call to GetMessage, regardless of whether the process is
        ///     drawing.
        /// </summary>
        STARTF_FORCEONFEEDBACK = 0x00000040,

        /// <summary>
        ///     Indicates that the feedback cursor is forced off while the process is starting. The Normal Select cursor is
        ///     displayed.
        /// </summary>
        STARTF_FORCEOFFFEEDBACK = 0x00000080,

        /// <summary>
        ///     Indicates that any windows created by the process cannot be pinned on the taskbar.
        ///     This flag must be combined with STARTF_TITLEISAPPID.
        /// </summary>
        STARTF_PREVENTPINNING = 0x00002000,

        /// <summary>
        ///     Indicates that the process should be run in full-screen mode, rather than in windowed mode.
        ///     This flag is only valid for console applications running on an x86 computer.
        /// </summary>
        STARTF_RUNFULLSCREEN = 0x00000020,

        /// <summary>
        ///     The lpTitle member contains an AppUserModelID. This identifier controls how the taskbar and Start menu present the
        ///     application, and enables it to be associated with the correct shortcuts and Jump Lists. Generally, applications
        ///     will use the SetCurrentProcessExplicitAppUserModelID and GetCurrentProcessExplicitAppUserModelID functions instead
        ///     of setting this flag. For more information, see Application User Model IDs.
        ///     If STARTF_PREVENTPINNING is used, application windows cannot be pinned on the taskbar. The use of any
        ///     AppUserModelID-related window properties by the application overrides this setting for that window only.
        ///     This flag cannot be used with STARTF_TITLEISLINKNAME.
        /// </summary>
        STARTF_TITLEISAPPID = 0x00001000,

        /// <summary>
        ///     The lpTitle member contains the path of the shortcut file (.lnk) that the user invoked to start this process. This
        ///     is typically set by the shell when a .lnk file pointing to the launched application is invoked. Most applications
        ///     will not need to set this value.
        ///     This flag cannot be used with STARTF_TITLEISAPPID.
        /// </summary>
        STARTF_TITLEISLINKNAME = 0x00000800,

        /// <summary>
        ///     The dwXCountChars and dwYCountChars members contain additional information.
        /// </summary>
        STARTF_USECOUNTCHARS = 0x00000008,

        /// <summary>
        ///     The dwFillAttribute member contains additional information.
        /// </summary>
        STARTF_USEFILLATTRIBUTE = 0x00000010,

        /// <summary>
        ///     The hStdInput member contains additional information.
        ///     This flag cannot be used with STARTF_USESTDHANDLES.
        /// </summary>
        STARTF_USEHOTKEY = 0x00000200,

        /// <summary>
        ///     The dwX and dwY members contain additional information.
        /// </summary>
        STARTF_USEPOSITION = 0x00000004,

        /// <summary>
        ///     The wShowWindow member contains additional information.
        /// </summary>
        STARTF_USESHOWWINDOW = 0x00000001,

        /// <summary>
        ///     The dwXSize and dwYSize members contain additional information.
        /// </summary>
        STARTF_USESIZE = 0x00000002,

        /// <summary>
        ///     The hStdInput, hStdOutput, and hStdError members contain additional information.
        ///     If this flag is specified when calling one of the process creation functions, the handles must be inheritable and
        ///     the function's bInheritHandles parameter must be set to TRUE. For more information, see Handle Inheritance.
        ///     If this flag is specified when calling the GetStartupInfo function, these members are either the handle value
        ///     specified during process creation or INVALID_HANDLE_VALUE.
        ///     Handles must be closed with CloseHandle when they are no longer needed.
        ///     This flag cannot be used with STARTF_USEHOTKEY.
        /// </summary>
        STARTF_USESTDHANDLES = 0x00000100
    }

    #endregion

    #region STARTUPINFO Structure

    /// <summary>
    ///     Specifies the window station, desktop, standard handles, and appearance of the main window for a process at
    ///     creation time.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct STARTUPINFO
    {
        /// <summary>
        ///     The size of the structure, in bytes.
        /// </summary>
        public Int32 cb;

        /// <summary>
        ///     Reserved; must be NULL.
        /// </summary>
        public string lpReserved;

        /// <summary>
        ///     The name of the desktop, or the name of both the desktop and window station for this process. A backslash in the
        ///     string indicates that the string includes both the desktop and window station names. For more information, see
        ///     Thread Connection to a Desktop.
        /// </summary>
        public string lpDesktop;

        /// <summary>
        ///     For console processes, this is the title displayed in the title bar if a new console window is created. If NULL,
        ///     the name of the executable file is used as the window title instead. This parameter must be NULL for GUI or console
        ///     processes that do not create a new console window.
        /// </summary>
        public string lpTitle;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEPOSITION, this member is the x offset of the upper left corner of a window if
        ///         a new window is created, in pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         The offset is from the upper left corner of the screen. For GUI processes, the specified position is used the
        ///         first time the new process calls CreateWindow to create an overlapped window if the x parameter of CreateWindow
        ///         is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwX;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEPOSITION, this member is the y offset of the upper left corner of a window if
        ///         a new window is created, in pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         The offset is from the upper left corner of the screen. For GUI processes, the specified position is used the
        ///         first time the new process calls CreateWindow to create an overlapped window if the y parameter of CreateWindow
        ///         is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwY;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESIZE, this member is the width of the window if a new window is created, in
        ///         pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         For GUI processes, this is used only the first time the new process calls CreateWindow to create an
        ///         overlapped window if the nWidth parameter of CreateWindow is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwXSize;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESIZE, this member is the height of the window if a new window is created, in
        ///         pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         For GUI processes, this is used only the first time the new process calls CreateWindow to create an
        ///         overlapped window if the nHeight parameter of CreateWindow is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwYSize;

        /// <summary>
        ///     If dwFlags specifies STARTF_USECOUNTCHARS, if a new console window is created in a console process, this member
        ///     specifies the screen buffer width, in character columns. Otherwise, this member is ignored.
        /// </summary>
        public Int32 dwXCountChars;

        /// <summary>
        ///     If dwFlags specifies STARTF_USECOUNTCHARS, if a new console window is created in a console process, this member
        ///     specifies the screen buffer height, in character rows. Otherwise, this member is ignored.
        /// </summary>
        public Int32 dwYCountChars;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEFILLATTRIBUTE, this member is the initial text and background colors if a new
        ///         console window is created in a console application. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         This value can be any combination of the following values: FOREGROUND_BLUE, FOREGROUND_GREEN, FOREGROUND_RED,
        ///         FOREGROUND_INTENSITY, BACKGROUND_BLUE, BACKGROUND_GREEN, BACKGROUND_RED, and BACKGROUND_INTENSITY. For example,
        ///         the following combination of values produces red text on a white background:
        ///     </para>
        ///     <code>FOREGROUND_RED| BACKGROUND_RED| BACKGROUND_GREEN| BACKGROUND_BLUE</code>
        /// </summary>
        public Int32 dwFillAttribute;

        /// <summary>
        ///     A bitfield that determines whether certain STARTUPINFO members are used when the process creates a window.
        /// </summary>
        public STARTF dwFlags;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESHOWWINDOW, this member can be any of the values that can be specified in the
        ///         nCmdShow parameter for the ShowWindow function, except for SW_SHOWDEFAULT. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         For GUI processes, the first time ShowWindow is called, its nCmdShow parameter is ignored wShowWindow
        ///         specifies the default value. In subsequent calls to ShowWindow, the wShowWindow member is used if the nCmdShow
        ///         parameter of ShowWindow is set to SW_SHOWDEFAULT.
        ///     </para>
        /// </summary>
        public Int16 wShowWindow;

        /// <summary>
        ///     Reserved for use by the C Run-time; must be zero.
        /// </summary>
        public Int16 cbReserved2;

        /// <summary>
        ///     Reserved for use by the C Run-time; must be NULL.
        /// </summary>
        public IntPtr lpReserved2;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESTDHANDLES, this member is the standard input handle for the process. If
        ///         STARTF_USESTDHANDLES is not specified, the default for standard input is the keyboard buffer.
        ///     </para>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEHOTKEY, this member specifies a hotkey value that is sent as the wParam
        ///         parameter of a WM_SETHOTKEY message to the first eligible top-level window created by the application that owns
        ///         the process. If the window is created with the WS_POPUP window style, it is not eligible unless the
        ///         WS_EX_APPWINDOW extended window style is also set. For more information, see CreateWindowEx.
        ///     </para>
        ///     <para>Otherwise, this member is ignored.</para>
        /// </summary>
        public IntPtr hStdInput;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESTDHANDLES, this member is the standard output handle for the process.
        ///         Otherwise, this member is ignored and the default for standard output is the console window's buffer.
        ///     </para>
        ///     <para>
        ///         If a process is launched from the taskbar or jump list, the system sets hStdOutput to a handle to the monitor
        ///         that contains the taskbar or jump list used to launch the process. For more information, see Remarks.
        ///     </para>
        ///     <para>
        ///         Windows 7, Windows Server 2008 R2, Windows Vista, Windows Server 2008, Windows XP, and Windows Server 2003:
        ///         This behavior was introduced in Windows 8 and Windows Server 2012.
        ///     </para>
        /// </summary>
        public IntPtr hStdOutput;

        /// <summary>
        ///     If dwFlags specifies STARTF_USESTDHANDLES, this member is the standard error handle for the process. Otherwise,
        ///     this member is ignored and the default for standard error is the console window's buffer.
        /// </summary>
        public IntPtr hStdError;
    }

    #endregion

    #region PROCESS_INFORMATION Structure

    /// <summary>
    ///     <para>
    ///         Contains information about a newly created process and its primary thread. It is used with the CreateProcess,
    ///         CreateProcessAsUser, CreateProcessWithLogonW, or CreateProcessWithTokenW function.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     If the function succeeds, be sure to call the CloseHandle function to close the hProcess and hThread handles when
    ///     you are finished with them. Otherwise, when the child process exits, the system cannot clean up the process
    ///     structures for the child process because the parent process still has open handles to the child process. However,
    ///     the system will close these handles when the parent process terminates, so the structures related to the child
    ///     process object would be cleaned up at this point.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_INFORMATION
    {
        /// <summary>
        ///     A handle to the newly created process. The handle is used to specify the process in all functions that perform
        ///     operations on the process object.
        /// </summary>
        public IntPtr hProcess;

        /// <summary>
        ///     A handle to the primary thread of the newly created process. The handle is used to specify the thread in all
        ///     functions that perform operations on the thread object.
        /// </summary>
        public IntPtr hThread;

        /// <summary>
        ///     A value that can be used to identify a process. The value is valid from the time the process is created until all
        ///     handles to the process are closed and the process object is freed; at this point, the identifier may be reused.
        /// </summary>
        public int dwProcessId;

        /// <summary>
        ///     A value that can be used to identify a thread. The value is valid from the time the thread is created until all
        ///     handles to the thread are closed and the thread object is freed; at this point, the identifier may be reused.
        /// </summary>
        public int dwThreadId;
    }

    #endregion

    #region Process Creation Flags

    [Flags]
    internal enum ProcessCreationFlags : uint
    {
        /// <summary>
        ///     <para>The child processes of a process associated with a job are not associated with the job.</para>
        ///     <para>
        ///         If the calling process is not associated with a job, this constant has no effect. If the calling process is
        ///         associated with a job, the job must set the JOB_OBJECT_LIMIT_BREAKAWAY_OK limit.
        ///     </para>
        /// </summary>
        CREATE_BREAKAWAY_FROM_JOB = 0x01000000,

        /// <summary>
        ///     <para>
        ///         The new process does not inherit the error mode of the calling process. Instead, the new process gets the
        ///         default error mode.
        ///     </para>
        ///     <para>This feature is particularly useful for multithreaded shell applications that run with hard errors disabled.</para>
        ///     <para>
        ///         The default behavior is for the new process to inherit the error mode of the caller. Setting this flag
        ///         changes that default behavior.
        ///     </para>
        /// </summary>
        CREATE_DEFAULT_ERROR_MODE = 0x04000000,

        /// <summary>
        ///     <para>
        ///         The new process has a new console, instead of inheriting its parent's console (the default). For more
        ///         information, see Creation of a Console.
        ///     </para>
        ///     <para>This flag cannot be used with DETACHED_PROCESS.</para>
        /// </summary>
        CREATE_NEW_CONSOLE = 0x00000010,

        /// <summary>
        ///     <para>
        ///         The new process is the root process of a new process group. The process group includes all processes that are
        ///         descendants of this root process. The process identifier of the new process group is the same as the process
        ///         identifier, which is returned in the lpProcessInformation parameter. Process groups are used by the
        ///         GenerateConsoleCtrlEvent function to enable sending a CTRL+BREAK signal to a group of console processes.
        ///     </para>
        ///     <para>If this flag is specified, CTRL+C signals will be disabled for all processes within the new process group.</para>
        ///     <para>This flag is ignored if specified with CREATE_NEW_CONSOLE.</para>
        /// </summary>
        CREATE_NEW_PROCESS_GROUP = 0x00000200,

        /// <summary>
        ///     <para>
        ///         The process is a console application that is being run without a console window. Therefore, the console
        ///         handle for the application is not set.
        ///     </para>
        ///     <para>
        ///         This flag is ignored if the application is not a console application, or if it is used with either
        ///         CREATE_NEW_CONSOLE or DETACHED_PROCESS.
        ///     </para>
        /// </summary>
        CREATE_NO_WINDOW = 0x08000000,

        /// <summary>
        ///     <para>
        ///         The process is to be run as a protected process. The system restricts access to protected processes and the
        ///         threads of protected processes. For more information on how processes can interact with protected processes,
        ///         see Process Security and Access Rights.
        ///     </para>
        ///     <para>
        ///         To activate a protected process, the binary must have a special signature. This signature is provided by
        ///         Microsoft but not currently available for non-Microsoft binaries. There are currently four protected processes:
        ///         media foundation, audio engine, Windows error reporting, and system. Components that load into these binaries
        ///         must also be signed. Multimedia companies can leverage the first two protected processes. For more information,
        ///         see Overview of the Protected Media Path.
        ///     </para>
        ///     <para>Windows Server 2003 and Windows XP:  This value is not supported.</para>
        /// </summary>
        CREATE_PROTECTED_PROCESS = 0x00040000,

        /// <summary>
        ///     Allows the caller to execute a child process that bypasses the process restrictions that would normally be applied
        ///     automatically to the process.
        /// </summary>
        CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,

        /// <summary>
        ///     This flag is valid only when starting a 16-bit Windows-based application. If set, the new process runs in a private
        ///     Virtual DOS Machine (VDM). By default, all 16-bit Windows-based applications run as threads in a single, shared
        ///     VDM. The advantage of running separately is that a crash only terminates the single VDM; any other programs running
        ///     in distinct VDMs continue to function normally. Also, 16-bit Windows-based applications that are run in separate
        ///     VDMs have separate input queues. That means that if one application stops responding momentarily, applications in
        ///     separate VDMs continue to receive input. The disadvantage of running separately is that it takes significantly more
        ///     memory to do so. You should use this flag only if the user requests that 16-bit applications should run in their
        ///     own VDM.
        /// </summary>
        CREATE_SEPARATE_WOW_VDM = 0x00000800,

        /// <summary>
        ///     The flag is valid only when starting a 16-bit Windows-based application. If the DefaultSeparateVDM switch in the
        ///     Windows section of WIN.INI is TRUE, this flag overrides the switch. The new process is run in the shared Virtual
        ///     DOS Machine.
        /// </summary>
        CREATE_SHARED_WOW_VDM = 0x00001000,

        /// <summary>
        ///     The primary thread of the new process is created in a suspended state, and does not run until the ResumeThread
        ///     function is called.
        /// </summary>
        CREATE_SUSPENDED = 0x00000004,

        /// <summary>
        ///     If this flag is set, the environment block pointed to by lpEnvironment uses Unicode characters. Otherwise, the
        ///     environment block uses ANSI characters.
        /// </summary>
        CREATE_UNICODE_ENVIRONMENT = 0x00000400,

        /// <summary>
        ///     The calling thread starts and debugs the new process. It can receive all related debug events using the
        ///     WaitForDebugEvent function.
        /// </summary>
        DEBUG_ONLY_THIS_PROCESS = 0x00000002,

        /// <summary>
        ///     <para>
        ///         The calling thread starts and debugs the new process and all child processes created by the new process. It
        ///         can receive all related debug events using the WaitForDebugEvent function.
        ///     </para>
        ///     <para>
        ///         A process that uses DEBUG_PROCESS becomes the root of a debugging chain. This continues until another process
        ///         in the chain is created with DEBUG_PROCESS.
        ///     </para>
        ///     <para>
        ///         If this flag is combined with DEBUG_ONLY_THIS_PROCESS, the caller debugs only the new process, not any child
        ///         processes.
        ///     </para>
        /// </summary>
        DEBUG_PROCESS = 0x00000001,

        /// <summary>
        ///     <para>
        ///         For console processes, the new process does not inherit its parent's console (the default). The new process
        ///         can call the AllocConsole function at a later time to create a console. For more information, see Creation of a
        ///         Console.
        ///     </para>
        ///     <para>This value cannot be used with CREATE_NEW_CONSOLE.</para>
        /// </summary>
        DETACHED_PROCESS = 0x00000008,

        /// <summary>
        ///     <para>
        ///         The process is created with extended startup information; the lpStartupInfo parameter specifies a
        ///         STARTUPINFOEX structure.
        ///     </para>
        ///     <para>Windows Server 2003 and Windows XP:  This value is not supported.</para>
        /// </summary>
        EXTENDED_STARTUPINFO_PRESENT = 0x00080000,

        /// <summary>
        ///     <para>
        ///         The process inherits its parent's affinity. If the parent process has threads in more than one processor
        ///         group, the new process inherits the group-relative affinity of an arbitrary group in use by the parent.
        ///     </para>
        ///     <para>Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported.</para>
        /// </summary>
        INHERIT_PARENT_AFFINITY = 0x00010000,
    }

    #endregion

    #region IoCounters Structure

    [StructLayout(LayoutKind.Sequential)]
    internal struct IoCounters
    {
        public readonly UInt64 ReadOperationCount;
        public readonly UInt64 WriteOperationCount;
        public readonly UInt64 OtherOperationCount;
        public readonly UInt64 ReadTransferCount;
        public readonly UInt64 WriteTransferCount;
        public readonly UInt64 OtherTransferCount;
    }

    #endregion

    #region JobObjectBasicLimitInformation Structure

    [StructLayout(LayoutKind.Sequential)]
    internal struct JobObjectBasicLimitInformation
    {
        public readonly Int64 PerProcessUserTimeLimit;
        public readonly Int64 PerJobUserTimeLimit;
        public LimitFlags LimitFlags;
        public readonly UIntPtr MinimumWorkingSetSize;
        public readonly UIntPtr MaximumWorkingSetSize;
        public readonly Int16 ActiveProcessLimit;
        public readonly Int64 Affinity;
        public readonly Int16 PriorityClass;
        public readonly Int16 SchedulingClass;
    }

    #endregion

    #region JobObjectExtendedLimitInformation Structure

    [StructLayout(LayoutKind.Sequential)]
    internal struct JobObjectExtendedLimitInformation
    {
        public JobObjectBasicLimitInformation BasicLimitInformation;
        public readonly IoCounters IoInfo;
        public readonly UIntPtr ProcessMemoryLimit;
        public readonly UIntPtr JobMemoryLimit;
        public readonly UIntPtr PeakProcessMemoryUsed;
        public readonly UIntPtr PeakJobMemoryUsed;
    }

    #endregion

    #endregion
}
