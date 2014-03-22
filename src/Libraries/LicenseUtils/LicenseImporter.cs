// Copyright 2014 Andrew C. Dvorak
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
using DotNetUtils;
using LicenseUtils.Properties;

namespace LicenseUtils
{
    public static class LicenseImporter
    {
        private static Works _works;

        public static Works Works
        {
            get { return _works ?? (_works = Import()); }
        }

        public static Works Import()
        {
            var licenseMap = SmartJsonConvert.DeserializeObject<Dictionary<string, License>>(GetResource("licenses_json"));
            var licenses = new List<License>();

            foreach (var id in licenseMap.Keys)
            {
                var license = licenseMap[id];
                license.Id = id;
                license.Text = GetResource(id + "_md");
                license.Html = GetResource(id + "_html");
                licenses.Add(license);
            }

            var works = SmartJsonConvert.DeserializeObject<Works>(GetResource("works_json"));
            foreach (var work in works.All.Where(work => work.LicenseId != null))
            {
                work.License = licenseMap[work.LicenseId];
            }

            return works;
        }

        private static string GetResource(string name)
        {
            var obj = Resources.ResourceManager.GetObject(name, Resources.Culture);

            var str = obj as string;
            if (str != null)
                return str;

            var bytes = obj as byte[];
            if (bytes != null)
                return Encoding.UTF8.GetString(bytes);

            return null;
        }
    }
}
