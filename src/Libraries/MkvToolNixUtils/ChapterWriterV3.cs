// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BDHero.BDROM;
using DotNetUtils.FS;
using I18N;
using ProcessUtils;

namespace MkvToolNixUtils
{
    public class ChapterWriterV3
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        private readonly ITempFileRegistrar _tempFileRegistrar;

        public ChapterWriterV3(ITempFileRegistrar tempFileRegistrar)
        {
            _tempFileRegistrar = tempFileRegistrar;
        }

        /// <summary>
        ///     Saves the given <paramref name="chapters"/> in Matroska XML format to a temporary file
        ///     and adds the necessary command line arguments to <paramref name="arguments"/> to
        ///     add the <paramref name="chapters"/> to an MKV file using any of the supported MkvToolNix binaries.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="chapters"></param>
        public void SetChapters(ArgumentList arguments, ICollection<Chapter> chapters)
        {
            var path = _tempFileRegistrar.CreateTempFile(GetType(), "chapters.xml");

            SaveAsXml(chapters, path);

            var firstChapterWithLanguage = chapters.FirstOrDefault(HasLanguage);
            var lang = firstChapterWithLanguage != null
                           ? firstChapterWithLanguage.Language.ISO_639_2
                           : Language.Undetermined.ISO_639_2;

            arguments.AddAll("--chapter-language", lang);
            arguments.AddAll("--chapter-charset", Encoding.BodyName.ToUpper());
            arguments.AddAll("--chapters", path);
        }

        private static void SaveAsXml(IEnumerable<Chapter> chapters, string path)
        {
            var writer = new XmlTextWriter(path, Encoding);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
                writer.WriteDocType("Chapters", null, "matroskachapters.dtd", null);
                writer.WriteStartElement("Chapters");
                    writer.WriteStartElement("EditionEntry");
                    foreach (var chapter in chapters.Where(chapter => chapter.Keep))
                    {
                        writer.WriteStartElement("ChapterAtom");
                            writer.WriteStartElement("ChapterTimeStart");
                                writer.WriteString(chapter.StartTimeXmlFormat); // 00:00:00.000
                            writer.WriteEndElement();
                            writer.WriteStartElement("ChapterDisplay");
                                writer.WriteStartElement("ChapterString");
                                    writer.WriteString(chapter.Title); // Chapter 1
                                writer.WriteEndElement();
                                if (HasLanguage(chapter))
                                {
                                    writer.WriteStartElement("ChapterLanguage");
                                        writer.WriteString(chapter.Language.ISO_639_2); // eng
                                    writer.WriteEndElement();
                                }   
                            writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        private static bool HasLanguage(Chapter chapter)
        {
            return chapter.Language != null && chapter.Language != Language.Undetermined;
        }
    }
}