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

namespace WinAPI.Kernel
{
    /// <summary>
    ///     Public class that holds all the Windows API calls necessary to manage Windows Job Objects.
    /// </summary>
    /// <seealso cref="https://www-auth.cs.wisc.edu/lists/htcondor-users/2009-June/msg00106.shtml" />
    public static class JobObjectAPI
    {

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
}
