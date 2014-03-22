using System.Windows.Forms;
using BDHero.ErrorReporting;
using UILib.WinForms.Forms;

namespace BDHeroGUI.Dialogs
{
    public class GenericErrorDialog : IErrorDialog
    {
        private readonly ErrorReport _report;

        public string Title { get; set; }
        
        public string Heading { get; set; }
        
        public string Message { get; set; }
        
        public string StackTrace { get; set; }

        public GenericErrorDialog(ErrorReport report)
        {
            _report = report;
        }

        public void AddResultVisitor(IErrorReportResultVisitor visitor)
        {
        }

        public void RemoveResultVisitor(IErrorReportResultVisitor visitor)
        {
        }

        public void ShowReportable(IWin32Window owner = null)
        {
            ShowNonReportable(owner);
        }

        public void ShowNonReportable(IWin32Window owner = null)
        {
             DetailForm.ShowExceptionDetail(owner, Title, _report.ExceptionMessageRaw, _report.ExceptionDetailRaw);
        }
    }
}
