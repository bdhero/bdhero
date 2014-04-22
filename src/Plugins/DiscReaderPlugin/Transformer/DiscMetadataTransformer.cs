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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BDHero.BDROM;
using BDHero.JobQueue;
using BDHero.Plugin.DiscReader.Exceptions;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using DotNetUtils.FS;
using I18N;
using IniParser;

namespace BDHero.Plugin.DiscReader.Transformer
{
    public static class DiscMetadataTransformer
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Regex BdmtFileNameRegex = new Regex(
            "bdmt_([a-z]{3}).xml",
            RegexOptions.IgnoreCase);

        private static readonly Regex BdmtXmlTitleRegex = new Regex(
            "<di:name>(.*?)</di:name>",
            RegexOptions.IgnoreCase);

        private static readonly Regex DboxTitleRegex = new Regex(
            "<Title>(.*?)</Title>",
            RegexOptions.IgnoreCase);

        private static readonly Regex ISANContentIdRegex = new Regex(
            "<mcmf[^>]+contentID=\"(?:0x)?[a-z0-9]+([a-z0-9]{24})\"",
            RegexOptions.IgnoreCase);

        public static void Transform(Disc disc)
        {
            var raw = new DiscMetadata.RawMetadata
                        {
                            HardwareVolumeLabel = TryGet(disc, () => GetHardwareVolumeLabel(disc)),
                            DiscInf             = TryGet(disc, () => GetAnyDVDDiscInf(disc)),
                            AllBdmtTitles       = TryGet(disc, () => GetAllBdmtTitles(disc)),
                            DboxTitle           = TryGet(disc, () => GetDboxTitle(disc)),
                            V_ISAN              = TryGet(disc, () => GetVISAN(disc)),
                        };

            var derived = new DiscMetadata.DerivedMetadata
                            {
                                VolumeLabel          = TryGet(disc, () => GetVolumeLabel(raw)),
                                VolumeLabelSanitized = TryGet(disc, () => GetVolumeLabelSanitized(raw)),
                                ValidBdmtTitles      = TryGet(disc, () => GetValidBdmtTitles(raw.AllBdmtTitles)),
                                DboxTitleSanitized   = TryGet(disc, () => GetDboxTitleSanitized(raw)),
                                SearchQueries        = TryGet(disc, () => new List<SearchQuery>()), /* populated by DiscTransformer */
                            };

            var metadata = new DiscMetadata
                            {
                                Raw = raw,
                                Derived = derived
                            };

            disc.Metadata = metadata;
        }

