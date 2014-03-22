namespace TextEditor.SyntaxHighlighting
{
    public class MySyntaxMode
    {
        public readonly string FileName;
        public readonly string Name;
        public readonly string[] Extensions;

        public MySyntaxMode(string fileName, string name, string extensions)
        {
            FileName = fileName;
            Name = name;
            Extensions = extensions.Split(';');
        }

        public MySyntaxMode(string fileName, string name, params string[] extensions)
        {
            FileName = fileName;
            Name = name;
            Extensions = extensions;
        }
    }
}