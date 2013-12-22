using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils.Extensions;
using I18N;
using WatTmdb.V3;
using Newtonsoft.Json;
using log4net;

namespace TmdbPlugin
{
    public class TmdbPlugin : IMetadataProviderPlugin
    {
        #region Fields

        private static readonly ILog Logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Tmdb _tmdbApi;
        private TmdbMovieSearch _tmdbMovieSearch;
        private string _rootImageUrl;
        private string _searchISO_639_1;
        private string _apiKey;
        private TmdbConfiguration _configuration;

        #endregion
        
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "TMDb"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return Resources.tmdb_icon; } }

        public int RunOrder { get { return 1; } }

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

            StartProgress("Loading plugin preferences...");

            var prefs = LoadPreferences();

            if (cancellationToken.IsCancellationRequested)
                return;

            Search(job);

            if (cancellationToken.IsCancellationRequested)
                return;

            GetPosters(job);

            FinishProgress("Finished querying TMDb");
        }

        #region Progress reporting

        private double _totalSteps = 5; // 5 = 1 config req + 3 search attempts + 1 poster image req
        private double _stepsCompleted;

        private void StartProgress(string status)
        {
            _stepsCompleted = 0;
            Host.ReportProgress(this, 100.0 * (_stepsCompleted / _totalSteps), status);
        }

        private void MakeProgress(string status)
        {
            Host.ReportProgress(this, 100.0 * (++_stepsCompleted / _totalSteps), status);
        }

        private void FinishProgress(string status)
        {
            Host.ReportProgress(this, 100.0, status);
        }

        #endregion

        #region Plugin Preferences

        /// <summary>
        /// Attempts to load the user's preferences file from disk if it exists,
        /// otherwise returns the default set of preferences.
        /// </summary>
        /// <returns></returns>
        private TmdbPreferences LoadPreferences()
        {
            var prefs = PluginUtils.GetPreferences(AssemblyInfo, () => new TmdbPreferences());

            // Use our default API key if the user has not specified their own
            if (string.IsNullOrWhiteSpace(prefs.ApiKey))
                prefs.ApiKey = new TmdbPreferences().ApiKey;

            _searchISO_639_1 = prefs.DefaultLanguage;
            _apiKey = prefs.ApiKey;

            return prefs;
        }

        /// <summary>
        /// Persists the user's preferences by serializing <paramref name="prefs"/> as JSON
        /// and saving it to disk.
        /// </summary>
        /// <param name="prefs"></param>
        private void SavePreferences(TmdbPreferences prefs)
        {
            var apiKey = prefs.ApiKey;

            // Don't save the default API key to the user's preferences file
            if (apiKey == new TmdbPreferences().ApiKey)
                prefs.ApiKey = null;

            PluginUtils.SavePreferences(AssemblyInfo, prefs);

            // Restore the API key if it was nulled out above
            prefs.ApiKey = apiKey;
        }

        #endregion

        #region TMDb Configuration

        private TmdbConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                Logger.Debug("Getting TMDb configuration");
                _configuration = _tmdbApi.GetConfiguration();
            }
            return _configuration;
        }

        private void GetBaseImageUrl()
        {
            if (string.IsNullOrEmpty(_rootImageUrl))
            {
                _rootImageUrl = GetConfiguration().images.base_url + "w185";
            }

            if (string.IsNullOrEmpty(_rootImageUrl))
            {
                Logger.Warn("Failed to obtain base image URL");
                return;
            }

            const string urlMessage = "api.themoviedb.org provided the following base image URL";
            Logger.InfoFormat("{0}: {1}", urlMessage, _rootImageUrl);
        }

        #endregion

        #region Movie Search

        private void Search(Job job)
        {
            var attempt = 0;
            var queries = job.SearchQueries.SelectMany(ConstructQueries).ToArray();

            _totalSteps = queries.Count() + 2; // 2 = 1 config req + 1 poster image req

            foreach (var query in queries)
            {
                QueryTmdb(++attempt, job, query);

                if (job.Movies.Any())
                    break;
            }
        }

        private static IList<SearchQuery> ConstructQueries(SearchQuery query)
        {
            var queries = new List<SearchQuery>();

            queries.Add(query);

            if (!query.Year.HasValue)
            {
                return queries;
            }

            // isan.org has the wrong year for some movies...
            // Search again w/ year + 1 if there are zero results for isan.org's year value
            queries.Add(new SearchQuery
                        {
                            Language = query.Language,
                            Title = query.Title,
                            Year = query.Year + 1
                        });

            // Search again w/o sending a year if there are still zero results
            queries.Add(new SearchQuery
                        {
                            Language = query.Language,
                            Title = query.Title,
                            Year = null
                        });

            return queries;
        }

        private void QueryTmdb(int attempt, Job job, SearchQuery query)
        {
            MakeProgress(string.Format("Querying TMDb w/ year = {0} (attempt {1})...", query.Year, attempt));
            ApiRequest(job, query);
        }

        private void ApiRequest(Job job, SearchQuery query)
        {
            job.Movies.Clear();

            var searchTitle = query.Title;
            var searchYear = query.Year;

            if (_apiKey == null)
            {
                const string message = "ERROR: No API key found";
                Logger.Error(message);
                throw new Exception(message);
            }

            _tmdbApi = new Tmdb(_apiKey, _searchISO_639_1);

            // TMDb (previously) choked on dashes - not sure if it still does or not...
            // E.G.: "The Amazing Spider-Man" --> "The Amazing Spider Man"
            searchTitle = Regex.Replace(searchTitle, @"-+", " ");

            var requestParameters = new TmdbApiParameters(searchTitle, searchYear, _searchISO_639_1);

            try
            {
                SearchTmdb(requestParameters, job);
            }
            catch (Exception ex)
            {
                HandleTmdbError(ex);
            }
        }

        private void SearchTmdb(TmdbApiParameters requestParameters, Job job)
        {
            GetBaseImageUrl();

            _tmdbMovieSearch = _tmdbApi.SearchMovie(requestParameters.Query, 1, requestParameters.Iso6391, false,
                                                    requestParameters.Year);

            if (_tmdbMovieSearch == null)
            {
                Logger.Warn("TMDb movie search returned null");
                return;
            }

            job.Movies.AddRange(_tmdbMovieSearch.results.Select(ToMovie));

            LogSearchResults();
        }

        private void LogSearchResults()
        {
            if (_tmdbMovieSearch == null || !_tmdbMovieSearch.results.Any())
                return;

            var results = new List<string>();

            foreach (var movie in _tmdbMovieSearch.results)
            {
                DateTime releaseYear;
                DateTime.TryParse(movie.release_date, out releaseYear);
                results.Add(String.Format("{0} ({1})", movie.original_title, releaseYear.Year));
            }

            const string bestGuessMessage = "Top TMDb search result";
            Logger.InfoFormat("{0}: {1} ", bestGuessMessage, _tmdbMovieSearch.results[0].original_title);

            const string matchesMessage = "TMDb returned the following matches";
            Logger.InfoFormat("{0}:\n{1} ", matchesMessage, string.Join(Environment.NewLine, results));
        }

        private Movie ToMovie(MovieResult movieResult, int i)
        {
            var releaseYear = GetReleaseYear(movieResult);
            var movie = new Movie
                {
                    Id = movieResult.id,
                    ReleaseYear = releaseYear,
                    Title = movieResult.title,
                    Url = string.Format("http://www.themoviedb.org/movie/{0}", movieResult.id),
                    IsSelected = i == 0
                };

            if (!string.IsNullOrEmpty(movieResult.poster_path))
            {
                movie.CoverArtImages.Add(new CoverArt
                                             {
                                                 Uri = _rootImageUrl + movieResult.poster_path,
                                                 IsSelected = true
                                             });
            }

            return movie;
        }

        private static int? GetReleaseYear(MovieResult movieResult)
        {
            int? releaseYear = null;
            DateTime releaseDate;
            if (DateTime.TryParse(movieResult.release_date, out releaseDate))
                releaseYear = releaseDate.Year;
            return releaseYear;
        }

        #endregion

        #region Poster Search

        private void GetPosters(Job job)
        {
            MakeProgress("Getting poster images...");

            foreach (var movie in job.Movies)
            {
                var tmdbMovieImages = new TmdbMovieImages();

                try
                {
                    if (string.IsNullOrEmpty(_rootImageUrl))
                    {
                        _rootImageUrl = GetConfiguration().images.base_url + "original";
                    }
                    tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, null);
                    var posterLanguages = (tmdbMovieImages.posters.Select(poster => poster.iso_639_1).ToList());
                    posterLanguages = posterLanguages.Distinct().ToList();

                    if (posterLanguages.Count == 0)
                    {
                        tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, "en");
                    }
                }
                catch (Exception ex)
                {
                    HandleTmdbError(ex);
                }

                if (tmdbMovieImages == null)
                    continue;

                foreach (var poster in tmdbMovieImages.posters)
                {
                    poster.file_path = _rootImageUrl + poster.file_path;

                    if (movie.CoverArtImages.All(x => x.Uri != poster.file_path))
                    {
                        movie.CoverArtImages.Add(new CoverArt
                        {
                            Uri = _rootImageUrl + poster.file_path,
                            Language = Language.FromCode(poster.iso_639_1)
                        });
                    }
                }
            }
        }

        #endregion

        #region Error Handling

        private void HandleTmdbError(Exception ex)
        {
            var tmdbResponse = _tmdbApi.ResponseContent;
            try
            {
                var pluginSettings = JsonConvert.DeserializeObject<TmdbApiErrors>(tmdbResponse);
                Logger.ErrorFormat("Error: api.themoviedb.org returned the following Status Code {0} : {1}",
                                   pluginSettings.StatusCode,
                                   pluginSettings.StatusMessage);
            }
            catch
            {
            }
        }

        #endregion

        #region Private Classes

        private class TmdbApiParameters
        {
            public readonly string Query;
            public readonly int? Year;
            public readonly string Iso6391;

            public TmdbApiParameters(string query, int? year, string iso6391)
            {
                Query = query;
                Year = year;
                Iso6391 = iso6391;
            }
        }

        private abstract class TmdbApiErrors
        {
            public int StatusCode { get; set; }
            public string StatusMessage { get; set; }
        }

        #endregion
    }
}
