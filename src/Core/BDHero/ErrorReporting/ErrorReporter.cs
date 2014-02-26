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
using DotNetUtils.Annotations;
using GitHub;

namespace BDHero.ErrorReporting
{
    public static class ErrorReporter
    {
        private static readonly GitHubClient Client = new GitHubClient("acdvorak/bdhero-private", "131765cc986bd5fa6d09d8633c4d973fbe6dfcf9");

        [NotNull]
        public static IErrorReportResult Report(ErrorReport report)
        {
//            var x = new {foo = 123};

            try
            {
                return ReportImpl(report);
            }
            catch (Exception ex)
            {
                return new ErrorReportResultFailed(ex);
            }
        }

        private static IErrorReportResult ReportImpl(ErrorReport report)
        {
            var issues = Client.SearchIssues(report.ExceptionDetailRedacted);

            if (issues.Any())
            {
                var issue = issues.First();
                var comment = Client.CreateIssueComment(issue, report.Body);
                return new ErrorReportResultUpdated(issue, comment);
            }
            else
            {
                var issue = Client.CreateIssue(report.Title, report.Body);
                return new ErrorReportResultCreated(issue);
            }
        }
    }
}
