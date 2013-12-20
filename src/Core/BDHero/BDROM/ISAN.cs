using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils.Annotations;

namespace BDHero.BDROM
{
    /// <summary>
    /// Immutable.
    /// </summary>
    /// <summary>
    /// ISAN (or ISAN number): International Standard Audiovisual Number.
    /// A globally unique number allocated for the exclusive identification of an AV Work,
    /// in accordance with the ISO 15706-1 standard.
    /// </summary>
    /// <remarks>
    /// <para>
    /// ISANs are globally unique and managed by a central authority.
    /// This gives us a way to uniquely identify Blu-ray Discs, as long as the studio includes it in the
    /// <c>AACS/mcmf.xml</c> file.  The majority of movies contain a valid ISAN, but about 1/3 do not.
    /// </para>
    /// <para>All ISANs have the following format:</para>
    /// <code>
    ///      +----Root----+ +EP+   +Version+
    ///      |            | |  |   |       |
    /// ISAN 0000-0000-14A9-0000-K-0000-0002-A
    ///                          |           |
    ///                          Check   Check
    ///                          Digit   Digit
    /// </code>
    /// <list type="list">
    ///     <item>First 12 digits: Root segment, which identifies the audiovisual work</item>
    ///     <item>Next 4 digits: Episode segment, which identifies the parts of a serialized work</item>
    ///     <item><i>(optional check character)</i></item>
    ///     <item>Last 8 digits: Version segment, which identifies the versions of a work</item>
    ///     <item><i>(optional check character)</i></item>
    /// </list>
    /// <para>
    /// ISANs may optionally contain 2 embedded check characters which guard against errors
    /// resulting from improper transcription of an ISAN.  The check characters are automatically
    /// assigned by a computer algorithm.  They are not mandatory, however, and may safely be omitted.
    /// </para>
    /// <para>Thus:</para>
    /// <code>0000-0000-14A9-0000-K-0000-0002-A</code>
    /// <para>is equivalent to:</para>
    /// <code>0000-0000-14A9-0000-0000-0002</code>
    /// <para>
    /// <see cref="Number"/> and <see cref="NumberFormatted"/> do <b>NOT</b> contain check digits.
    /// The ISAN found in the <c>AACS/mcmf.xml</c> file on Blu-ray Discs does not contain the optional check digits,
    /// so BDHero doesn't bother calculating them.
    /// </para>
    /// <para>
    /// ISANs are hierarchical, with a top-level ISAN identifying the original work, and one or more child V-ISANs
    /// identifying the various versions (releases or editions) of that work on Blu-ray.  According to
    /// the official ISAN user guide (http://www.isan.org/docs/isan_user_guide.pdf):
    /// </para>
    /// <blockquote>
    /// Version means a particular version, or aggregation of elements that affects the content of an AV Work.
    /// For example, any change that affects the content of an AV Work (e.g., artistic content, language, editing,
    /// technical format, distribution) and which requires separate identification for the use or exploitation
    /// of that specific content can be treated as a new Version for the purposes of assigning a V-ISAN.
    /// </blockquote>
    /// <para>
    /// For example:
    /// </para>
    /// <code>
    /// ISAN 0000-0000-14A9-0000-K-0000-0000-E: Blade runner (1982 - 117 min)
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0001-C: Blade Runner (2007 - 118 min): Europe BD Final Cut
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0002-A: Blade Runner (2007 - 118 min): NA BD Branching
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0003-8: Blade Runner (2007 - 140 min): NA BD Work Print
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0004-6: Blade Runner (2007 - 118 min): NA/Japan BD Final Cut
    /// </code>
    /// </remarks>
    /// TODO: Write unit tests
    /// TODO: Add language(s), alternate title(s), and additional information
    public class Isan
    {
        private static readonly string[] InvalidRoots = {"000000000000", "FFFFFFFFFFFF"};

        /// <summary>
        /// Matches any valid ISAN number <em>without dashes</em> at word boundaries.
        /// </summary>
        private static readonly Regex IsanRegex = new Regex(@"\b([0-9a-f]{4})([0-9a-f]{4})([0-9a-f]{4})([0-9a-f]{4})[a-z0-9]?([0-9a-f]{4})([0-9a-f]{4})[a-z0-9]?\b", RegexOptions.IgnoreCase);

        /// <summary>
        /// Unformatted ISAN without dashes or check digits.
        /// Consists of 24 hexadecimal digits.
        /// </summary>
        /// <example>
        /// <code>
        ///  +---Root---++EP++Version+
        ///  |          ||  ||      |
        ///  0000000014A9000000000002 = V-ISAN for BLADE_RUNNER_BRANCH
        ///  0000000014A9000000000000 = ISAN for BLADE_RUNNER_BRANCH
        /// </code>
        /// </example>
        public readonly string Number;

