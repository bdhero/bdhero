using System.Drawing;
using DotNetUtils.Attributes;
using DotNetUtils.Extensions;

namespace BDHero.BDROM
{
    /// <summary>
    ///     Standard dimensions of jacket images (found in the <c>BDMV/META/DL</c> directory).
    /// </summary>
    public enum JacketSize
    {
        /// <summary>
        ///     416 x 240.
        /// </summary>
        [Size(416, 240)]
        Small,

        /// <summary>
        ///     640 x 360.
        /// </summary>
        [Size(640, 360)]
        Large
    }

    public static class JacketSizeExtensions
    {
        public static Size GetDimensions(this JacketSize jacketSize)
        {
            return jacketSize.GetAttributeProperty<SizeAttribute, Size>(attribute => attribute.Size);
        }
    }
}
