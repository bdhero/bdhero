using System.Diagnostics;
using DotNetUtils.Annotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Global
namespace GitHub.Models
{
    /// <summary>
    ///     Represents a user (person) that has a GitHub login.
    /// </summary>
    [UsedImplicitly]
    [DebuggerDisplay("{Login}")]
    public class User
    {
        /// <summary>
        ///     Username.
        /// </summary>
        public string Login;

        /// <summary>
        ///     The user's unique ID.
        /// </summary>
        public int Id;

        /// <summary>
        ///     URL of the user's avatar image.
        /// </summary>
        public string AvatarUrl;

        /// <summary>
        ///     Unique Gravatar ID of the user's avatar image.
        /// </summary>
        public string GravatarId;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for the user.
        /// </summary>
        public string Url;

        /// <summary>
        ///     URL of a human-friendly HTML-formatted overview page for the user.
        /// </summary>
        public string HtmlUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of users that are following this user.
        /// </summary>
        public string FollowersUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of users that this user is following.
        /// </summary>
        public string FollowingUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of this user's Gists.
        /// </summary>
        public string GistsUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of users and repos that this user has starred.
        /// </summary>
        public string StarredUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of this user's subscriptions.
        /// </summary>
        public string SubscriptionsUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of organizations this user belongs to.
        /// </summary>
        public string OrganizationsUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of this user's repositories.
        /// </summary>
        public string ReposUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of this user's events.
        /// </summary>
        public string EventsUrl;

        /// <summary>
        ///     GitHub API <c>GET</c> URL for a list of this user's received events.
        /// </summary>
        public string ReceivedEventsUrl;

        /// <summary>
        ///     User type (<c>"User"</c>).
        /// </summary>
        public string Type;

        /// <summary>
        ///     Determines whether this user is a GitHub admin or not.
        /// </summary>
        public bool SiteAdmin;
    }
}