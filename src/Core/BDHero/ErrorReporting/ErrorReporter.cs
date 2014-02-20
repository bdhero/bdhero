// Copyright 2014 Andrew C. Dvorak
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
using System.IO;
using BDHero.ErrorReporting.Models;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Net;
using Newtonsoft.Json;

namespace BDHero.ErrorReporting
{
    public class ErrorReporter
    {
        // TODO: Make this configurable?
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
                var json = SmartJsonConvert.SerializeObject(newIssueRequest);
                streamWriter.Write(json);
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
                    var newIssueResponse = SmartJsonConvert.DeserializeObject<NewIssueResponse>(responseText);
                    return newIssueResponse;
                }
            }
        }
    }
}
