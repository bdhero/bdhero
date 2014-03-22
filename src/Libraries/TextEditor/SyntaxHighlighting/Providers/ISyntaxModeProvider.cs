using System.Collections.Generic;
using System.Xml;

// ReSharper disable ReturnTypeCanBeEnumerable.Global
namespace TextEditor.SyntaxHighlighting.Providers
{
    /// <summary>
    ///     Interface for a class that reads syntax highlighting definition (<c>.XSHD</c>) files.
    /// </summary>
    public interface ISyntaxModeProvider
    {
        /// <summary>
        ///     Gets a collection of all syntax modes currently loaded by the provider.
        /// </summary>
        ICollection<MySyntaxMode> SyntaxModes { get; }

        /// <summary>
        ///     Retrieves an XML text reader for the specified <paramref name="syntaxMode"/>.
        /// </summary>
        /// <param name="syntaxMode">
        ///     Syntax mode to read.
        /// </param>
        /// <returns>
        ///     XML reader with the requested <paramref name="syntaxMode"/> file loaded.
        /// </returns>
        XmlTextReader GetSyntaxModeFile(MySyntaxMode syntaxMode);
    }
}