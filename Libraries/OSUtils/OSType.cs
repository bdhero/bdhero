namespace OSUtils
{
    public enum OSType
    {
        /// <summary>
        /// Any version of Windows supported by .NET 4.0, from Windows XP to Windows 8.1 and beyond.
        /// </summary>
        Windows,

        /// <summary>
        /// Any version of Mac OS (a.k.a. Darwin) supported by Mono 3.2.
        /// </summary>
        Mac,

        /// <summary>
        /// Any version of Linux supported by Mono 3.2.
        /// </summary>
        Linux,

        /// <summary>
        /// Any version of UNIX (other than Linux or Mac) supported by Mono 3.2.
        /// </summary>
        Unix,

        /// <summary>
        /// Any other operating system not specified in this enum.
        /// </summary>
        Other
    }
}