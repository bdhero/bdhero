using DotNetUtils.Net;
using Newtonsoft.Json;

namespace GitHub.Models
{
    internal interface IGitHubRequest<TResponse> where TResponse : new()
    {
        [JsonIgnore]
        HttpRequestMethod Method { get; }

        [JsonIgnore]
        string Url { get; }
    }
}
