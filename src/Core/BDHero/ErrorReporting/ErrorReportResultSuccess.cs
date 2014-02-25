namespace BDHero.ErrorReporting
{
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
}