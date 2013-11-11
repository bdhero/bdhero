﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetUtils.Crypto;
using DotNetUtils.Extensions;
using Mono.Options;
using Newtonsoft.Json;

namespace Hasher
{
    class Program
    {
        private static bool _verbose;
        private static bool _json;
        private static bool _map;

        private static Stream StdIn
        {
            get
            {
                try
                {
                    bool keyAvailable = Console.KeyAvailable;
                    // If we get to this point without throwing an exception,
                    // it means that stdin has not been redirected.
                    return null;
                }
                catch (InvalidOperationException)
                {
                    // Get input redirected from a file or piped in
                    return Console.OpenStandardInput();
                }
            }
        }

        static void Main(string[] args)
        {
            var inputs = new List<CryptoHashInput>();
            var algorithms = new HashSet<CryptoHashAlgorithm>();

            var optionSet = new OptionSet
                {
                    { "h|?|help", s => PrintUsageAndExit() },
                    { "V|verbose", s => _verbose = true },
                    { "json", s => _json = true },
                    { "map", s => _map = true },
                    { "lower", s => CryptoHashAlgorithm.LowerCase = true },
                    { "md5", s => algorithms.Add(new MD5Algorithm()) },
                    { "sha1", s => algorithms.Add(new SHA1Algorithm()) },
                    { "sha256", s => algorithms.Add(new SHA256Algorithm()) },
                    { "sha512", s => algorithms.Add(new SHA512Algorithm()) },
                };

            var paths = optionSet.Parse(args);

            if (!algorithms.Any())
            {
                algorithms.Add(new MD5Algorithm());
                algorithms.Add(new SHA1Algorithm());
                algorithms.Add(new SHA256Algorithm());
                algorithms.Add(new SHA512Algorithm());
            }

            var stdin = StdIn;
            if (stdin != null)
            {
                inputs.Add(new CryptoHashInput("stdin", stdin, algorithms));
            }

            inputs.AddRange(paths.Select(path => new CryptoHashInput(path, algorithms)));

            Print(inputs);
        }

        private static void Print(List<CryptoHashInput> inputs)
        {
            if (_json)
                PrintJson(inputs);
            else
                PrintText(inputs);
        }

        private static void PrintJson(List<CryptoHashInput> inputs)
        {
            Object obj = inputs;
            if (_map)
            {
                var map = new Dictionary<string, CryptoHashInput>();
                map.AddRange(inputs.Select(input => new KeyValuePair<string, CryptoHashInput>(input.Name, input)));
                obj = map;
            }
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        private static void PrintText(List<CryptoHashInput> inputs)
        {
            var i = 0;
            foreach (var input in inputs)
            {
                if (_verbose)
                {
                    Console.WriteLine("Name: {0}", input.Name);
                    Console.WriteLine("Size: {0} bytes ({1})", input.Size, input.HumanSize);

                    foreach (var algorithmMachineName in input.Algorithms.Keys)
                    {
                        Console.WriteLine("{0}: {1}", algorithmMachineName, input.Algorithms[algorithmMachineName]);
                    }
                }
                else
                {
                    foreach (var algorithmMachineName in input.Algorithms.Keys)
                    {
                        Console.WriteLine(input.Algorithms[algorithmMachineName]);
                    }
                }
                if (_verbose && i < inputs.Count - 1)
                {
                    Console.WriteLine("~~~~~~~~~~~~");
                }
                i++;
            }
        }

        private static void PrintUsageAndExit()
        {
            var exeName = Assembly.GetEntryAssembly().GetName().Name;
            var usage = new Usage(exeName).TransformText();
            Console.Error.WriteLine(usage);
            Environment.Exit(0);
        }
    }
}
