using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Net;
using GitHub.Models;

namespace GitHub
{
    /// <summary>
    ///     REST client for accessing the GitHub public API.
    /// </summary>
    public class GitHubClient
    {
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
        ///     Creates a new issue.
        /// </summary>
        /// <param name="exception">
        ///     An exception to report in the issue.
        /// </param>
        /// <returns>
        ///     The response returned by GitHub.
        /// </returns>
        /// <exception cref="WebException">
        ///     Thrown if <see cref="WebResponse.GetResponseStream"/> returns <c>null</c>.
        /// </exception>
        [NotNull]
        public CreateIssueResponse CreateIssue(Exception exception)
        {
            return Request(new CreateIssueRequest(_repo, exception));
        }

        /// <summary>
        ///     Searches existing issues for the given <paramref name="exception"/> and returns a list of results.
        /// </summary>
        /// <param name="exception">
        ///     The exception being searched for.
        /// </param>
        /// <returns>
        ///     A list of search results that match the given <paramref name="exception"/>.
        /// </returns>
        /// <exception cref="WebException">
        ///     Thrown if <see cref="WebResponse.GetResponseStream"/> returns <c>null</c>.
        /// </exception>
        [NotNull]
        public SearchIssuesResult[] SearchIssues(Exception exception)
        {
            var response = Request(new SearchIssuesRequest(exception, _repo));
            return (response.Results ?? new List<SearchIssuesResult>()).ToArray();
        }

        /// <summary>
        ///     Creates a new comment on an existing issue.
        /// </summary>
        /// <param name="issue">
        ///     The existing issue to create a comment for.
        /// </param>
        /// <param name="exception">
        ///     An exception to report in the comment.
        /// </param>
        /// <returns>
        ///     A list of search results that match the given <paramref name="exception"/>.
        /// </returns>
        /// <exception cref="WebException">
        ///     Thrown if <see cref="WebResponse.GetResponseStream"/> returns <c>null</c>.
        /// </exception>
        [NotNull]
        public CreateIssueCommentResponse CreateIssueComment(SearchIssuesResult issue, Exception exception)
        {
            return Request(new CreateIssueCommentRequest(_repo, issue.Number, exception));
        }

        #endregion

        #region Private implementation

        /// <exception cref="WebException">
        ///     Thrown if <see cref="WebResponse.GetResponseStream"/> returns <c>null</c>.
        /// </exception>
        [NotNull]
        private TResponse Request<TResponse>(IGitHubRequest<TResponse> requestBody)
            where TResponse : new()
        {
            var headers = new List<string>
                          {
                              string.Format("Authorization: token {0}", _oauthToken)
                          };

            var request = HttpRequest.BuildRequest(requestBody.Method, requestBody.Url, false, headers);

            // Expected response format
            request.Accept = "application/vnd.github.v3+json";

            if (requestBody.Method == HttpRequestMethod.Put || requestBody.Method == HttpRequestMethod.Post)
            {
                // Request body format
                request.ContentType = "application/json";

                using (var requestStream = request.GetRequestStream())
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    var json = SmartJsonConvert.SerializeObject(requestBody);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            using (var httpResponse = request.GetResponse())
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
