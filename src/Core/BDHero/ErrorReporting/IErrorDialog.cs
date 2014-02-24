using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BDHero.ErrorReporting
{
    internal interface IErrorDialog
    {
        string Title { get; set; }

        string Heading { get; set; }
        
        string Message { get; set; }

        string StackTrace { get; set; }

        void ShowNonReportable(IWin32Window owner = null);

        void ShowReportable(string reportTitle, string reportBody);

        void ShowReportable(IWin32Window owner, string reportTitle, string reportBody);
    }
}
