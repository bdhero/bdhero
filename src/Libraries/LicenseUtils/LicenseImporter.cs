using System.Collections.Generic;
using System.Linq;
using System.Text;
using LicenseUtils.Properties;
using Newtonsoft.Json;

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
            var licenseMap = JsonConvert.DeserializeObject<Dictionary<string, License>>(GetResource("licenses_json"));
            var licenses = new List<License>();

            foreach (var id in licenseMap.Keys)
            {
                var license = licenseMap[id];
                license.Id = id;
                license.Text = GetResource(id + "_md");
                license.Html = GetResource(id + "_html");
                licenses.Add(license);
            }

            var works = JsonConvert.DeserializeObject<Works>(GetResource("works_json"));
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
