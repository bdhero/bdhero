using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DotNetUtils.Crypto
{
    public class CryptoHashInput
    {
        [JsonProperty(PropertyName = "name")]
        public readonly string Name;

        [JsonProperty(PropertyName = "size")]
        public readonly long Size;

        [JsonProperty(PropertyName = "humanSize")]
        public string HumanSize { get { return FileUtils.HumanFriendlyFileSize(Size); } }

        [JsonProperty(PropertyName = "algorithms")]
        public readonly IDictionary<string, string> Algorithms;

        public CryptoHashInput(string path, IEnumerable<CryptoHashAlgorithm> algorithms) :
            this(Path.GetFileName(path), File.ReadAllBytes(path), algorithms)
        {
        }

        public CryptoHashInput(string name, Stream stream, IEnumerable<CryptoHashAlgorithm> algorithms) :
            this(name, FileUtils.ReadStream(stream), algorithms)
        {
        }

        public CryptoHashInput(string name, byte[] buffer, IEnumerable<CryptoHashAlgorithm> algorithms)
        {
            Name = name;
            Size = buffer.Length;
            Algorithms = new Dictionary<string, string>();

            foreach (var algorithm in algorithms)
            {
                Algorithms[algorithm.MachineName] = algorithm.ComputeBytes(buffer);
            }
        }
    }
}
