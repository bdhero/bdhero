using System;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TimeSpan"/> objects.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns a culture-invariant representation of the TimeSpan in <c>hh:mm:ss</c> format.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns><c>hh:mm:ss</c></returns>
        public static string ToStringShort(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// Returns a culture-invariant representation of the TimeSpan in <c>hh:mm:ss.fff</c> format.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns><c>hh:mm:ss.fff</c></returns>
        public static string ToStringMedium(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss\.fff");
        }

        /// <summary>
        /// Returns a culture-invariant representation of the TimeSpan in <c>hh:mm:ss.fffffff</c> format.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns><c>hh:mm:ss.fffffff</c></returns>
        public static string ToStringLong(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss\.fffffff");
        }
    }
}