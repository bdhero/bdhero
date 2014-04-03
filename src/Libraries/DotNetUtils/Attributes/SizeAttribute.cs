using System;
using System.Drawing;

namespace DotNetUtils.Attributes
{
    /// <summary>
    ///     Specifies the dimensions represented by an enum member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SizeAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets the size value.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SizeAttribute"/> class with the specified
        ///     <paramref name="width"/> and <paramref name="height"/>.
        /// </summary>
        /// <param name="width">Width represented by the enum member (typically in pixels).</param>
        /// <param name="height">Height represented by the enum member (typically in pixels).</param>
        public SizeAttribute(int width, int height)
        {
            Size = new Size(width, height);
        }
    }
}
