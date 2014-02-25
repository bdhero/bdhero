using GitHub.Models;

namespace BDHero.ErrorReporting
{
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
}