using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using LicenseUtils.Properties;
using MarkdownSharp;
using Newtonsoft.Json;

namespace LicenseUtils
{
    public class LicenseImporter
    {
        public void Import()
        {
            var licenseMap = JsonConvert.DeserializeObject<Dictionary<string, License>>(GetResource("licenses"));
            var licenses = new List<License>();
            foreach (var id in licenseMap.Keys)
            {
                var license = licenseMap[id];
                license.Id = id;
                license.Text = GetResource(id);
                license.Html = new Markdown().Transform(license.Text);
                licenses.Add(license);
            }
            var works = JsonConvert.DeserializeObject<Works>(GetResource("works"));
            foreach (var work in works.All.Where(work => work.LicenseId != null))
            {
                work.License = licenseMap[work.LicenseId];
                Console.WriteLine(work);
                Console.WriteLine();
            }
        }

        private string GetResource(string name)
        {
            var bytes = (byte[]) Resources.ResourceManager.GetObject(name, Resources.Culture);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
