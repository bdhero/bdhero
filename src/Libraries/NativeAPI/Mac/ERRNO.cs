// ReSharper disable InconsistentNaming
namespace NativeAPI.Mac
{
    /// <summary>
    ///     Standard system error codes defined in <c>/usr/include/sys/errno.h</c>.
    /// </summary>
    /// <seealso cref="http://www.freebsd.org/cgi/man.cgi?sysctl(3)"/>
    public enum ERRNO
    {
        /// <summary>
        ///     No search permission for one of the encountered "directories", or no read permission where oldval was nonzero, or no write permission where newval was nonzero.
        ///     -or-
        ///     An attempt is made to set a read-only value.
        ///     -or-
        ///     A process without appropriate privilege attempts to set a value.
        ///     -or-
        ///     Operation not permitted
        /// </summary>
        EPERM = 1,

        /// <summary>
        ///     The name array specifies a value that is unknown.
        ///     -or-
        ///     No such file or directory
        /// </summary>
        ENOENT = 2,

        /// <summary>
        ///     No such process
        /// </summary>
        ESRCH = 3,

        /// <summary>
        ///     Interrupted system call
        /// </summary>
        EINTR = 4,

        /// <summary>
        ///     Input/output error
        /// </summary>
        EIO = 5,

        /// <summary>
        ///     Device not configured
        /// </summary>
        ENXIO = 6,

        /// <summary>
        ///     Argument list too long
        /// </summary>
        E2BIG = 7,

        /// <summary>
        ///     Exec format error
        /// </summary>
        ENOEXEC = 8,

        /// <summary>
        ///     Bad file descriptor
        /// </summary>
        EBADF = 9,

        /// <summary>
        ///     No child processes
        /// </summary>
        ECHILD = 10,

        /// <summary>
        ///     Resource deadlock avoided */ /* 11 was EAGAIN
        /// </summary>
        EDEADLK = 11,

        /// <summary>
        ///     The length pointed to by oldlenp is too short to hold the requested value.
        ///     -or-
        ///     The smaller of either the length pointed to by oldlenp or the estimated size of the returned data exceeds the system limit on locked memory.
        ///     -or-
        ///     Locking the buffer oldp, or a portion of the buffer if the estimated size of the data to be returned is smaller, would cause the process to exceed its per- process locked memory limit.
        ///     -or-
        ///     Cannot allocate memory
        /// </summary>
        ENOMEM = 12,

        /// <summary>
        ///     No search permission for one of the encountered "directories", or no read permission where oldval was nonzero, or no write permission where newval was nonzero.
        ///     -or-
        ///     Permission denied
        /// </summary>
        EACCES = 13,

        /// <summary>
        ///     The invocation asked for the previous value by setting oldval non-NULL, but allowed zero room in oldlenp.
        ///     -or-
        ///     The buffer name, oldp, newp, or length pointer oldlenp contains an invalid address.
        ///     -or-
        ///     Bad address
        /// </summary>
        EFAULT = 14,

        /// <summary>
        ///     Block device required
        /// </summary>
        ENOTBLK = 15,

        /// <summary>
        ///     Device / Resource busy
        /// </summary>
        EBUSY = 16,

        /// <summary>
        ///     File exists
        /// </summary>
        EEXIST = 17,

        /// <summary>
        ///     Cross-device link
        /// </summary>
        EXDEV = 18,

        /// <summary>
        ///     Operation not supported by device
        /// </summary>
        ENODEV = 19,

        /// <summary>
        ///     name was not found.
        ///     -or-
        ///     The name array specifies an intermediate rather than terminal name.
        ///     -or-
        ///     Not a directory
        /// </summary>
        ENOTDIR = 20,

        /// <summary>
        ///     The name array specifies a terminal name, but the actual name is not terminal.
        ///     -or-
        ///     Is a directory
        /// </summary>
        EISDIR = 21,

        /// <summary>
        ///     The name array is less than two or greater than CTL_MAXNAME.
        ///     -or-
        ///     A non-null newp is given and its specified length in newlen is too large or too small.
        ///     -or-
        ///     Invalid argument
        /// </summary>
        EINVAL = 22,

        /// <summary>
        ///     Too many open files in system
        /// </summary>
        ENFILE = 23,

        /// <summary>
        ///     Too many open files
        /// </summary>
        EMFILE = 24,

        /// <summary>
        ///     Inappropriate ioctl for device
        /// </summary>
        ENOTTY = 25,

        /// <summary>
        ///     Text file busy
        /// </summary>
        ETXTBSY = 26,

        /// <summary>
        ///     File too large
        /// </summary>
        EFBIG = 27,

        /// <summary>
        ///     No space left on device
        /// </summary>
        ENOSPC = 28,

        /// <summary>
        ///     Illegal seek
        /// </summary>
        ESPIPE = 29,

        /// <summary>
        ///     Read-only file system
        /// </summary>
        EROFS = 30,

        /// <summary>
        ///     Too many links
        /// </summary>
        EMLINK = 31,

        /// <summary>
        ///     Broken pipe
        /// </summary>
        EPIPE = 32,
    }
}