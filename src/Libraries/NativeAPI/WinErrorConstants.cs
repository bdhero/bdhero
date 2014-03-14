// ReSharper disable InconsistentNaming
namespace NativeAPI
{
    /// <summary>
    ///     Constants defined in <c>Winerror.h</c>.
    ///     Includes the most common <c>HRESULT</c> values.
    /// </summary>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/aa378137(v=vs.85).aspx"/>
    internal class WinErrorConstants
    {
        /// <summary>
        ///     Operation successful.
        /// </summary>
        public const uint S_OK = 0x00000000;

        /// <summary>
        ///     Not implemented.
        /// </summary>
        public const uint E_NOTIMPL = 0x80004001;

        /// <summary>
        ///     No such interface supported.
        /// </summary>
        public const uint E_NOINTERFACE = 0x80004002;

        /// <summary>
        ///     Pointer that is not valid.
        /// </summary>
        public const uint E_POINTER = 0x80004003;

        /// <summary>
        ///     Operation aborted.
        /// </summary>
        public const uint E_ABORT = 0x80004004;

        /// <summary>
        ///     Unspecified failure.
        /// </summary>
        public const uint E_FAIL = 0x80004005;

        /// <summary>
        ///     Unexpected failure.
        /// </summary>
        public const uint E_UNEXPECTED = 0x8000FFFF;

        /// <summary>
        ///     General access denied error.
        /// </summary>
        public const uint E_ACCESSDENIED = 0x80070005;

        /// <summary>
        ///     Handle that is not valid.
        /// </summary>
        public const uint E_HANDLE = 0x80070006;

        /// <summary>
        ///     Failed to allocate necessary memory.
        /// </summary>
        public const uint E_OUTOFMEMORY = 0x8007000E;

        /// <summary>
        ///     One or more arguments are not valid.
        /// </summary>
        public const uint E_INVALIDARG = 0x80070057;

    }
}
