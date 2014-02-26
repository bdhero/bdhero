using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Net;
using GitHub.Exceptions;
using GitHub.Models;

namespace GitHub
{
    /// <summary>
    ///     REST client for accessing the GitHub public API.
    /// </summary>
    public class GitHubClient
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _repo;
        private readonly string _oauthToken;

        /// <summary>
        ///     Constructs a new <see cref="GitHubClient"/> instance with the given parameters.
        /// </summary>
        /// <param name="repo">
        ///     User login and repository name separated by a slash (e.g., <c>"user/repo"</c>).
        /// </param>
        /// <param name="oauthToken">
        ///     OAuth2 Token.  Required to perform restricted actions (e.g., create an issue or comment).
        /// </param>
        public GitHubClient(string repo, string oauthToken)
        {
            _repo = repo;
            _oauthToken = oauthToken;
        }

        #region Public API

        /// <summary>
        ///     Searches existing issues for the given <paramref name="exception"/> and returns a list of results.
        /// </summary>
        /// <param name="searchStr">
        ///     Text to search for.
        /// </param>
        /// <returns>
        ///     A list of search results that match the given <paramref name="exception"/>.
        /// </returns>
        /// <exception cref="WebException">
        ///     Thrown if no response is received.
        /// </exception>
        /// <exception cref="RequestException">
        ///     Thrown if the GitHub API is unable to complete the request and responds with an error message.
        /// </exception>
        [NotNull]
        public SearchIssuesResult[] SearchIssues(string searchStr)
        {
            var response = Request(new SearchIssuesRequest(_repo, searchStr));
            return (response.Results ?? new List<SearchIssuesResult>()).ToArray();
        }

        /// <summary>
        ///     Creates a new issue.
        /// </summary>
        /// <param name="title">
        ///     The title of the issue.
        /// </param>
        /// <param name="body">
        ///     The issue's body text in Markdown format.
        /// </param>
        /// <returns>
        ///     The response returned by GitHub.
        /// </returns>
        /// <exception cref="WebException">
        ///     Thrown if no response is received.
        /// </exception>
        /// <exception cref="RequestException">
        ///     Thrown if the GitHub API is unable to complete the request and responds with an error message.
        /// </exception>
        [NotNull]
        public CreateIssueResponse CreateIssue(string title, string body)
        {
            return Request(new CreateIssueRequest(_repo, title, body));
        }

        /// <summary>
        ///     Creates a new comment on an existing issue.
        /// </summary>
        /// <param name="issue">
        ///     The existing issue to create a comment for.
        /// </param>
        /// <param name="body">
        ///     The comment's body in Markdown format.
        /// </param>
        /// <returns>
        ///     A list of search results that match the given <paramref name="exception"/>.
        /// </returns>
        /// <exception cref="WebException">
        ///     Thrown if no response is received.
        /// </exception>
        /// <exception cref="RequestException">
        ///     Thrown if the GitHub API is unable to complete the request and responds with an error message.
        /// </exception>
        [NotNull]
        public CreateIssueCommentResponse CreateIssueComment(SearchIssuesResult issue, string body)
        {
            return Request(new CreateIssueCommentRequest(_repo, issue.Number, body));
        }

        #endregion

        #region Private implementation

        /// <exception cref="WebException">
        ///     Thrown if no response is received.
        /// </exception>
        /// <exception cref="RequestException">
        ///     Thrown if the GitHub API is unable to complete the request and responds with an error message.
        /// </exception>
        [NotNull]
        private TResponse Request<TResponse>(IGitHubRequest<TResponse> request)
            where TResponse : new()
        {
            var headers = new List<string>
                          {
                              string.Format("Authorization: token {0}", _oauthToken)
                          };

            var httpRequest = HttpRequest.BuildRequest(request.Method, request.Url, false, headers);

            // Expected response format
            httpRequest.Accept = "application/vnd.github.v3+json";

            if (request.Method == HttpRequestMethod.Put || request.Method == HttpRequestMethod.Post)
            {
                // Request body format
                httpRequest.ContentType = "application/json";

                using (var requestStream = httpRequest.GetRequestStream())
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    var json = SmartJsonConvert.SerializeObject(request);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            try
            {
                using (var httpResponse = httpRequest.GetResponse())
                {
                    return GetResponse<TResponse>(httpResponse);
                }
            }
            catch (WebException ex)
            {
                var errorResponse = GetResponse<ErrorResponse>(ex.Response);

                Logger.Error(errorResponse.ToString(), ex);

                throw new RequestException(ex, errorResponse);
            }
        }

        /// <exception cref="WebException">
        ///     Thrown if <paramref name="httpResponse"/> is <c>null</c> or <see cref="WebResponse.GetResponseStream"/> returns <c>null</c>.
        /// </exception>
        private static TResponse GetResponse<TResponse>(WebResponse httpResponse)
        {
            if (httpResponse == null)
            {
                throw new WebException("No response received");
            }
            using (var responseStream = httpResponse.GetResponseStream())
            {
                if (responseStream == null)
                {
                    throw new WebException("Response stream is null");
                }
                using (var streamReader = new StreamReader(responseStream))
                {
                    var responseText = streamReader.ReadToEnd();
                    var response = SmartJsonConvert.DeserializeObject<TResponse>(responseText);
                    return response;
                }
            }
        }

        #endregion
    }
}
