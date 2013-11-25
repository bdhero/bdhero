using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNetUtils.Crypto
{
    /// <summary>
    ///     Abstract base class for crypographic hashing algorithms.
    ///     Provides convenience methods for computing hexadecimal hashes (0-9, a-f) from strings, files, streams,
    ///     and byte arrays.
    /// </summary>
    public abstract class CryptoHashAlgorithm
    {
        /// <summary>
        ///     Gets or sets whether hexadecimal hash values should be uppercase (<c>true</c>) or lowercase (<c>false</c>).
        ///     The default is <c>true</c>.
        /// </summary>
        public bool UpperCase = true;

        #region Abstract members

        /// <summary>
        ///     Provides the implementation for the hashing algorithm.
        /// </summary>
        /// <param name="buffer">Input data to be hashed.</param>
        /// <returns>The computed hash value of <paramref name="buffer"/>.</returns>
        protected abstract byte[] ComputeImpl(byte[] buffer);

        /// <summary>
        ///     Human-friendly name of the algorithm.  E.G., "SHA-1".
        /// </summary>
        public abstract string HumanName { get; }

        /// <summary>
        ///     Lowercase alphanumerics only (e.g., "sha1").
        /// </summary>
        public abstract string MachineName { get; }

        #endregion

        #region Concrete members

        /// <summary>
        ///     Computes the crypographic hash of the given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Input data to be hashed.</param>
        /// <returns>The computed hash value of <paramref name="text"/>.</returns>
        public string ComputeText(string text)
        {
            return ComputeBytes(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        ///     Computes the crypographic hash of <paramref name="path"/>'s contents.
        /// </summary>
        /// <param name="path">Path to a file.</param>
        /// <returns>The computed hash value of <paramref name="path"/>'s contents.</returns>
        public string ComputeFile(string path)
        {
            return ComputeBytes(File.ReadAllBytes(path));
        }

        /// <summary>
        ///     Computes the crypographic hash of <paramref name="stream"/>'s contents.
        /// </summary>
        /// <param name="stream">Input data stream to hash.</param>
        /// <returns>The computed hash value of <paramref name="stream"/>'s contents.</returns>
        public string ComputeStream(Stream stream)
        {
            return ComputeBytes(FileUtils.ReadStream(stream));
        }

        /// <summary>
        ///     Computes the crypographic hash of <paramref name="buffer"/>'s contents.
        /// </summary>
        /// <param name="buffer">Input data to hash.</param>
        /// <returns>The computed hash value of <paramref name="buffer"/>'s contents.</returns>
        public string ComputeBytes(byte[] buffer)
        {
            var format = UpperCase ? "X2" : "x2";
            var hash = ComputeImpl(buffer);
            var sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString(format));
            }
            return sb.ToString();
        }

        #endregion

        #region ToString

        /// <summary>
        ///     Returns a string that represents the current cryptographic hashing algorithm.
        /// </summary>
        /// <returns>A string that represents the current cryptographic hashing algorithm.</returns>
        public override string ToString()
        {
            return MachineName;
        }

        #endregion

        #region Equality

        /// <summary>
        ///     Determines if the current algorithm and the <paramref name="other"/> algorithm represent the same
        ///     cryptographic hashing algorithm.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        ///     <c>true</c> if this algorithm represent the same cryptographic hashing algorithm as
        ///     <paramref name="other"/>; otherwise <c>false</c>.
        /// </returns>
        protected bool Equals(CryptoHashAlgorithm other)
        {
            return string.Equals(MachineName, other.MachineName);
        }

        /// <summary>
        ///     Determines if the current algorithm is equal to <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="obj"/> has the same runtime class and represents the same cryptographic
        ///     hashing algorithm as this algorithm; otherwise <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CryptoHashAlgorithm) obj);
        }

        public override int GetHashCode()
        {
            return (MachineName != null ? MachineName.GetHashCode() : 0);
        }

        #endregion
    }

    /// <summary>
    ///     Concrete implementation of <seealso cref="CryptoHashAlgorithm"/> that generates MD5 sums.
    /// </summary>
    public class MD5Algorithm : CryptoHashAlgorithm
    {
        protected override byte[] ComputeImpl(byte[] buffer)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(buffer);
            }
        }

        public override string HumanName
        {
            get { return "MD5"; }
        }

        public override string MachineName
        {
            get { return "md5"; }
        }
    }

    /// <summary>
    ///     Concrete implementation of <seealso cref="CryptoHashAlgorithm"/> that generates SHA-1 sums.
    /// </summary>
    public class SHA1Algorithm : CryptoHashAlgorithm
    {
        protected override byte[] ComputeImpl(byte[] buffer)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(buffer);
            }
        }

        public override string HumanName
        {
            get { return "SHA-1"; }
        }

        public override string MachineName
        {
            get { return "sha1"; }
        }
    }

    /// <summary>
    ///     Concrete implementation of <seealso cref="CryptoHashAlgorithm"/> that generates SHA-256 sums.
    /// </summary>
    public class SHA256Algorithm : CryptoHashAlgorithm
    {
        protected override byte[] ComputeImpl(byte[] buffer)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(buffer);
            }
        }

        public override string HumanName
        {
            get { return "SHA-256"; }
        }

        public override string MachineName
        {
            get { return "sha256"; }
        }
    }

    /// <summary>
    ///     Concrete implementation of <seealso cref="CryptoHashAlgorithm"/> that generates SHA-512 sums.
    /// </summary>
    public class SHA512Algorithm : CryptoHashAlgorithm
    {
        protected override byte[] ComputeImpl(byte[] buffer)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(buffer);
            }
        }

        public override string HumanName
        {
            get { return "SHA-512"; }
        }

        public override string MachineName
        {
            get { return "sha512"; }
        }
    }
}