        private static TResult TryGet<TResult>(Disc disc, Func<TResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Disc.FileSystem:\n{0}", disc.FileSystem.ToJsonPlainIndented().Indent()));
                throw new DiscMetadataTransformerException("Error occurred while retrieving metadata from disc.", ex);
            }
        }

        #region Derived methods

        private static string GetVolumeLabel(DiscMetadata.RawMetadata raw)
        {
            return raw.DiscInf != null ? raw.DiscInf.VolumeLabel : raw.HardwareVolumeLabel;
        }

        private static string GetVolumeLabelSanitized(DiscMetadata.RawMetadata raw)
        {
            return SanitizeVolumeLabel(GetVolumeLabel(raw));
        }

        private static string SanitizeVolumeLabel(string volumeLabel)
        {
            var sanitized = volumeLabel;

            sanitized = Regex.Replace(sanitized, @"^\d{6,}_", ""); // e.g., "01611720_GOODFELLAS" => "GOODFELLAS"
            sanitized = Regex.Replace(sanitized, @"_NA$", ""); // remove trailing region codes (NA = North America)
            sanitized = Regex.Replace(sanitized, @"_BD$", ""); // remove redundant trailing "BD" (we already know it's a Blu-ray Disc...)
            sanitized = Regex.Replace(sanitized, @"_+", " ");

            return sanitized;
        }

        private static IDictionary<Language, string> GetValidBdmtTitles(IDictionary<Language, string> allBdmtTitles)
        {
            var valid = new Dictionary<Language, string>();
            var filtered = allBdmtTitles.Select(SanitizeTitle).Where(IsBdmtTitleValid);
            foreach (var kvp in filtered)
            {
                valid[kvp.Key] = kvp.Value;
            }
            return valid;
        }

        private static KeyValuePair<Language, string> SanitizeTitle(KeyValuePair<Language, string> pair)
        {
            return new KeyValuePair<Language, string>(pair.Key, SanitizeTitle(pair.Value));
        }

        /// <summary>
        /// Sanitizes a title by removing useless garbage.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Sanitized version of the title if it is not empty, otherwise <c>null</c></returns>
        [CanBeNull]
        private static string SanitizeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return null;

            var sanitized = title.Trim();

            // TODO: Handle "(500) Days of Summer"
            sanitized = Regex.Replace(sanitized, @" - Blu-ray.*", "", RegexOptions.IgnoreCase);
            sanitized = Regex.Replace(sanitized, @" \(?Disc \w+(?: of \w+)?\)?", "", RegexOptions.IgnoreCase);
            sanitized = Regex.Replace(sanitized, @"\s*[[(].*", "", RegexOptions.IgnoreCase);
            sanitized = Regex.Replace(sanitized, @"[^\w']+", " ", RegexOptions.IgnoreCase);
            sanitized = Regex.Replace(sanitized, @" {2,}", " ", RegexOptions.IgnoreCase);
            sanitized = sanitized.Trim();

            if (Regex.IsMatch(sanitized, "blu.?ray", RegexOptions.IgnoreCase))
            {
                sanitized = "";
            }

            return string.IsNullOrWhiteSpace(sanitized) ? null : sanitized;
        }

        private static bool IsBdmtTitleValid(KeyValuePair<Language, string> keyValuePair)
        {
            var title = (keyValuePair.Value ?? "").Trim();
            return !string.IsNullOrWhiteSpace(title);
        }

        private static string GetDboxTitleSanitized(DiscMetadata.RawMetadata raw)
        {
            return SanitizeTitle(raw.DboxTitle);
        }

        #endregion

        #region Raw methods

        private static string GetHardwareVolumeLabel(Disc disc)
        {
            var root = disc.FileSystem.Directories.Root;
            var parent = Directory.GetParent(root.FullName);

            if (parent == null)
            {
                // path is the root directory
                var drives = DriveInfo.GetDrives().ToArray();
                var drive = drives.FirstOrDefault(info => info.Name == root.FullName);
                if (drive != null)
                {
                    return drive.VolumeLabel;
                }
            }

            return root.Name;
        }

        private static AnyDVDDiscInf GetAnyDVDDiscInf(Disc disc)
        {

            // TODO: REMOVE ME
            throw new Exception();
            var discInf = disc.FileSystem.Files.AnyDVDDiscInf;
            if (discInf == null)
                return null;
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(discInf.FullName);
            var discData = data["disc"];
            var anyDVDDiscInf = new AnyDVDDiscInf
                {
                    AnyDVDVersion = discData["version"],
                    VolumeLabel = discData["label"],
                    Region = RegionCodeParser.Parse(discData["region"])
                };
            return anyDVDDiscInf;
        }

        private static IDictionary<Language, string> GetAllBdmtTitles(Disc disc)
        {
            var titles = new Dictionary<Language, string>();
            foreach (var file in disc.FileSystem.Files.BDMT)
            {
                var filenameMatch = BdmtFileNameRegex.Match(file.Name);
                var iso639_2 = filenameMatch.Groups[1].Value;
                var language = Language.FromCode(iso639_2);
                var xml = string.Join("", File.ReadLines(file.FullName));

                if (!BdmtXmlTitleRegex.IsMatch(xml))
                    continue;

                var xmlMatch = BdmtXmlTitleRegex.Match(xml);
                var title = xmlMatch.Groups[1].Value.Trim();

                titles[language] = title;
            }
            return titles;
        }

        private static string GetDboxTitle(Disc disc)
        {
            var dbox = disc.FileSystem.Files.DBox;
            if (dbox == null)
                return null;

            string xml;
            var encoding = FileUtils.DetectEncodingAuto(dbox.FullName, out xml);

            // Replace newlines with spaces
            xml = Regex.Replace(xml, @"[\n\r\f]+", " ");

            if (!DboxTitleRegex.IsMatch(xml))
                return null;

            var match = DboxTitleRegex.Match(xml);
            var title = SanitizeTitle(match.Groups[1].Value);
            return title;
        }

        private static VIsan GetVISAN(Disc disc)
        {
            var file = disc.FileSystem.Files.MCMF;
            if (file == null)
                return null;

            var xml = string.Join(" ", File.ReadAllLines(file.FullName));
            if (!ISANContentIdRegex.IsMatch(xml))
                return null;

            var match = ISANContentIdRegex.Match(xml);
            var contentId = match.Groups[1].Value;

            return VIsan.TryParse(contentId);
        }

        #endregion
    }
}
