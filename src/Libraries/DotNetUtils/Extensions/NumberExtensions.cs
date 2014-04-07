using System;
using System.Globalization;

namespace DotNetUtils.Extensions
{
    /// <summary>
    ///     Static utility class containing extension methods for culture-invariant number parsing.
    /// </summary>
    public static class NumberExtensions
    {
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        /// <summary>
        ///     Converts the string representation of a number in a culture-invariant format to its 32-bit signed integer equivalent.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer equivalent to the number specified in the string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The string is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     The string is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The string represents a number that is less than <see cref="int.MinValue"/> or greater than <see cref="int.MaxValue"/>.
        /// </exception>
        public static int ParseIntInvariant(this string str)
        {
            return int.Parse(str, InvariantCulture);
        }

        /// <summary>
        ///     Converts the string representation of a number in a culture-invariant format to its 64-bit signed integer equivalent.
        /// </summary>
        /// <returns>
        ///     A 64-bit signed integer equivalent to the number specified in the string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The string is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     The string is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The string represents a number that is less than <see cref="long.MinValue"/> or greater than <see cref="long.MaxValue"/>.
        /// </exception>
        public static long ParseLongInvariant(this string str)
        {
            return long.Parse(str, InvariantCulture);
        }

        /// <summary>
        ///     Converts the string representation of a number in a culture-invariant format to its 64-bit unsigned integer equivalent.
        /// </summary>
        /// <returns>
        ///     A 64-bit unsigned integer equivalent to the number specified in the string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The string is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     The string is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The string represents a number that is less than <see cref="UInt64.MinValue"/> or greater than <see cref="UInt64.MaxValue"/>.
        /// </exception>
        public static UInt64 ParseUInt64Invariant(this string str)
        {
            return UInt64.Parse(str, InvariantCulture);
        }

        /// <summary>
        ///     Converts the string representation of a number in a culture-invariant format to its single-precision floating-point number equivalent.
        /// </summary>
        /// <returns>
        ///     A single-precision floating-point number equivalent to the number specified in the string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The string is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     The string is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The string represents a number that is less than <see cref="float.MinValue"/> or greater than <see cref="float.MaxValue"/>.
        /// </exception>
        public static float ParseFloatInvariant(this string str)
        {
            return float.Parse(str, InvariantCulture);
        }

        /// <summary>
        ///     Converts the string representation of a number in a culture-invariant format to its double-precision floating-point number equivalent.
        /// </summary>
        /// <returns>
        ///     A double-precision floating-point number equivalent to the number specified in the string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The string is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        ///     The string is not in the correct format.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The string represents a number that is less than <see cref="double.MinValue"/> or greater than <see cref="double.MaxValue"/>.
        /// </exception>
        public static double ParseDoubleInvariant(this string str)
        {
            return double.Parse(str, InvariantCulture);
        }

        /// <summary>
        ///     Converts the string representation of a number in a culture-invariant format to its double-precision floating-point number equivalent.
        ///     A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="result">
        ///     When this method returns, contains a double-precision floating-point number equivalent to the numeric value
        ///     or symbol contained in the string, if the conversion succeeded, or zero if the conversion failed.
        ///     The conversion fails if the the string is <c>null</c>, is not in a compliant format, or represents
        ///     a number less than <see cref="double.MinValue"/> or greater than <see cref="double.MaxValue"/>.
        ///     This parameter is passed uninitialized. 
        /// </param>
        /// <returns>
        ///     <c>true</c> if the string was converted successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryParseDoubleInvariant(this string str, out double result)
        {
            return double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, InvariantCulture, out result);
        }
    }
}
