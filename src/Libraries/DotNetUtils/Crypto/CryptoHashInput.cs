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

using System.Collections.Generic;
using System.IO;
using DotNetUtils.FS;
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
