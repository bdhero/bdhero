using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class StreamClipTransformer
    {
        public static List<StreamClip> Transform(IEnumerable<TSStreamClip> tsStreamClips)
        {
            // TODO: Add angle support!
            return tsStreamClips.Where(IsDefaultAngle).Where(StreamFileExists).Select(Transform).ToList();
        }

        private static bool IsDefaultAngle(TSStreamClip tsStreamClip)
        {
            return tsStreamClip.AngleIndex == 0;
        }

        /// <summary>
        /// Guards against a bug in BDInfo where a non-existent stream clip (00121.m2ts) gets created with a <c>null</c> <see cref="TSStreamClip.StreamFile"/>
        /// when scanning CASINO_ROYAL.
        /// </summary>
        private static bool StreamFileExists(TSStreamClip tsStreamClip)
        {
            return tsStreamClip.StreamFile != null;
        }

        private static StreamClip Transform(TSStreamClip tsStreamClip, int index)
        {
            Debug.Assert(tsStreamClip.StreamFile != null, "tsStreamClip.StreamFile is null", tsStreamClip.Name);

            return new StreamClip(tsStreamClip.StreamFile.FileInfo, tsStreamClip.Name, tsStreamClip.FileSize, index, tsStreamClip.AngleIndex, tsStreamClip.Length);
        }
    }
}
