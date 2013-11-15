using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DotNetUtils.Crypto
{
    public abstract class CryptoHashAlgorithm
    {
        public static bool LowerCase = false;

        #region Abstract members

        protected abstract byte[] ComputeImpl(byte[] buffer);

        /// <summary>
        /// Human-friendly name of the algorithm.  E.G., "SHA-1".
        /// </summary>
        public abstract string HumanName { get; }

        /// <summary>
        /// Lowercase alphanumerics only (e.g., "sha1").
        /// </summary>
        public abstract string MachineName { get; }

        #endregion

        #region Concrete members

        public string ComputeText(string text)
        {
            return ComputeBytes(Encoding.UTF8.GetBytes(text));
        }

        public string ComputeFile(string path)
        {
            return ComputeBytes(File.ReadAllBytes(path));
        }

        public string ComputeStream(Stream stream)
        {
            return ComputeBytes(FileUtils.ReadStream(stream));
        }

        public string ComputeBytes(byte[] buffer)
        {
            var hash = ComputeImpl(buffer);
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            var str = sb.ToString();
            return LowerCase ? str.ToLowerInvariant() : str;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return MachineName;
        }

        #endregion

        #region Equality

        protected bool Equals(CryptoHashAlgorithm other)
        {
            return string.Equals(MachineName, other.MachineName);
        }

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
