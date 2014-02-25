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
using GitHub.Models;

namespace BDHero.ErrorReporting
{
    public static class ErrorReporter
    {
        private static readonly GitHubClient Client = new GitHubClient("acdvorak/bdhero-private", "131765cc986bd5fa6d09d8633c4d973fbe6dfcf9");

        [NotNull]
        public static IErrorReportResult Report(Exception exception)
        {
//            var x = new {foo = 123};

            try
            {
                return ReportImpl(exception);
            }
            catch (Exception ex)
            {
                return new ErrorReportResultFailed(ex);
            }
        }

        private static IErrorReportResult ReportImpl(Exception exception)
        {
            var issues = Client.SearchIssues(exception);

            if (issues.Any())
            {
                var issue = issues.First();
                var comment = Client.CreateIssueComment(issue, exception);
                return new ErrorReportResultUpdated(issue, comment);
            }
            else
            {
                var issue = Client.CreateIssue(exception);
                return new ErrorReportResultCreated(issue);
            }
        }
    }

    public interface IErrorReportResultVisitor
    {
        void Visit(ErrorReportResultCreated result);

        void Visit(ErrorReportResultUpdated result);

        void Visit(ErrorReportResultFailed result);
    }

    public interface IErrorReportResult
    {
        void Accept(IErrorReportResultVisitor visitor);
    }

    public abstract class ErrorReportResultSuccess : IErrorReportResult
    {
        public readonly int IssueNumber;

        public readonly string Url;

        protected ErrorReportResultSuccess(int issueNumber, string url)
        {
            IssueNumber = issueNumber;
            Url = url;
        }

        public abstract void Accept(IErrorReportResultVisitor visitor);
    }

    public class ErrorReportResultCreated : ErrorReportResultSuccess
    {
        public ErrorReportResultCreated(CreateIssueResponse issue)
            : base(issue.Number, issue.HtmlUrl)
        {
        }

        public override void Accept(IErrorReportResultVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ErrorReportResultUpdated : ErrorReportResultSuccess
    {
        public ErrorReportResultUpdated(SearchIssuesResult issue, CreateIssueCommentResponse comment)
            : base(issue.Number, comment.HtmlUrl)
        {
        }

        public override void Accept(IErrorReportResultVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ErrorReportResultFailed : IErrorReportResult
    {
        public readonly Exception Exception;

        public ErrorReportResultFailed(Exception exception)
        {
            Exception = exception;
        }

        public void Accept(IErrorReportResultVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
