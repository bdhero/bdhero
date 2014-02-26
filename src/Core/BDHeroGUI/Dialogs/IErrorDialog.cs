using System.Windows.Forms;
using BDHero.ErrorReporting;

namespace BDHeroGUI.Dialogs
{
    internal interface IErrorDialog
    {
        string Title { get; set; }

        string Heading { get; set; }
        
        string Message { get; set; }

        string StackTrace { get; set; }

        void AddResultVisitor(IErrorReportResultVisitor visitor);

        void RemoveResultVisitor(IErrorReportResultVisitor visitor);

        /// <summary>
        ///     <para>
        ///         Displays the error dialog as a modal window of the given <paramref name="owner"/>,
        ///         or as an independent modeless dialog if <paramref name="owner"/> is <c>null</c>.
        ///     </para>
        ///     <para>
        ///         The user will be given the option of submitting an error report.
        ///     </para>
        /// </summary>
        /// <param name="owner">
        ///     Optional parent window for this dialog.  If <c>null</c>, the dialog will be displayed modeless.
        /// </param>
        void ShowReportable(IWin32Window owner = null);

        /// <summary>
        ///     <para>
        ///         Displays the error dialog as a modal window of the given <paramref name="owner"/>,
        ///         or as an independent modeless dialog if <paramref name="owner"/> is <c>null</c>.
        ///     </para>
        ///     <para>
        ///         The user will not be given the option of submitting an error report.
        ///     </para>
        /// </summary>
        /// <param name="owner">
        ///     Optional parent window for this dialog.  If <c>null</c>, the dialog will be displayed modeless.
        /// </param>
        void ShowNonReportable(IWin32Window owner = null);
    }
}
