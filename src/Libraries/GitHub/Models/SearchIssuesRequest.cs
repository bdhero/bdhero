using System;
using System.Collections.Generic;
using System.Linq;
using DotNetUtils.Net;

namespace GitHub.Models
{
    internal class SearchIssuesRequest : IGitHubRequest<SearchIssuesResponse>
    {
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

            var query = string.Format("{0}:+{1}+{2}", exception.GetType().FullName, exception.Message, JoinQualifiers(qualifiers));

            _url = string.Format("https://api.github.com/search/issues?q={0}", query);
        }

        private static string JoinQualifiers(Dictionary<string, string> qualifiers)
        {
            return string.Join("+", qualifiers.Select(JoinQualifier));
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
