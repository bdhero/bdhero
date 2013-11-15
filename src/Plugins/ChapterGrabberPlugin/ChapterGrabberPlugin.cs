﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Xml;
using BDHero.BDROM;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils.Annotations;
using DotNetUtils.Net;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace ChapterGrabberPlugin
{
    [UsedImplicitly]
    public class ChapterGrabberPlugin : IMetadataProviderPlugin
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string ApiGet = "http://chapterdb.org/chapters/search?title=";
        private const string ApiGetEnd = "&chapterCount=0 HTTP/1.1";
        private static string xmlResponse;

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

            //var playlist = job.Disc.Playlists[job.SelectedPlaylistIndex];
            if (job.SearchQuery != null)
            {
                var apiResults = GetChapters(job.SearchQuery.Title);
                /*
                var apiValues = CompareChapters(apiResults, playlist.Chapters);
                if (apiValues != null && apiValues.Count > 0)
                {
                    ReplaceChapters(apiValues[0], playlist.Chapters);
                }*/

                if (cancellationToken.IsCancellationRequested)
                    return;

                Host.ReportProgress(this, 90.0, "Comparing search results to available playlists...");

                foreach(var moviePlaylist in job.Disc.Playlists)
                {
                    var apiValues = CompareChapters(apiResults, moviePlaylist.Chapters);
                    if (apiValues != null && apiValues.Count > 0)
                    {
                        StoreSearchResults(apiValues, moviePlaylist);

                        // To Do:  Allow the user to select which chapter list to use when 
                        // defaulted to [0] first chapter list that matches filter criteria
                        ReplaceChapters(apiValues[0], moviePlaylist.Chapters);

                        var Message = "Custom chapters were added to: " + moviePlaylist.FileName;
                        Logger.Info(Message);
                    }
                }    
            }

            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 100.0, "Finished querying ChapterDb.org");
        }

        private static void StoreSearchResults(IEnumerable<JsonChaps> searchResults, Playlist playlist)
        {
            var validResults = searchResults.Where(chaps => IsMatch(chaps, playlist.Chapters)).Where(IsValid).ToArray();
            playlist.ChapterSearchResults = validResults.Select(searchResult => Transform(searchResult, playlist)).ToList();
        }

        private static ChapterSearchResult Transform(JsonChaps searchResult, Playlist playlist)
        {
            var jsonChapters = searchResult.chapterInfo.chapters.chapter.Take(playlist.ChapterCount).ToArray();
            return new ChapterSearchResult
                {
                    Title =
                        string.Format("+{0}: {1}", searchResult.chapterInfo.confirmations,
                                      searchResult.chapterInfo.title),
                    Chapters = jsonChapters.Select(Transform).ToList()
                };
        }

        private static Chapter Transform(JsonChapter jsonChapter, int i)
        {
            var title = (jsonChapter.name ?? "").Trim();
            var isValidChapterName = IsValidChapterName(title);
            var chapter = new Chapter(i + 1, jsonChapter.time.TotalSeconds) { Title = title, Keep = isValidChapterName };
            return chapter;
        }

        static private List<JsonChaps> GetChapters(string movieName)
        {
            var movieSearchResults = new List<JsonChaps>();
            var headers = new List<string>();
                headers.Add("ApiKey: G88IO875M9SKU6DPB82F");
                headers.Add("UserName: BDHero");
            var doc = new XmlDocument();
            
            try
            {
                xmlResponse = HttpRequest.Get(ApiGet + movieName + ApiGetEnd, headers);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            var errorBody = reader.ReadToEnd();
                            var errorRegex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                            var rex = new Regex(errorRegex, RegexOptions.IgnoreCase);
                            var errorTitle = rex.Match(errorBody).Value.Trim();
                            throw new Exception("Error: " + errorTitle, ex);
                        }
                    }
                }
                else
                {
                    throw new Exception("Error: An error occurred when contacting chapterdb.org", ex);
                }
            }

            try
            {
                doc.LoadXml(xmlResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: An error occurred when processing the response from chapterdb.org", ex);
            }

            if (doc.DocumentElement != null)
            {
                foreach (var node in doc.DocumentElement)
                {
                    string jsonText;
                    try
                    {
                        jsonText = Regex.Replace(JsonConvert.SerializeXmlNode((XmlElement)node, Formatting.Indented),
                                                "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
                        jsonText = jsonText.Replace("?xml", "xml");

                        movieSearchResults.Add(JsonConvert.DeserializeObject<JsonChaps>(jsonText));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error: An error occurred when serializing the response from chapterdb.org", ex);
                    }
                }
            }
           
            if (movieSearchResults.Count > 0)
            {               
                var Message = movieSearchResults.Count + " possible matches were found for " + movieName;
                Logger.Info(Message);
            }
            else
            {
                var Message = "No matches were found for " + movieName;
                Logger.Info(Message);
            }
            return movieSearchResults;
        }

        static private List<JsonChaps> CompareChapters(List<JsonChaps> apiData, IList<Chapter> discData)
        {
            var apiResultsFilteredByChapter = apiData.Where(chaps => IsMatch(chaps, discData)).ToList();
            
            var apiResultsFilteredByValidName = apiResultsFilteredByChapter.Where(IsValid).ToList();

            if (apiResultsFilteredByValidName.Count > 0)
            {
                var Message = apiResultsFilteredByValidName.Count + " result(s) matched the filter criteria for custom chapters";
                Logger.Info(Message);
            }

            return apiResultsFilteredByValidName;
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
            var test = Math.Abs(timeDisc - timeApi);
            return Math.Abs(timeDisc - timeApi) <= 1.0;
        }

        static private void ReplaceChapters(JsonChaps apiData, IList<Chapter> discData)
        {
            for (var i=0; i<discData.Count; i++)
            {
                var chapter = discData[i];
                var jsonChapter = apiData.chapterInfo.chapters.chapter[i];
                chapter.Title = jsonChapter.name;
            }
        }
    }
}
