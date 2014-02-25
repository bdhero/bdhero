using GitHub.Models;

namespace BDHero.ErrorReporting
{
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
}