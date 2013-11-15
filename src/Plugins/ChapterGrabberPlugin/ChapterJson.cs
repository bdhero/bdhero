using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChapterGrabberPlugin
{
    /// <summary>
    /// Represents a single search result (i.e., a user submission) from ChapterDb.org.
    /// </summary>
    public class JsonChaps
    {
        public ChapterInfo chapterInfo { get; set; }
    }

    public class Ref
    {
        public string chapterSetId { get; set; }
    }

    public class Source
    {
        public string name { get; set; }
        public string type { get; set; }
        public string hash { get; set; }
        public string fps { get; set; }
        public TimeSpan duration { get; set; }
    }

    public class JsonChapter
    {
        public TimeSpan time { get; set; }
        public string name { get; set; }
    }

    public class JsonChapters
    {
        public List<JsonChapter> chapter { get; set; }
    }

    public class ChapterInfo
    {
        public string xml_lang { get; set; }
        public string version { get; set; }
        public string extractor { get; set; }
        public string client { get; set; }
        public string confirmations { get; set; }
        public string xmlns { get; set; }
        public string title { get; set; }
        public Ref @ref { get; set; }
        public Source source { get; set; }
        public JsonChapters chapters { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string updatedBy { get; set; }
        public string updatedDate { get; set; }
    }
}
