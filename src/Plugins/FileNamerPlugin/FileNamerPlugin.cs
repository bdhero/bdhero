using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BDHero.JobQueue;

namespace BDHero.Plugin.FileNamer
{
    public class FileNamerPlugin : INameProviderPlugin
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Preferences _prefs;

        internal Preferences Preferences
        {
            get { return _prefs ?? (_prefs = GetPreferences()); }
        }

        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "BDHero File Namer"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return null; } }

        public int RunOrder { get { return 0; } }

        public EditPluginPreferenceHandler EditPreferences
        {
            get { return EditPluginPreferenceHandler; }
        }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        // TODO: Write full implementation
        public void Rename(CancellationToken cancellationToken, Job job)
        {
            Host.ReportProgress(this, 0.0, "Auto-renaming output file...");

            var namer = new FileNamer(job, Preferences);

            job.OutputPath = namer.GetPath().FullName;

            Host.ReportProgress(this, 100.0, "Finished auto-renaming output file");
        }

        private Preferences GetPreferences()
        {
            return PluginUtils.GetPreferences(AssemblyInfo, () => new Preferences());
        }

        private void SavePreferences(Preferences prefs)
        {
            PluginUtils.SavePreferences(AssemblyInfo, prefs);
        }

        private DialogResult EditPluginPreferenceHandler(Form parent)
        {
            // FormFileNamerPreferences automatically copies the user's changes to
            // Preferences if the user hits "Save".
            var result = new FormFileNamerPreferences(Preferences).ShowDialog(parent);

            if (result == DialogResult.OK)
            {
                SavePreferences(Preferences);
            }

            return result;
        }
    }
}
