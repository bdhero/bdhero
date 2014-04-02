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
using System.Linq;
using BDHero.Startup;
using DotNetUtils.Annotations;
using GitHub;

namespace BDHero.ErrorReporting
{
    public static class ErrorReporter
    {
        private const string DevRepo = "acdvorak/bdhero-private";
        private const string ProdRepo = "bdhero/bdhero";

        [NotNull]
        public static IErrorReportResult Report(ErrorReport report, AppConfig appConfig)
        {
//            var x = new {foo = 123};

            try
            {
                return ReportImpl(report, appConfig);
            }
            catch (Exception ex)
            {
                return new ErrorReportResultFailed(ex);
            }
        }

        private static IErrorReportResult ReportImpl(ErrorReport report, AppConfig appConfig)
        {
            var repo = appConfig.IsDebugMode ? DevRepo : ProdRepo;
            var client = new GitHubClient(repo, "131765cc986bd5fa6d09d8633c4d973fbe6dfcf9");
            var issues = client.SearchIssues(report.ExceptionDetailRedacted);

            if (issues.Any())
            {
                var issue = issues.First();
                var comment = client.CreateIssueComment(issue, report.Body);
                return new ErrorReportResultUpdated(issue, comment);
            }
            else
            {
                var issue = client.CreateIssue(report.Title, report.Body);
                return new ErrorReportResultCreated(issue);
            }
        }
    }
}
