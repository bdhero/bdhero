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
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using BDHero.BDROM;
using BDHero.JobQueue;
using BDHero.Plugin;
using DotNetUtils.Annotations;
using DotNetUtils.Net;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace ChapterGrabberPlugin
{
    [UsedImplicitly]
    public class ChapterGrabberPlugin : IMetadataProviderPlugin
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string Username = "BDHero";
        private const string ApiKey = "G88IO875M9SKU6DPB82F";

        private const string QueryUrl = "http://chapterdb.org/chapters/search?title=";
        private const string QueryParams = "&chapterCount=0 HTTP/1.1";

        // TODO: Make these user-configurable
        private const bool ShouldPrependChapterNumbers = false;
        private const string ChapterNumberFormat = "{0:D2}.";

        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "ChapterDb"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return Resources.chapterdb_icon; } }

        public int RunOrder { get { return 2; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void GetMetadata(CancellationToken cancellationToken, Job job)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 0.0, "Querying ChapterDb.org...");

            if (job.SearchQuery != null)
            {
                var apiResults = GetChapters(job.SearchQuery.Title);

                if (cancellationToken.IsCancellationRequested)
                    return;

                Host.ReportProgress(this, 90.0, "Matching results against playlists...", "Comparing search results to available playlists...");

                foreach(var moviePlaylist in job.Disc.Playlists)
                {
                    var apiValues = CompareChapters(apiResults, moviePlaylist.Chapters);
                    if (apiValues != null && apiValues.Count > 0)
                    {
                        StoreSearchResults(moviePlaylist, apiValues);

                        // To Do:  Allow the user to select which chapter list to use when 
                        // defaulted to [0] first chapter list that matches filter criteria
                        ReplaceChapters(apiValues[0], moviePlaylist.Chapters);

                        Logger.InfoFormat("Custom chapters were added to {0}", moviePlaylist.FileName);
                    }
                }    
            }

            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 100.0, "Finished querying ChapterDb.org");
        }

        private static List<JsonChaps> GetChapters(string movieName)
        {
            var movieSearchResults = new List<JsonChaps>();
            var headers = new List<string> { string.Format("ApiKey: {0}", ApiKey), string.Format("UserName: {0}", Username) };
            var doc = new XmlDocument();
            
            try
            {
                var xmlResponse = HttpRequest.Get(QueryUrl + movieName + QueryParams, headers);
                doc.LoadXml(xmlResponse);
            }
            catch (WebException ex)
            {
                using (var webResponse = ex.Response)
                {
                    var httpWebResponse = webResponse as HttpWebResponse;
                    if (httpWebResponse == null)
                        throw;

                    using (var responseStream = httpWebResponse.GetResponseStream())
                    using (var reader = new StreamReader(responseStream))
                    {
                        var errorBody = reader.ReadToEnd();
                        var regex = new Regex(@"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase);
                        var errorTitle = regex.Match(errorBody).Value.Trim();
                        throw new ChapterGrabberException(errorTitle, ex);
                    }
                }
            }

            if (doc.DocumentElement != null)
            {
                foreach (var node in doc.DocumentElement)
                {
                    var jsonText = Regex.Replace(JsonConvert.SerializeXmlNode((XmlElement) node, Formatting.Indented),
                                                 "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
                    jsonText = jsonText.Replace("?xml", "xml");

                    movieSearchResults.Add(JsonConvert.DeserializeObject<JsonChaps>(jsonText));
                }
            }
           
            if (movieSearchResults.Count > 0)
            {               
                Logger.InfoFormat("{0} possible matche(s) were found for {1}", movieSearchResults.Count, movieName);
            }
            else
            {
                Logger.InfoFormat("No matches were found for {0}", movieName);
            }

            return movieSearchResults;
        }

        #region Validation / comparison

        private static List<JsonChaps> CompareChapters(List<JsonChaps> apiData, IList<Chapter> discData)
        {
            var apiResultsFilteredByChapter = apiData.Where(chaps => IsMatch(chaps, discData)).ToList();
            var apiResultsFilteredByValidName = apiResultsFilteredByChapter.Where(IsValid).ToList();

            if (apiResultsFilteredByValidName.Any())
            {
                Logger.InfoFormat("{0} result(s) matched the filter criteria for custom chapters", apiResultsFilteredByValidName.Count);
            }

            return apiResultsFilteredByValidName;
        }

        private static bool IsMatch(JsonChaps jsonChaps, IList<Chapter> chapterDisc)
        {
            var chapterCountMatches = jsonChaps.chapterInfo.chapters.chapter.Count == chapterDisc.Count ||
                                      jsonChaps.chapterInfo.chapters.chapter.Count == chapterDisc.Count + 1;
            if (!chapterCountMatches)
                return false;

            for (var i = 0; i < chapterDisc.Count; i++)
            {
                var discChapter = chapterDisc[i];
                var apiChapter = jsonChaps.chapterInfo.chapters.chapter[i];
                if (!DoTimecodesMatch(discChapter.StartTime.TotalSeconds, apiChapter.time.TotalSeconds))
                    return false;
            }
            return true;
        }

        private static bool DoTimecodesMatch(double timeDisc, double timeApi)
        {
            return Math.Abs(timeDisc - timeApi) <= 1.0;
        }

        private static bool IsValid(JsonChaps jsonChaps)
        {
            List<JsonChapter> jsonChapters = jsonChaps.chapterInfo.chapters.chapter;
            var areAllInvalid = jsonChapters.All(IsInvalidChapter);
            return !areAllInvalid;
        }

        private static bool IsInvalidChapter(JsonChapter jsonChapter)
        {
            return !IsValidChapterName(jsonChapter.name);
        }

        private static bool IsValidChapterName([CanBeNull] string chapterName)
        {
            var trimmed = (chapterName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                return false;
            TimeSpan parsed;
            if (TimeSpan.TryParse(trimmed, out parsed))
                return false;
            if (Regex.IsMatch(trimmed, @"^(?:Chapter|Scene|Kapitel)?[\W_]*[0-9]+\s*$", RegexOptions.IgnoreCase))
                return false;
            return true;
        }

        #endregion

        #region Storage

        private static void StoreSearchResults(Playlist playlist, IEnumerable<JsonChaps> searchResults)
        {
            var validResults = searchResults.Where(chaps => IsMatch(chaps, playlist.Chapters)).Where(IsValid).ToArray();
            playlist.ChapterSearchResults = validResults.Select(searchResult => Transform(searchResult, playlist)).ToList();
        }

        private static void ReplaceChapters(JsonChaps apiData, IList<Chapter> discData)
        {
            for (var i=0; i<discData.Count; i++)
            {
                var chapter = discData[i];
                var jsonChapter = apiData.chapterInfo.chapters.chapter[i];
                chapter.Title = jsonChapter.name;
            }
        }

        #endregion

        #region Transformation

        private static ChapterSearchResult Transform(JsonChaps searchResult, Playlist playlist)
        {
            var jsonChapters = searchResult.chapterInfo.chapters.chapter.Take(playlist.ChapterCount).ToArray();
            var chapters = jsonChapters.Select(Transform).ToList();

            if (ShouldStripLeadingDigits(chapters))
                StripLeadingDigits(chapters);

            if (ShouldPrependChapterNumbers)
                PrependChapterNumbers(chapters);

            return new ChapterSearchResult
            {
                Title =
                    string.Format("+{0}: {1}", searchResult.chapterInfo.confirmations,
                                  searchResult.chapterInfo.title),
                Chapters = chapters
            };
        }

        private static Chapter Transform(JsonChapter jsonChapter, int i)
        {
            var title = (jsonChapter.name ?? "").Trim();
            var isValidChapterName = IsValidChapterName(title);
            var chapter = new Chapter(i + 1, jsonChapter.time.TotalSeconds) { Title = title, Keep = isValidChapterName };
            return chapter;
        }

        #endregion

        #region Number stripping

        private static readonly Regex LeadingDigitsRegex = new Regex(@"^(?<number>\d+)[.:) ]+");

        private static bool ShouldStripLeadingDigits(List<Chapter> chapters)
        {
            var numWithLeadingDigits = chapters.Count(ShouldStripLeadingDigits);
            return (numWithLeadingDigits >= chapters.Count - 2);
        }

        private static bool ShouldStripLeadingDigits(Chapter chapter)
        {
            if (!LeadingDigitsRegex.IsMatch(chapter.Title))
                return false;
            
            int intValue;
            var strValue = LeadingDigitsRegex.Match(chapter.Title).Groups["number"].Value;

            // This should never happen, but you never know
            if (!int.TryParse(strValue, out intValue))
                return false;

            // Leading digits must match the chapter number (+/- 1)
            return (intValue == chapter.Number || intValue == chapter.Number + 1 || intValue == chapter.Number - 1);
        }

        private static void StripLeadingDigits(List<Chapter> chapters)
        {
            chapters.ForEach(StripLeadingDigits);
        }

        private static void StripLeadingDigits(Chapter chapter)
        {
            chapter.Title = LeadingDigitsRegex.Replace(chapter.Title, "");
        }

        #endregion

        #region Chapter number prepending

        private static void PrependChapterNumbers(List<Chapter> chapters)
        {
            chapters.ForEach(chapter => chapter.Title = string.Format("{0} {1}", string.Format(ChapterNumberFormat, chapter.Number), chapter.Title));
        }

        #endregion
    }
}
