using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDHero.BDROM;
using BDHero.Plugin;
using CsQuery;
using CsQuery.Web;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;

namespace IsanPlugin
{
    public class IsanMetadataProvider
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TimeSpan Timeout = ServerConfig.Default.Timeout;

        private const double TotalRequests = 2;

        /// <summary>
        /// Spoof IE6 on XP.
        /// </summary>
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322)";

        private const string IsanLookupUrl = "http://web.isan.org/template/1.2/publicSearch.do?code={0}";

        private static readonly Regex TitleYearLengthRegex = new Regex(@"(.*)\s+\(\s*(\d{4})[\s\-]*(\d+)\s*min\s*\)", RegexOptions.IgnoreCase);

        private readonly ProgressToken _token;

        private double _numRequests;

        /// <summary>
        /// Constructs a new IsanMetadataProvider that does <strong>not</strong> report its progress and
        /// does <strong>not</strong> allow the user to cancel requests.
        /// </summary>
        public IsanMetadataProvider() : this(new ProgressToken())
        {
        }

        /// <summary>
        /// Constructs a new IsanMetadataProvider that reports its progress and allows the user to cancel requests
        /// via the given <paramref name="token"/>.
        /// </summary>
        /// <param name="token"></param>
        public IsanMetadataProvider(ProgressToken token)
        {
            _token = token;
        }

        public void Populate([CanBeNull] VIsan vIsan)
        {
            if (vIsan == null || !vIsan.IsSearchable)
                return;

            var dom = GetDom(vIsan);

            Populate(vIsan, dom);
            SetParent(vIsan, dom);
            PopulateParent(vIsan);

            _token.ReportProgress(100.0, "Finished querying isan.org");
        }

        private CQ GetDom(Isan isan)
        {
            if (_token.IsCancelled)
                return null;

            string status;

            status = string.Format("Querying isan.org for {0} {1}...", isan is VIsan ? "V-ISAN" : "ISAN", isan.NumberFormatted);
            _token.ReportProgress(100.0 * _numRequests / TotalRequests, status);

            var numberEscaped = Uri.EscapeUriString(isan.Number);
            var url = string.Format(IsanLookupUrl, numberEscaped);

            CQ dom = CQ.CreateFromUrl(url, new ServerConfig { UserAgent = UserAgent, Timeout = Timeout });

            status = string.Format("Got response from isan.org for {0} {1}...", isan is VIsan ? "V-ISAN" : "ISAN", isan.NumberFormatted);
            _token.ReportProgress(100.0 * (++_numRequests) / TotalRequests, status);

            return dom;
        }

        private void Populate(Isan isan, [CanBeNull] CQ dom)
        {
            if (_token.IsCancelled || dom == null)
                return;

            var titles = dom[".title"];
            titles.ForEach(delegate(IDomObject o)
                {
                    var innerText = (o.InnerText ?? "").UnescapeHtml().Trim();
                    var fullText = o.Render().StripHtml().UnescapeHtml().Trim();
                    TryPopulate(isan, innerText);
                    TryPopulate(isan, fullText);
                });
        }

        private static void TryPopulate(Isan isan, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            var title = text.StripLeadingIsan().Trim();
            if (Isan.IsIsan(text))
                Logger.DebugFormat("Full V-ISAN: {0}", text);
            if (IsTitleYearLength(title))
                SetTitleYearLength(isan, title);
        }

        private static bool IsTitleYearLength(string text)
        {
            return TitleYearLengthRegex.IsMatch(text);
        }

        private static void SetTitleYearLength(Isan isan, string text)
        {
            var match = TitleYearLengthRegex.Match(text);
            isan.Title = match.Groups[1].Value.Trim();
            isan.Year = Int32.Parse(match.Groups[2].Value.Trim());
            isan.LengthMin = Int32.Parse(match.Groups[3].Value.Trim());
        }

        private void SetParent(VIsan vIsan, CQ dom)
        {
            if (_token.IsCancelled || dom == null)
                return;

            var titles = dom["a[href*=\"javascript:publicDisplayWork\"]"];
            titles.ForEach(delegate(IDomObject o)
                {
                    var href = (o.GetAttribute("href") ?? "").Trim();
                    if (Isan.IsIsan(href))
                    {
                        vIsan.Parent = Isan.TryParse(href);
                    }
                });
        }

        private void PopulateParent(VIsan vIsan)
        {
            if (_token.IsCancelled || vIsan.Parent == null)
                return;

            var dom = GetDom(vIsan.Parent);

            Populate(vIsan.Parent, dom);
        }
    }

    internal static class StringExtensions
    {
        private static readonly Regex LeadingIsanRegex = new Regex(@"\b(?:(?:V-)?ISAN\s*)?([0-9a-f]{4})-([0-9a-f]{4})-([0-9a-f]{4})-([0-9a-f]{4})-?[a-z0-9]?-?([0-9a-f]{4})-?([0-9a-f]{4})-?[a-z0-9]?\b", RegexOptions.IgnoreCase);

        // TODO: Write unit tests
        public static string StripLeadingIsan(this string str)
        {
            return LeadingIsanRegex.Replace(str, "");
        }
    }
}
