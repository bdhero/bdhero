namespace BDHero.ErrorReporting
{
    public interface IErrorReportResult
    {
        void Accept(IErrorReportResultVisitor visitor);
    }
}