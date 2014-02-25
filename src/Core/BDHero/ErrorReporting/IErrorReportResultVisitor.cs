namespace BDHero.ErrorReporting
{
    public interface IErrorReportResultVisitor
    {
        void Visit(ErrorReportResultCreated result);

        void Visit(ErrorReportResultUpdated result);

        void Visit(ErrorReportResultFailed result);
    }
}