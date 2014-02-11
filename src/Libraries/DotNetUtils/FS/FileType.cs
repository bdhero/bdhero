using System.Linq;
using System.Windows.Forms;

namespace DotNetUtils.FS
{
    /// <summary>
    ///     Represents a set of logically-related file extensions that form a single file type.
    /// </summary>
    public struct FileType
    {
        /// <summary>
        ///     Gets or sets a list of file extensions represented by this set.
        /// </summary>
        /// <example>
        ///     <c>{ ".mkv", ".mka", "mks" }</c>
        /// </example>
        public string[] Extensions;

        /// <summary>
        ///     Gets or sets a brief description of the file type.
        /// </summary>
        /// <example>
        ///     <c>"Matroska container file"</c>
        /// </example>
        public string Description;

        /// <summary>
        ///     Returns a string that can be assigned to <see cref="FileDialog.Filter"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var exts =
                FileUtils.NormalizeFileExtensions(Extensions)
                         .Select(ext => string.Format("*{0}", ext))
                         .ToArray();
            return string.Format("{0} ({1})|{2}", Description, string.Join("; ", exts), string.Join(";", exts));
        }
    }
}