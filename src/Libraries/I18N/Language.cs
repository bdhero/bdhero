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

using System;
using System.Collections.Generic;
using System.Globalization;

// ReSharper disable InconsistentNaming
// ReSharper disable ReturnTypeCanBeEnumerable.Global
namespace I18N
{
    /// <summary>
    /// Represents an international language used by Blu-ray Discs.
    /// </summary>
    public class Language : IComparable<Language>, IComparable
    {
        #region Static properties (private)

        private static readonly List<Language> Languages = new List<Language>();
        private static readonly Dictionary<string, Language> ISO_639_1_Map = new Dictionary<string, Language>();
        private static readonly Dictionary<string, Language> ISO_639_2_Map = new Dictionary<string, Language>();

        #endregion

        #region Static properties (public)

        /// <summary>
        /// Gets a readonly collection (in no particular order) of all known Blu-ray <see cref="Language"/>s.
        /// </summary>
        public static ICollection<Language> AllLanguages
        {
            get { return Languages.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the language of the operating system's UI culture for the logged in user.
        /// </summary>
        public static Language CurrentUILanguage
        {
            get { return FromCode(CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName); }
        }

        /// <summary>
        /// Gets a readonly collection (in no particular order) of all known Blu-ray ISO 639-2 language codes.
        /// </summary>
        public static ICollection<string> ISO6392Codes
        {
            get { return new List<string>(ISO_639_2_Map.Keys).AsReadOnly(); }
        }

        /// <summary>
        /// Gets the Language object for the "und" language code.
        /// </summary>
        /// <remarks>Shortcut for <c>FromCode("und")</c>.</remarks>
        public static Language Undetermined { get { return FromCode("und"); } }

        /// <summary>
        /// Gets the Language object for the "eng" language code.
        /// </summary>
        /// <remarks>Shortcut for <c>FromCode("eng")</c>.</remarks>
        public static Language English { get { return FromCode("eng"); } }

        #endregion

        #region Instance methods and properties

        /// <summary>2-digit ISO 639-1 code (e.g., "en", "fr", "es")</summary>
        public string ISO_639_1 { get; private set; }

        /// <summary>3-digit ISO 639-2 code (e.g., "eng", "fra", "spa")</summary>
        public string ISO_639_2 { get; private set; }

        /// <summary>Human-friendly English name of the language (e.g., "English", "French", "Spanish")</summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a string containing the <see cref="ISO_639_2"/> and <see cref="Name"/> of the language,
        /// suitable for displaying in UIs.
        /// </summary>
        public string UIDisplayName
        {
            get { return string.Format("{0} - {1}", ISO_639_2, Name); }
        }

        private Language(string ISO_639_1, string ISO_639_2, string Name)
        {
            this.ISO_639_1 = ISO_639_1;
            this.ISO_639_2 = ISO_639_2;
            this.Name = Name;
        }

        public override string ToString()
        {
            return Name;
        }

        protected bool Equals(Language other)
        {
            return string.Equals(ISO_639_2, other.ISO_639_2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Language) obj);
        }

        public override int GetHashCode()
        {
            return (ISO_639_2 != null ? ISO_639_2.GetHashCode() : 0);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Language;
            if (other != null) return CompareTo(other);
            if (obj == null) return 0;
            return 1;
        }

        public int CompareTo(Language other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion

        #region Static methods

        static Language()
        {
            Languages.Add(new Language("ab", "abk", "Abkhazian"));
            Languages.Add(new Language(null, "ace", "Achinese"));
            Languages.Add(new Language(null, "ach", "Acoli"));
            Languages.Add(new Language(null, "ada", "Adangme"));
            Languages.Add(new Language("aa", "aar", "Afar"));
            Languages.Add(new Language(null, "afh", "Afrihili"));
            Languages.Add(new Language("af", "afr", "Afrikaans"));
            Languages.Add(new Language(null, "afa", "Afro-Asiatic (Other)"));
            Languages.Add(new Language("ak", "aka", "Akan"));
            Languages.Add(new Language(null, "akk", "Akkadian"));
            Languages.Add(new Language(null, "alb", "Albanian"));
            Languages.Add(new Language("sq", "sqi", "Albanian"));
            Languages.Add(new Language(null, "ale", "Aleut"));
            Languages.Add(new Language(null, "alg", "Algonquian languages"));
            Languages.Add(new Language(null, "tut", "Altaic (Other)"));
            Languages.Add(new Language("am", "amh", "Amharic"));
            Languages.Add(new Language(null, "apa", "Apache languages"));
            Languages.Add(new Language("ar", "ara", "Arabic"));
            Languages.Add(new Language(null, "arc", "Aramaic"));
            Languages.Add(new Language(null, "arp", "Arapaho"));
            Languages.Add(new Language(null, "arn", "Araucanian"));
            Languages.Add(new Language(null, "arw", "Arawak"));
            Languages.Add(new Language(null, "arm", "Armenian"));
            Languages.Add(new Language("hy", "hye", "Armenian"));
            Languages.Add(new Language(null, "art", "Artificial (Other)"));
            Languages.Add(new Language("as", "asm", "Assamese"));
            Languages.Add(new Language(null, "ath", "Athapascan languages"));
            Languages.Add(new Language(null, "aus", "Australian languages"));
            Languages.Add(new Language(null, "map", "Austronesian (Other)"));
            Languages.Add(new Language("av", "ava", "Avaric"));
            Languages.Add(new Language("ae", "ave", "Avestan"));
            Languages.Add(new Language(null, "awa", "Awadhi"));
            Languages.Add(new Language("ay", "aym", "Aymara"));
            Languages.Add(new Language("az", "aze", "Azerbaijani"));
            Languages.Add(new Language(null, "ban", "Balinese"));
            Languages.Add(new Language(null, "bat", "Baltic (Other)"));
            Languages.Add(new Language(null, "bal", "Baluchi"));
            Languages.Add(new Language("bm", "bam", "Bambara"));
            Languages.Add(new Language(null, "bai", "Bamileke languages"));
            Languages.Add(new Language(null, "bad", "Banda"));
            Languages.Add(new Language(null, "bnt", "Bantu (Other)"));
            Languages.Add(new Language(null, "bas", "Basa"));
            Languages.Add(new Language("ba", "bak", "Bashkir"));
            Languages.Add(new Language(null, "baq", "Basque"));
            Languages.Add(new Language("eu", "eus", "Basque"));
            Languages.Add(new Language(null, "btk", "Batak (Indonesia)"));
            Languages.Add(new Language(null, "bej", "Beja"));
            Languages.Add(new Language("be", "bel", "Belarusian"));
            Languages.Add(new Language(null, "bem", "Bemba"));
            Languages.Add(new Language("bn", "ben", "Bengali"));
            Languages.Add(new Language(null, "ber", "Berber (Other)"));
            Languages.Add(new Language(null, "bho", "Bhojpuri"));
            Languages.Add(new Language("bh", "bih", "Bihari"));
            Languages.Add(new Language(null, "bik", "Bikol"));
            Languages.Add(new Language(null, "bin", "Bini"));
            Languages.Add(new Language("bi", "bis", "Bislama"));
            Languages.Add(new Language("bs", "bos", "Bosnian"));
            Languages.Add(new Language(null, "bra", "Braj"));
            Languages.Add(new Language("br", "bre", "Breton"));
            Languages.Add(new Language(null, "bug", "Buginese"));
            Languages.Add(new Language("bg", "bul", "Bulgarian"));
            Languages.Add(new Language(null, "bua", "Buriat"));
            Languages.Add(new Language(null, "bur", "Burmese"));
            Languages.Add(new Language("my", "mya", "Burmese"));
            Languages.Add(new Language(null, "cad", "Caddo"));
            Languages.Add(new Language(null, "car", "Carib"));
            Languages.Add(new Language("ca", "cat", "Catalan"));
            Languages.Add(new Language(null, "cau", "Caucasian (Other)"));
            Languages.Add(new Language(null, "ceb", "Cebuano"));
            Languages.Add(new Language(null, "cel", "Celtic (Other)"));
            Languages.Add(new Language(null, "cai", "Central American Indian (Other)"));
            Languages.Add(new Language(null, "chg", "Chagatai"));
            Languages.Add(new Language(null, "cmc", "Chamic languages"));
            Languages.Add(new Language("ch", "cha", "Chamorro"));
            Languages.Add(new Language("ce", "che", "Chechen"));
            Languages.Add(new Language(null, "chr", "Cherokee"));
            Languages.Add(new Language(null, "chy", "Cheyenne"));
            Languages.Add(new Language(null, "chb", "Chibcha"));
            Languages.Add(new Language(null, "chi", "Chinese"));
            Languages.Add(new Language("zh", "zho", "Chinese"));
            Languages.Add(new Language(null, "chn", "Chinook jargon"));
            Languages.Add(new Language(null, "chp", "Chipewyan"));
            Languages.Add(new Language(null, "cho", "Choctaw"));
            Languages.Add(new Language("cu", "chu", "Church Slavic"));
            Languages.Add(new Language(null, "chk", "Chuukese"));
            Languages.Add(new Language("cv", "chv", "Chuvash"));
            Languages.Add(new Language(null, "cop", "Coptic"));
            Languages.Add(new Language("kw", "cor", "Cornish"));
            Languages.Add(new Language("co", "cos", "Corsican"));
            Languages.Add(new Language("cr", "cre", "Cree"));
            Languages.Add(new Language(null, "mus", "Creek"));
            Languages.Add(new Language(null, "crp", "Creoles and pidgins (Other)"));
            Languages.Add(new Language(null, "cpe", "Creoles and pidgins,"));
            Languages.Add(new Language(null, "cpf", "Creoles and pidgins,"));
            Languages.Add(new Language(null, "cpp", "Creoles and pidgins,"));
            Languages.Add(new Language(null, "scr", "Croatian"));
            Languages.Add(new Language("hr", "hrv", "Croatian"));
            Languages.Add(new Language(null, "cus", "Cushitic (Other)"));
            Languages.Add(new Language(null, "cze", "Czech"));
            Languages.Add(new Language("cs", "ces", "Czech"));
            Languages.Add(new Language(null, "dak", "Dakota"));
            Languages.Add(new Language("da", "dan", "Danish"));
            Languages.Add(new Language(null, "day", "Dayak"));
            Languages.Add(new Language(null, "del", "Delaware"));
            Languages.Add(new Language(null, "din", "Dinka"));
            Languages.Add(new Language("dv", "div", "Divehi"));
            Languages.Add(new Language(null, "doi", "Dogri"));
            Languages.Add(new Language(null, "dgr", "Dogrib"));
            Languages.Add(new Language(null, "dra", "Dravidian (Other)"));
            Languages.Add(new Language(null, "dua", "Duala"));
            Languages.Add(new Language(null, "dut", "Dutch"));
            Languages.Add(new Language("nl", "nld", "Dutch"));
            Languages.Add(new Language(null, "dum", "Dutch, Middle (ca. 1050-1350)"));
            Languages.Add(new Language(null, "dyu", "Dyula"));
            Languages.Add(new Language("dz", "dzo", "Dzongkha"));
            Languages.Add(new Language(null, "efi", "Efik"));
            Languages.Add(new Language(null, "egy", "Egyptian (Ancient)"));
            Languages.Add(new Language(null, "eka", "Ekajuk"));
            Languages.Add(new Language(null, "elx", "Elamite"));
            Languages.Add(new Language("en", "eng", "English"));
            Languages.Add(new Language(null, "enm", "English, Middle (1100-1500)"));
            Languages.Add(new Language(null, "ang", "English, Old (ca.450-1100)"));
            Languages.Add(new Language("eo", "epo", "Esperanto"));
            Languages.Add(new Language("et", "est", "Estonian"));
            Languages.Add(new Language("ee", "ewe", "Ewe"));
            Languages.Add(new Language(null, "ewo", "Ewondo"));
            Languages.Add(new Language(null, "fan", "Fang"));
            Languages.Add(new Language(null, "fat", "Fanti"));
            Languages.Add(new Language("fo", "fao", "Faroese"));
            Languages.Add(new Language("fj", "fij", "Fijian"));
            Languages.Add(new Language("fi", "fin", "Finnish"));
            Languages.Add(new Language(null, "fiu", "Finno-Ugrian (Other)"));
            Languages.Add(new Language(null, "fon", "Fon"));
            Languages.Add(new Language(null, "fre", "French"));
            Languages.Add(new Language("fr", "fra", "French"));
            Languages.Add(new Language(null, "frm", "French, Middle (ca.1400-1600)"));
            Languages.Add(new Language(null, "fro", "French, Old (842-ca.1400)"));
            Languages.Add(new Language("fy", "fry", "Frisian"));
            Languages.Add(new Language(null, "fur", "Friulian"));
            Languages.Add(new Language("ff", "ful", "Fulah"));
            Languages.Add(new Language(null, "gaa", "Ga"));
            Languages.Add(new Language("gl", "glg", "Gallegan"));
            Languages.Add(new Language("lg", "lug", "Ganda"));
            Languages.Add(new Language(null, "gay", "Gayo"));
            Languages.Add(new Language(null, "gba", "Gbaya"));
            Languages.Add(new Language(null, "gez", "Geez"));
            Languages.Add(new Language(null, "geo", "Georgian"));
            Languages.Add(new Language("ka", "kat", "Georgian"));
            Languages.Add(new Language(null, "ger", "German"));
            Languages.Add(new Language("de", "deu", "German"));
            Languages.Add(new Language(null, "nds", "Saxon"));
            Languages.Add(new Language(null, "gmh", "German, Middle High (ca.1050-1500)"));
            Languages.Add(new Language(null, "goh", "German, Old High (ca.750-1050)"));
            Languages.Add(new Language(null, "gem", "Germanic (Other)"));
            Languages.Add(new Language(null, "gil", "Gilbertese"));
            Languages.Add(new Language(null, "gon", "Gondi"));
            Languages.Add(new Language(null, "gor", "Gorontalo"));
            Languages.Add(new Language(null, "got", "Gothic"));
            Languages.Add(new Language(null, "grb", "Grebo"));
            Languages.Add(new Language(null, "grc", "Greek, Ancient (to 1453)"));
            Languages.Add(new Language(null, "gre", "Greek"));
            Languages.Add(new Language("el", "ell", "Greek"));
            Languages.Add(new Language("gn", "grn", "Guarani"));
            Languages.Add(new Language("gu", "guj", "Gujarati"));
            Languages.Add(new Language(null, "gwi", "Gwich´in"));
            Languages.Add(new Language(null, "hai", "Haida"));
            Languages.Add(new Language("ha", "hau", "Hausa"));
            Languages.Add(new Language(null, "haw", "Hawaiian"));
            Languages.Add(new Language("he", "heb", "Hebrew"));
            Languages.Add(new Language("hz", "her", "Herero"));
            Languages.Add(new Language(null, "hil", "Hiligaynon"));
            Languages.Add(new Language(null, "him", "Himachali"));
            Languages.Add(new Language("hi", "hin", "Hindi"));
            Languages.Add(new Language("ho", "hmo", "Hiri Motu"));
            Languages.Add(new Language(null, "hit", "Hittite"));
            Languages.Add(new Language(null, "hmn", "Hmong"));
            Languages.Add(new Language("hu", "hun", "Hungarian"));
            Languages.Add(new Language(null, "hup", "Hupa"));
            Languages.Add(new Language(null, "iba", "Iban"));
            Languages.Add(new Language(null, "ice", "Icelandic"));
            Languages.Add(new Language("is", "isl", "Icelandic"));
            Languages.Add(new Language("ig", "ibo", "Igbo"));
            Languages.Add(new Language(null, "ijo", "Ijo"));
            Languages.Add(new Language(null, "ilo", "Iloko"));
            Languages.Add(new Language(null, "inc", "Indic (Other)"));
            Languages.Add(new Language(null, "ine", "Indo-European (Other)"));
            Languages.Add(new Language("id", "ind", "Indonesian"));
            Languages.Add(new Language("ia", "ina", "Interlingua (International"));
            Languages.Add(new Language("ie", "ile", "Interlingue"));
            Languages.Add(new Language("iu", "iku", "Inuktitut"));
            Languages.Add(new Language("ik", "ipk", "Inupiaq"));
            Languages.Add(new Language(null, "ira", "Iranian (Other)"));
            Languages.Add(new Language("ga", "gle", "Irish"));
            Languages.Add(new Language(null, "mga", "Irish, Middle (900-1200)"));
            Languages.Add(new Language(null, "sga", "Irish, Old (to 900)"));
            Languages.Add(new Language(null, "iro", "Iroquoian languages"));
            Languages.Add(new Language("it", "ita", "Italian"));
            Languages.Add(new Language("ja", "jpn", "Japanese"));
            Languages.Add(new Language("jv", "jav", "Javanese"));
            Languages.Add(new Language(null, "jrb", "Judeo-Arabic"));
            Languages.Add(new Language(null, "jpr", "Judeo-Persian"));
            Languages.Add(new Language(null, "kab", "Kabyle"));
            Languages.Add(new Language(null, "kac", "Kachin"));
            Languages.Add(new Language("kl", "kal", "Kalaallisut"));
            Languages.Add(new Language(null, "kam", "Kamba"));
            Languages.Add(new Language("kn", "kan", "Kannada"));
            Languages.Add(new Language("kr", "kau", "Kanuri"));
            Languages.Add(new Language(null, "kaa", "Kara-Kalpak"));
            Languages.Add(new Language(null, "kar", "Karen"));
            Languages.Add(new Language("ks", "kas", "Kashmiri"));
            Languages.Add(new Language(null, "kaw", "Kawi"));
            Languages.Add(new Language("kk", "kaz", "Kazakh"));
            Languages.Add(new Language(null, "kha", "Khasi"));
            Languages.Add(new Language("km", "khm", "Khmer"));
            Languages.Add(new Language(null, "khi", "Khoisan (Other)"));
            Languages.Add(new Language(null, "kho", "Khotanese"));
            Languages.Add(new Language("ki", "kik", "Kikuyu"));
            Languages.Add(new Language(null, "kmb", "Kimbundu"));
            Languages.Add(new Language("rw", "kin", "Kinyarwanda"));
            Languages.Add(new Language("ky", "kir", "Kirghiz"));
            Languages.Add(new Language("kv", "kom", "Komi"));
            Languages.Add(new Language("kg", "kon", "Kongo"));
            Languages.Add(new Language(null, "kok", "Konkani"));
            Languages.Add(new Language("ko", "kor", "Korean"));
            Languages.Add(new Language(null, "kos", "Kosraean"));
            Languages.Add(new Language(null, "kpe", "Kpelle"));
            Languages.Add(new Language(null, "kro", "Kru"));
            Languages.Add(new Language("kj", "kua", "Kuanyama"));
            Languages.Add(new Language(null, "kum", "Kumyk"));
            Languages.Add(new Language("ku", "kur", "Kurdish"));
            Languages.Add(new Language(null, "kru", "Kurukh"));
            Languages.Add(new Language(null, "kut", "Kutenai"));
            Languages.Add(new Language(null, "lad", "Ladino"));
            Languages.Add(new Language(null, "lah", "Lahnda"));
            Languages.Add(new Language(null, "lam", "Lamba"));
            Languages.Add(new Language("lo", "lao", "Lao"));
            Languages.Add(new Language("la", "lat", "Latin"));
            Languages.Add(new Language("lv", "lav", "Latvian"));
            Languages.Add(new Language("lb", "ltz", "Letzeburgesch"));
            Languages.Add(new Language(null, "lez", "Lezghian"));
            Languages.Add(new Language("ln", "lin", "Lingala"));
            Languages.Add(new Language("lt", "lit", "Lithuanian"));
            Languages.Add(new Language(null, "loz", "Lozi"));
            Languages.Add(new Language("lu", "lub", "Luba-Katanga"));
            Languages.Add(new Language(null, "lua", "Luba-Lulua"));
            Languages.Add(new Language(null, "lui", "Luiseno"));
            Languages.Add(new Language(null, "lun", "Lunda"));
            Languages.Add(new Language(null, "luo", "Luo (Kenya and Tanzania)"));
            Languages.Add(new Language(null, "lus", "Lushai"));
            Languages.Add(new Language(null, "mac", "Macedonian"));
            Languages.Add(new Language("mk", "mkd", "Macedonian"));
            Languages.Add(new Language(null, "mad", "Madurese"));
            Languages.Add(new Language(null, "mag", "Magahi"));
            Languages.Add(new Language(null, "mai", "Maithili"));
            Languages.Add(new Language(null, "mak", "Makasar"));
            Languages.Add(new Language("mg", "mlg", "Malagasy"));
            Languages.Add(new Language(null, "may", "Malay"));
            Languages.Add(new Language("ms", "msa", "Malay"));
            Languages.Add(new Language("ml", "mal", "Malayalam"));
            Languages.Add(new Language("mt", "mlt", "Maltese"));
            Languages.Add(new Language(null, "mnc", "Manchu"));
            Languages.Add(new Language(null, "mdr", "Mandar"));
            Languages.Add(new Language(null, "man", "Mandingo"));
            Languages.Add(new Language(null, "mni", "Manipuri"));
            Languages.Add(new Language(null, "mno", "Manobo languages"));
            Languages.Add(new Language("gv", "glv", "Manx"));
            Languages.Add(new Language(null, "mao", "Maori"));
            Languages.Add(new Language("mi", "mri", "Maori"));
            Languages.Add(new Language("mr", "mar", "Marathi"));
            Languages.Add(new Language(null, "chm", "Mari"));
            Languages.Add(new Language("mh", "mah", "Marshall"));
            Languages.Add(new Language(null, "mwr", "Marwari"));
            Languages.Add(new Language(null, "mas", "Masai"));
            Languages.Add(new Language(null, "myn", "Mayan languages"));
            Languages.Add(new Language(null, "men", "Mende"));
            Languages.Add(new Language(null, "mic", "Micmac"));
            Languages.Add(new Language(null, "min", "Minangkabau"));
            Languages.Add(new Language(null, "mis", "Miscellaneous languages"));
            Languages.Add(new Language(null, "moh", "Mohawk"));
            Languages.Add(new Language(null, "mol", "Moldavian"));
            Languages.Add(new Language(null, "mkh", "Mon-Khmer (Other)"));
            Languages.Add(new Language(null, "lol", "Mongo"));
            Languages.Add(new Language("mn", "mon", "Mongolian"));
            Languages.Add(new Language(null, "mos", "Mossi"));
            Languages.Add(new Language(null, "mul", "Multiple languages"));
            Languages.Add(new Language(null, "mun", "Munda languages"));
            Languages.Add(new Language(null, "nah", "Nahuatl"));
            Languages.Add(new Language("na", "nau", "Nauru"));
            Languages.Add(new Language("nv", "nav", "Navajo"));
            Languages.Add(new Language("nd", "nde", "Ndebele, North"));
            Languages.Add(new Language("nr", "nbl", "Ndebele, South"));
            Languages.Add(new Language("ng", "ndo", "Ndonga"));
            Languages.Add(new Language("ne", "nep", "Nepali"));
            Languages.Add(new Language(null, "new", "Newari"));
            Languages.Add(new Language(null, "nia", "Nias"));
            Languages.Add(new Language(null, "nic", "Niger-Kordofanian (Other)"));
            Languages.Add(new Language(null, "ssa", "Nilo-Saharan (Other)"));
            Languages.Add(new Language(null, "niu", "Niuean"));
            Languages.Add(new Language(null, "non", "Norse, Old"));
            Languages.Add(new Language(null, "nai", "North American Indian (Other)"));
            Languages.Add(new Language("se", "sme", "Northern Sami"));
            Languages.Add(new Language("no", "nor", "Norwegian"));
            Languages.Add(new Language("nb", "nob", "Norwegian Bokmål"));
            Languages.Add(new Language("nn", "nno", "Norwegian Nynorsk"));
            Languages.Add(new Language(null, "nub", "Nubian languages"));
            Languages.Add(new Language(null, "nym", "Nyamwezi"));
            Languages.Add(new Language("ny", "nya", "Nyanja"));
            Languages.Add(new Language(null, "nyn", "Nyankole"));
            Languages.Add(new Language(null, "nyo", "Nyoro"));
            Languages.Add(new Language(null, "nzi", "Nzima"));
            Languages.Add(new Language("oc", "oci", "Occitan"));
            Languages.Add(new Language("oj", "oji", "Ojibwa"));
            Languages.Add(new Language("or", "ori", "Oriya"));
            Languages.Add(new Language("om", "orm", "Oromo"));
            Languages.Add(new Language(null, "osa", "Osage"));
            Languages.Add(new Language("os", "oss", "Ossetian"));
            Languages.Add(new Language(null, "oto", "Otomian languages"));
            Languages.Add(new Language(null, "pal", "Pahlavi"));
            Languages.Add(new Language(null, "pau", "Palauan"));
            Languages.Add(new Language("pi", "pli", "Pali"));
            Languages.Add(new Language(null, "pam", "Pampanga"));
            Languages.Add(new Language(null, "pag", "Pangasinan"));
            Languages.Add(new Language("pa", "pan", "Panjabi"));
            Languages.Add(new Language(null, "pap", "Papiamento"));
            Languages.Add(new Language(null, "paa", "Papuan (Other)"));
            Languages.Add(new Language(null, "per", "Persian"));
            Languages.Add(new Language("fa", "fas", "Persian"));
            Languages.Add(new Language(null, "peo", "Persian, Old (ca.600-400 B.C.)"));
            Languages.Add(new Language(null, "phi", "Philippine (Other)"));
            Languages.Add(new Language(null, "phn", "Phoenician"));
            Languages.Add(new Language(null, "pon", "Pohnpeian"));
            Languages.Add(new Language("pl", "pol", "Polish"));
            Languages.Add(new Language("pt", "por", "Portuguese"));
            Languages.Add(new Language(null, "pra", "Prakrit languages"));
            Languages.Add(new Language(null, "pro", "Provençal"));
            Languages.Add(new Language("ps", "pus", "Pushto"));
            Languages.Add(new Language("qu", "que", "Quechua"));
            Languages.Add(new Language("rm", "roh", "Raeto-Romance"));
            Languages.Add(new Language(null, "raj", "Rajasthani"));
            Languages.Add(new Language(null, "rap", "Rapanui"));
            Languages.Add(new Language(null, "rar", "Rarotongan"));
            Languages.Add(new Language(null, "roa", "Romance (Other)"));
            Languages.Add(new Language(null, "rum", "Romanian"));
            Languages.Add(new Language("ro", "ron", "Romanian"));
            Languages.Add(new Language(null, "rom", "Romany"));
            Languages.Add(new Language("rn", "run", "Rundi"));
            Languages.Add(new Language("ru", "rus", "Russian"));
            Languages.Add(new Language(null, "sal", "Salishan languages"));
            Languages.Add(new Language(null, "sam", "Samaritan Aramaic"));
            Languages.Add(new Language(null, "smi", "Sami languages (Other)"));
            Languages.Add(new Language("sm", "smo", "Samoan"));
            Languages.Add(new Language(null, "sad", "Sandawe"));
            Languages.Add(new Language("sg", "sag", "Sango"));
            Languages.Add(new Language("sa", "san", "Sanskrit"));
            Languages.Add(new Language(null, "sat", "Santali"));
            Languages.Add(new Language("sc", "srd", "Sardinian"));
            Languages.Add(new Language(null, "sas", "Sasak"));
            Languages.Add(new Language(null, "sco", "Scots"));
            Languages.Add(new Language("gd", "gla", "Gaelic"));
            Languages.Add(new Language(null, "sel", "Selkup"));
            Languages.Add(new Language(null, "sem", "Semitic (Other)"));
            Languages.Add(new Language(null, "scc", "Serbian"));
            Languages.Add(new Language("sr", "srp", "Serbian"));
            Languages.Add(new Language(null, "srr", "Serer"));
            Languages.Add(new Language(null, "shn", "Shan"));
            Languages.Add(new Language("sn", "sna", "Shona"));
            Languages.Add(new Language(null, "sid", "Sidamo"));
            Languages.Add(new Language(null, "sgn", "Sign languages"));
            Languages.Add(new Language(null, "bla", "Siksika"));
            Languages.Add(new Language("sd", "snd", "Sindhi"));
            Languages.Add(new Language("si", "sin", "Sinhalese"));
            Languages.Add(new Language(null, "sit", "Sino-Tibetan (Other)"));
            Languages.Add(new Language(null, "sio", "Siouan languages"));
            Languages.Add(new Language(null, "den", "Slave (Athapascan)"));
            Languages.Add(new Language(null, "sla", "Slavic (Other)"));
            Languages.Add(new Language(null, "slo", "Slovak"));
            Languages.Add(new Language("sk", "slk", "Slovak"));
            Languages.Add(new Language("sl", "slv", "Slovenian"));
            Languages.Add(new Language(null, "sog", "Sogdian"));
            Languages.Add(new Language("so", "som", "Somali"));
            Languages.Add(new Language(null, "son", "Songhai"));
            Languages.Add(new Language(null, "snk", "Soninke"));
            Languages.Add(new Language(null, "wen", "Sorbian languages"));
            Languages.Add(new Language(null, "nso", "Sotho, Northern"));
            Languages.Add(new Language("st", "sot", "Sotho, Southern"));
            Languages.Add(new Language(null, "sai", "South American Indian (Other)"));
            Languages.Add(new Language("es", "spa", "Spanish"));
            Languages.Add(new Language(null, "suk", "Sukuma"));
            Languages.Add(new Language(null, "sux", "Sumerian"));
            Languages.Add(new Language("su", "sun", "Sundanese"));
            Languages.Add(new Language(null, "sus", "Susu"));
            Languages.Add(new Language("sw", "swa", "Swahili"));
            Languages.Add(new Language("ss", "ssw", "Swati"));
            Languages.Add(new Language("sv", "swe", "Swedish"));
            Languages.Add(new Language(null, "syr", "Syriac"));
            Languages.Add(new Language("tl", "tgl", "Tagalog"));
            Languages.Add(new Language("ty", "tah", "Tahitian"));
            Languages.Add(new Language(null, "tai", "Tai (Other)"));
            Languages.Add(new Language("tg", "tgk", "Tajik"));
            Languages.Add(new Language(null, "tmh", "Tamashek"));
            Languages.Add(new Language("ta", "tam", "Tamil"));
            Languages.Add(new Language("tt", "tat", "Tatar"));
            Languages.Add(new Language("te", "tel", "Telugu"));
            Languages.Add(new Language(null, "ter", "Tereno"));
            Languages.Add(new Language(null, "tet", "Tetum"));
            Languages.Add(new Language("th", "tha", "Thai"));
            Languages.Add(new Language(null, "tib", "Tibetan"));
            Languages.Add(new Language("bo", "bod", "Tibetan"));
            Languages.Add(new Language(null, "tig", "Tigre"));
            Languages.Add(new Language("ti", "tir", "Tigrinya"));
            Languages.Add(new Language(null, "tem", "Timne"));
            Languages.Add(new Language(null, "tiv", "Tiv"));
            Languages.Add(new Language(null, "tli", "Tlingit"));
            Languages.Add(new Language(null, "tpi", "Tok Pisin"));
            Languages.Add(new Language(null, "tkl", "Tokelau"));
            Languages.Add(new Language(null, "tog", "Tonga (Nyasa)"));
            Languages.Add(new Language("to", "ton", "Tonga (Tonga Islands)"));
            Languages.Add(new Language(null, "tsi", "Tsimshian"));
            Languages.Add(new Language("ts", "tso", "Tsonga"));
            Languages.Add(new Language("tn", "tsn", "Tswana"));
            Languages.Add(new Language(null, "tum", "Tumbuka"));
            Languages.Add(new Language("tr", "tur", "Turkish"));
            Languages.Add(new Language(null, "ota", "Turkish, Ottoman (1500-1928)"));
            Languages.Add(new Language("tk", "tuk", "Turkmen"));
            Languages.Add(new Language(null, "tvl", "Tuvalu"));
            Languages.Add(new Language(null, "tyv", "Tuvinian"));
            Languages.Add(new Language("tw", "twi", "Twi"));
            Languages.Add(new Language(null, "uga", "Ugaritic"));
            Languages.Add(new Language("ug", "uig", "Uighur"));
            Languages.Add(new Language("uk", "ukr", "Ukrainian"));
            Languages.Add(new Language(null, "umb", "Umbundu"));
            Languages.Add(new Language(null, "und", "Undetermined"));
            Languages.Add(new Language("ur", "urd", "Urdu"));
            Languages.Add(new Language("uz", "uzb", "Uzbek"));
            Languages.Add(new Language(null, "vai", "Vai"));
            Languages.Add(new Language("ve", "ven", "Venda"));
            Languages.Add(new Language("vi", "vie", "Vietnamese"));
            Languages.Add(new Language("vo", "vol", "Volapük"));
            Languages.Add(new Language(null, "vot", "Votic"));
            Languages.Add(new Language(null, "wak", "Wakashan languages"));
            Languages.Add(new Language(null, "wal", "Walamo"));
            Languages.Add(new Language(null, "war", "Waray"));
            Languages.Add(new Language(null, "was", "Washo"));
            Languages.Add(new Language(null, "wel", "Welsh"));
            Languages.Add(new Language("cy", "cym", "Welsh"));
            Languages.Add(new Language("wo", "wol", "Wolof"));
            Languages.Add(new Language("xh", "xho", "Xhosa"));
            Languages.Add(new Language(null, "sah", "Yakut"));
            Languages.Add(new Language(null, "yao", "Yao"));
            Languages.Add(new Language(null, "yap", "Yapese"));
            Languages.Add(new Language("yi", "yid", "Yiddish"));
            Languages.Add(new Language("yo", "yor", "Yoruba"));
            Languages.Add(new Language(null, "ypk", "Yupik languages"));
            Languages.Add(new Language(null, "znd", "Zande"));
            Languages.Add(new Language(null, "zap", "Zapotec"));
            Languages.Add(new Language(null, "zen", "Zenaga"));
            Languages.Add(new Language("za", "zha", "Zhuang"));
            Languages.Add(new Language("zu", "zul", "Zulu"));
            Languages.Add(new Language(null, "zun", "Zuni"));

            foreach (Language lang in Languages)
            {
                if (!String.IsNullOrEmpty(lang.ISO_639_1))
                    ISO_639_1_Map.Add(lang.ISO_639_1, lang);
                if (!String.IsNullOrEmpty(lang.ISO_639_2))
                    ISO_639_2_Map.Add(lang.ISO_639_2, lang);
            }
        }

        public static string GetName(string code)
        {
            Language lang = FromCode(code);
            return lang != null ? lang.Name : null;
        }

        /// <summary>
        /// Gets the Language object associated with the given 2-letter (ISO 639-1) or 3-letter (ISO 639-2) language code.
        /// </summary>
        /// <param name="code">2-letter (ISO 639-1) or 3-letter (ISO 639-2) ISO language code</param>
        /// <returns>Language object that matches the ISO 639 code, or null if none exists</returns>
        public static Language FromCode(string code)
        {
            code = code != null ? code.ToLowerInvariant() : null;
            Language lang = null;
            if (!String.IsNullOrEmpty(code))
            {
                switch (code.Length)
                {
                    case 2:
                        ISO_639_1_Map.TryGetValue(code, out lang);
                        break;
                    case 3:
                        ISO_639_2_Map.TryGetValue(code, out lang);
                        break;
                }
            }
            return lang;
        }

        #endregion
    }
}
