using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDInfo
{
    /// <summary>
    /// Constants (ported over from BDHeroSettings)
    /// </summary>
    static class BDInfoSettings
    {
        /// <summary>
        /// Enable 3D Blu-ray support (incomplete and untested).
        /// </summary>
        public const bool EnableSSIF = false;
        public const bool KeepStreamOrder = true;
        public const bool FilterShortPlaylists = true;
        public const int FilterShortPlaylistsValue = 20;
        public const bool FilterLoopingPlaylists = true;
    }
}
