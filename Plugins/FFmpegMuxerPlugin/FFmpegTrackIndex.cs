namespace BDHero.Plugin.FFmpegMuxer
{
    internal class FFmpegTrackIndex
    {
        /// <summary>
        /// The absolute position (index) of the track in the input MPLS, starting at 0.
        /// </summary>
        public int InputIndex { get; set; }

        /// <summary>
        /// The index of the track relative to other tracks of the same type (e.g., audio, video, subtitle) in the input MPLS, starting at 0.
        /// </summary>
        public int InputIndexOfType { get; set; }

        /// <summary>
        /// The absolute position (index) of the track in the output MKV, starting at 0.
        /// </summary>
        public int OutputIndex { get; set; }

        /// <summary>
        /// The index of the track relative to other tracks of the same type (e.g., audio, video, subtitle) in the output MKV, starting at 0.
        /// </summary>
        public int OutputIndexOfType { get; set; }
    }
}