        /// <summary>
        /// Formatted ISAN with dashes but without check digits.
        /// Consists of 6 sets of 4 hexadecimal digits separated by dashes ('-').
        /// </summary>
        /// <example>
        /// <code>
        ///  +----Root----+ +EP+ +Version+
        ///  |            | |  | |       |
        ///  0000-0000-14A9-0000-0000-0002 = V-ISAN for BLADE_RUNNER_BRANCH
        ///  0000-0000-14A9-0000-0000-0000 = ISAN for BLADE_RUNNER_BRANCH
        /// </code>
        /// </example>
        public readonly string NumberFormatted;

        /// <summary>
        /// Gets the first 3 groups of 4 hex characters without dashes.
        /// </summary>
        public readonly string Root;

        /// <summary>
        /// Gets the first 3 groups of 4 hex characters with dashes between each group.
        /// </summary>
        public readonly string RootFormatted;

        /// <summary>
        /// Gets the 4th group of 4 hex characters.
        /// </summary>
        public readonly string Episode;

        /// <summary>
        /// Gets the last 2 groups of 4 hex characters without dashes.
        /// </summary>
        public readonly string Version;

        /// <summary>
        /// Gets the last 2 groups of 4 characters with dashes between each group.
        /// </summary>
        public readonly string VersionFormatted;

        /// <summary>
        /// Name of the audiovisual work (i.e., movie title).
        /// </summary>
        public string Title;

        /// <summary>
        /// Year the audiovisual work was released.  Note that this may differ between ISANs and V-ISANs (e.g., Blade Runner).
        /// </summary>
        public int? Year;

        /// <summary>
        /// Length (duration) of the film in minutes.
        /// </summary>
        public int? LengthMin;

        /// <summary>
        /// Gets a value indicating whether the ISAN's <see cref="Root"/> is valid and searchable (i.e., is not all zeros or Fs).
        /// </summary>
        public bool IsSearchable { get { return !InvalidRoots.Contains(Root); } }

        protected Isan(string n1, string n2, string n3, string n4, string n5, string n6)
        {
            Root = string.Format("{0}{1}{2}", n1, n2, n3);
            RootFormatted = string.Format("{0}-{1}-{2}", n1, n2, n3);

            Episode = n4;

            Version = string.Format("{0}{1}", n5, n6);
            VersionFormatted = string.Format("{0}-{1}", n5, n6);

            Number = string.Format("{0}{1}{2}", Root, Episode, Version);
            NumberFormatted = string.Format("{0}-{1}-{2}", RootFormatted, Episode, VersionFormatted);
        }

        public static bool IsIsan(string number)
        {
            number = number.Replace("-", "");
            return IsanRegex.IsMatch(number);
        }

        /// <summary>
        /// Attempts to parse an ISAN number.
        /// </summary>
        /// <param name="number">Any valid ISAN number</param>
        /// <returns>A new ISAN object if the given number is a valid ISAN number; otherwise <c>null</c></returns>
        [CanBeNull]
        public static Isan TryParse(string number)
        {
            if (!IsIsan(number))
                return null;

            var n = Parse(number);
            
            return new Isan(n[0], n[1], n[2], n[3], n[4], n[5]);
        }

        /// <summary>
        /// Parses any valid ISAN number into 6 individual groups of 4 hex characters each.
        /// </summary>
        /// <param name="number">Any valid ISAN number</param>
        /// <returns>Enumerable containing 6 groups of 4 characters each</returns>
        protected static string[] Parse(string number)
        {
            number = number.Replace("-", "");
            return !IsIsan(number) ? null : IsanRegex.Match(number).Groups.OfType<Group>().Skip(1).Select(@group => @group.Value).ToArray();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}: \"{2}\" ({3})",
                                 IsSearchable ? "valid" : "invalid!",
                                 NumberFormatted,
                                 Title,
                                 Year
                );
        }
    }

    public class VIsan : Isan
    {
        public Isan Parent;

        protected VIsan(string n1, string n2, string n3, string n4, string n5, string n6)
            : base(n1, n2, n3, n4, n5, n6)
        {
        }

        public new static VIsan TryParse(string number)
        {
            if (!IsIsan(number))
                return null;

            var n = Parse(number);

            return new VIsan(n[0], n[1], n[2], n[3], n[4], n[5]);
        }
    }
}
