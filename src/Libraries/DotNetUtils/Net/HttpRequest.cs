// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.Caching;
using DotNetUtils.Annotations;

namespace DotNetUtils.Net
{
    /// <summary>
    ///     Invoked just before an HTTP request is made, allowing observers to modify the request before it gets sent.
    ///     This can be useful to override the system's default proxy settings, set timeout values, etc.
    /// </summary>
    /// <param name="request"></param>
    public delegate void BeforeRequestEventHandler(HttpWebRequest request);

    /// <summary>
    ///     Helper utility class for making HTTP requests and parsing/caching the responses.
    /// </summary>
    public static class HttpRequest
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Gets or sets the User-Agent HTTP request header sent to the web server when making requests.
        ///     Defaults to <c>"ENTRY_ASSEMBLY_NAME/ENTRY_ASSEMBLY_VERSION"</c>.
        /// </summary>
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static string UserAgent = AssemblyUtils.GetAssemblyName() + "/" + AssemblyUtils.GetAssemblyVersion();

        private static readonly MemoryCache ImageCache = MemoryCache.Default;

        /// <summary>
        ///     Invoked before every HTTP request made by <strong>this class only</strong>.
        ///     Applies to requests sent with the following methods:
        ///     <list type=">">
        ///         <item>
        ///             <see cref="Get(string,System.Collections.Generic.List{string})" />
        ///         </item>
        ///         <item>
        ///             <see cref="Get(System.Net.HttpWebRequest)" />
        ///         </item>
        ///         <item>
        ///             <see cref="Put" />
        ///         </item>
        ///         <item>
        ///             <see cref="Post" />
        ///         </item>
        ///         <item>
        ///             <see cref="GetImage" />
        ///         </item>
        ///     </list>
        /// </summary>
        public static event BeforeRequestEventHandler BeforeRequestGlobal;

        /// <summary>
        ///     Performs a synchronous HTTP GET request and returns the full response as a string.
        /// </summary>
        /// <param name="uri">URI of the web resource to GET</param>
        /// <param name="headers">Optional list of fully-formatted headers of the form <c>Header-Name: Header-Value</c></param>
        /// <returns>Response body as a string</returns>
        public static string Get(string uri, List<string> headers = null)
        {
            return Get(BuildRequest(HttpRequestMethod.Get, uri, false, headers));
        }

        /// <summary>
        ///     Performs a synchronous HTTP GET request and returns the full response as a string.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response body as a string</returns>
        public static string Get(HttpWebRequest request)
        {
            NotifyBeforeRequest(request);
            using (var httpResponse = request.GetResponse())
            {
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        return null;
                    }
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        ///     Performs a synchronous HTTP POST request and returns the full response as a string.
        /// </summary>
        /// <param name="uri">URI of the web resource to POST to</param>
        /// <param name="formData">Optional form data to send to the server in the request body</param>
        /// <returns></returns>
        public static string Post(string uri, IDictionary<string, string> formData = null)
        {
            return PutOrPost(HttpRequestMethod.Post, uri, formData);
        }

        /// <summary>
        ///     Performs a synchronous HTTP PUT request and returns the full response as a string.
        /// </summary>
        /// <param name="uri">URI of the web resource to POST to</param>
        /// <param name="formData">Optional form data to send to the server in the request body</param>
        /// <returns></returns>
        public static string Put(string uri, IDictionary<string, string> formData = null)
        {
            return PutOrPost(HttpRequestMethod.Put, uri, formData);
        }

        /// <summary>
        ///     Builds and configures an <see cref="HttpWebRequest"/> object with the given parameters.
        /// </summary>
        /// <param name="method">HTTP verb to use for the request.</param>
        /// <param name="uri">Address of a local or remote HTTP server.</param>
        /// <param name="cache">Determines whether the request should be cached.</param>
        /// <param name="headers">HTTP request headers to send to the server.</param>
        /// <returns>A fully configured <see cref="HttpWebRequest"/> object.</returns>
        public static HttpWebRequest BuildRequest(HttpRequestMethod method, string uri, bool cache = false, List<string> headers = null)
        {
            LogBuildRequest(method, uri, cache, headers);

            var request = (HttpWebRequest) WebRequest.Create(uri);

            request.Method = method.ToString("G").ToUpper();
            request.UserAgent = UserAgent;
            request.KeepAlive = true;
            request.CachePolicy = new RequestCachePolicy(cache ? RequestCacheLevel.CacheIfAvailable : RequestCacheLevel.BypassCache);
            request.Expect = null;

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header);
                }
            }

            if (HttpRequestMethod.Post == method || HttpRequestMethod.Put == method)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            return request;
        }

        private static void LogBuildRequest(HttpRequestMethod method, string uri, bool cache = false, List<string> headers = null)
        {
            var strHeaders = "";
            if (headers != null && headers.Count > 0)
            {
                strHeaders = string.Format(" with headers: {0}", string.Join("; ", headers));
            }
            Logger.DebugFormat("{0} \"{1}\" -- cache = {2}{3}", method.ToString("G").ToUpper(), uri, cache, strHeaders);
        }

        [CanBeNull]
        private static string PutOrPost(HttpRequestMethod method, string uri, IDictionary<string, string> formData)
        {
            var request = BuildRequest(method, uri);

            using (var requestStream = request.GetRequestStream())
            {
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    var body = new List<string>();
                    if (formData != null)
                    {
                        body.AddRange(formData.Keys.Select(key => EncodeForPostBody(key, formData[key])));
                    }
                    streamWriter.Write(string.Join("&", body) + "\n");
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            NotifyBeforeRequest(request);

            using (var httpResponse = request.GetResponse())
            {
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        return null;
                    }
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        var responseText = streamReader.ReadToEnd();
                        return responseText;
                    }
                }
            }
        }

        private static string EncodeForPostBody(string key, string value)
        {
            return Uri.EscapeUriString(key) + "=" + Uri.EscapeUriString(value);
        }

        private static void NotifyBeforeRequest(HttpWebRequest request)
        {
            if (BeforeRequestGlobal != null)
            {
                BeforeRequestGlobal(request);
            }
        }

        #region Images

        /// <summary>
        ///     Performs a synchronous HTTP GET request and returns the full response as an Image.
        /// </summary>
        /// <param name="uri">URI of the web resource to GET</param>
        /// <param name="cache">
        ///     Determines whether a successful response should be cached to speed up future requests for the same
        ///     <paramref name="uri" />
        /// </param>
        /// <returns></returns>
        [CanBeNull]
        public static Image GetImage(string uri, bool cache = true)
        {
            return cache ? GetImageCached(uri) : GetImageNoCache(uri);
        }

        [CanBeNull]
        private static Image GetImageNoCache(string uri)
        {
            var request = BuildRequest(HttpRequestMethod.Get, uri, true);
            NotifyBeforeRequest(request);
            using (var httpResponse = request.GetResponse())
            {
                using (var stream = httpResponse.GetResponseStream())
                {
                    return stream != null ? Image.FromStream(stream) : null;
                }
            }
        }

        [CanBeNull]
        private static Image GetImageCached([NotNull] string url)
        {
            return ImageCache.Contains(url) ? ImageCache[url] as Image : FetchAndCacheImage(url);
        }

        [CanBeNull]
        private static Image FetchAndCacheImage([NotNull] string url)
        {
            var image = GetImageNoCache(url);
            if (image == null)
            {
                return null;
            }
            ImageCache.Set(url, image, new CacheItemPolicy());
            return image;
        }

        #endregion
    }
}
