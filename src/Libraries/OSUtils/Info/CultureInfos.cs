using System.Globalization;
using DotNetUtils;
using DotNetUtils.Annotations;

namespace OSUtils.Info
{
    public class CultureInfos
    {
        /// <summary>
        /// Gets the <see cref="CultureInfo"/> that represents the culture installed with the operating system.
        /// </summary>
        [UsedImplicitly]
        public readonly CultureInfo InstalledUICulture;

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> that represents the culture used by the current thread.
        /// </summary>
        [UsedImplicitly]
        public readonly CultureInfo CurrentCulture;

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> that represents the current culture used by the Resource Manager
        /// to look up culture-specific resources at run time.
        /// </summary>
        [UsedImplicitly]
        public readonly CultureInfo CurrentUICulture;

        public CultureInfos()
        {
            InstalledUICulture = CultureInfo.InstalledUICulture;
            CurrentCulture     = CultureInfo.CurrentCulture;
            CurrentUICulture   = CultureInfo.CurrentUICulture;
        }

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
}