using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;
using DotNetUtils.Net;
using Newtonsoft.Json;

namespace BDHero
{
    public class ErrorReporter
    {
//        private const string Url = "https://api.github.com/repos/bdhero/bdhero/issues";
        private const string Url = "https://api.github.com/repos/acdvorak/bdhero-private/issues";

        [CanBeNull]
        public static NewIssueResponse Report(Exception exception)
        {
            var headers = new List<string>
                          {
                              "Authorization: token 131765cc986bd5fa6d09d8633c4d973fbe6dfcf9"
                          };

            var newIssueRequest = new NewIssueRequest(exception);

            var request = HttpRequest.BuildRequest(HttpRequestMethod.Post, Url, false, headers);

            request.Accept = "application/vnd.github.v3+json";
            request.ContentType = "application/json";

            using (var requestStream = request.GetRequestStream())
            using (var streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(JsonConvert.SerializeObject(newIssueRequest));
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var httpResponse = request.GetResponse())
            using (var responseStream = httpResponse.GetResponseStream())
            {
                if (responseStream == null)
                {
                    return null;
                }
                using (var streamReader = new StreamReader(responseStream))
                {
                    var responseText = streamReader.ReadToEnd();
                    var newIssueResponse = JsonConvert.DeserializeObject<NewIssueResponse>(responseText);
                    return newIssueResponse;
                }
            }
        }
    }

    public class NewIssueRequest
    {
        [JsonProperty("title")]
        public string Title;
        
        [JsonProperty("body")]
        public string Body;
        
        [JsonProperty("assignee")]
        public string Assignee = "acdvorak";

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        public NewIssueRequest(Exception exception)
        {
            var indent = "    ";
            var stackTrace = string.Join("\n", exception.ToString().Split('\n').Select(line => indent + line));
            Title = string.Format("Exception: {0} ({1} v{2})", exception.Message, AppUtils.AppName, AppUtils.AppVersion);
            Body = string.Format("{0} v{1}:\n\n{2}", AppUtils.AppName, AppUtils.AppVersion, stackTrace);
            Labels = new List<string> { "report" };
        }
    }

    [UsedImplicitly]
    public class NewIssueResponse
    {

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("labels_url")]
        public string LabelsUrl { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("labels")]
        public List<object> Labels { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("assignee")]
        public object Assignee { get; set; }

        [JsonProperty("milestone")]
        public object Milestone { get; set; }

        [JsonProperty("comments")]
        public int Comments { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("closed_at")]
        public object ClosedAt { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("closed_by")]
        public object ClosedBy { get; set; }
    }

    [UsedImplicitly]
    public class User
    {

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("followers_url")]
        public string FollowersUrl { get; set; }

        [JsonProperty("following_url")]
        public string FollowingUrl { get; set; }

        [JsonProperty("gists_url")]
        public string GistsUrl { get; set; }

        [JsonProperty("starred_url")]
        public string StarredUrl { get; set; }

        [JsonProperty("subscriptions_url")]
        public string SubscriptionsUrl { get; set; }

        [JsonProperty("organizations_url")]
        public string OrganizationsUrl { get; set; }

        [JsonProperty("repos_url")]
        public string ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }

        [JsonProperty("received_events_url")]
        public string ReceivedEventsUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("site_admin")]
        public bool SiteAdmin { get; set; }
    }
}
