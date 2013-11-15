using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.BDROM
{
    /// <summary>
    /// Describes various optional Blu-ray features present on the disc.
    /// </summary>
    public class DiscFeatures
    {
        /// <summary>
        /// Disc is protected with BD+ encryption.
        /// </summary>
        public bool IsBDPlus;

        /// <summary>
        /// Disc contains Java menus.
        /// </summary>
        public bool IsBDJava;

        /// <summary>
        /// Disc contains 3D video.
        /// </summary>
        public bool Is3D;

        /// <summary>
        /// Disc contains D-BOX motion code.
        /// </summary>
        public bool IsDbox;

        /// <summary>
        /// Disc contains a copy-protected "Digital Copy" of the film that users can download
        /// and watch on their computers and portable devices.
        /// </summary>
        public bool IsDCopy;

        /// <summary>
        /// Disc contains a PSP-ready "Digital Copy" of the film that users can download
        /// and play on their PSPs.
        /// </summary>
        public bool IsPSP;
    }
}
