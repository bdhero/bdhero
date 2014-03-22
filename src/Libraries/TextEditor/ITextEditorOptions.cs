namespace TextEditor
{
    public interface ITextEditorOptions
    {
        #region Line numbers

        bool ShowLineNumbers { get; set; }

        #endregion

        #region Column ruler

        bool ShowColumnRuler { get; set; }

        int ColumnRulerPosition { get; set; }

        bool CutCopyWholeLine { get; set; }

        #endregion

        #region Indentation

        int IndentationSize { get; set; }

        bool ConvertTabsToSpaces { get; set; }

        #endregion

        #region Character visualization

        bool ShowSpaces { get; set; }

        bool ShowTabs { get; set; }

        bool ShowWhiteSpace { get; set; }

        #endregion

        #region Word wrapping

        bool SupportsWordWrap { get; }

        bool WordWrap { get; set; }

        double WordWrapIndent { get; set; }

        #endregion
    }
}
