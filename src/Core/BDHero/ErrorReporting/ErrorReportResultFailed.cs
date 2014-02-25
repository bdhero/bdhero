using System;

namespace BDHero.ErrorReporting
{
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