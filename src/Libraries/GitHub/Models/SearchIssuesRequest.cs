using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetUtils.Net;
using RestSharp.Extensions;

namespace GitHub.Models
{
    internal class SearchIssuesRequest : IGitHubRequest<SearchIssuesResponse>
    {
        private const int MaxQueryLength = 256;
        private const string QuerySeparatorUnencoded = " ";
        private const string QuerySeparatorEncoded = "%20";
        private const string QuerySeparatorProper = "+";

        private readonly string _url;

        public SearchIssuesRequest(Exception exception, string repo)
        {
            var qualifiers = new Dictionary<string, string>
                             {
                                 { "type", "issue" },
                                 { "state", "open" },
                                 { "label", CreateIssueRequest.DefaultLabel },
                                 { "repo", repo }
                             };

            var exceptionStr = Queryable(exception);
            var qualifiersStr = JoinQualifiers(qualifiers);

            var exceptionStrTrimmed = exceptionStr;

            // The GitHub Search API only allows 256 characters in the query param
            if (exceptionStr.Length + qualifiersStr.Length + QuerySeparatorUnencoded.Length > MaxQueryLength)
            {
                var endPos = MaxQueryLength - qualifiersStr.Length - QuerySeparatorUnencoded.Length;

                exceptionStrTrimmed = exceptionStr.Substring(0, endPos);

                // Ensure that the search query ENDS with a space.
                // Queries that get cut off mid-word (e.g., "System.IO.IOEx|ception", "System|.IO.IOException")
                // do not return any results.
                var after = exceptionStr.Substring(endPos);
                if (new Regex(@"\S$").IsMatch(exceptionStr) && new Regex(@"^\S").IsMatch(after))
                {
                    exceptionStrTrimmed = new Regex(@"\S+$").Replace(exceptionStrTrimmed, "");
                }
            }

            exceptionStrTrimmed = exceptionStrTrimmed.Trim();

            var query = string.Format("{0}{1}{2}", exceptionStrTrimmed, QuerySeparatorUnencoded, qualifiersStr);
            var queryEncoded = query.UrlEncode().Replace(QuerySeparatorEncoded, QuerySeparatorProper);

            _url = string.Format("https://api.github.com/search/issues?q={0}", queryEncoded);
        }

        private static string Queryable(Exception exception)
        {
            var lines = new Regex(@"[\n\r\f]+").Split(exception.ToString()).Select(line => line.Trim());
            var concat = string.Join(" ", lines);

            return concat;
        }

        private static string JoinQualifiers(Dictionary<string, string> qualifiers)
        {
            return string.Join(QuerySeparatorUnencoded, qualifiers.Select(JoinQualifier));
        }

        private static string JoinQualifier(KeyValuePair<string, string> pair)
        {
            return string.Format("{0}:{1}", pair.Key, pair.Value);
        }

        public HttpRequestMethod Method
        {
            get { return HttpRequestMethod.Get; }
        }

        public string Url
        {
            get { return _url; }
        }
    }
}
