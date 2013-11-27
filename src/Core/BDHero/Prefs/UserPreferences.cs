using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDHero.Startup;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace BDHero.Prefs
{
    public delegate void UserPreferenceMutator(UserPreferences userPreferences);

    public interface IPreferenceManager
    {
        UserPreferences Preferences { get; }

        void UpdatePreferences(UserPreferenceMutator mutator);
    }

    public class PreferenceManager : IPreferenceManager
    {
        private const string UserPreferencesFileName = "prefs.json";

        private readonly IDirectoryLocator _directoryLocator;

        private string PreferenceFilePath
        {
            get
            {
                return Path.Combine(_directoryLocator.AppConfigDir, UserPreferencesFileName);
            }
        }

        [UsedImplicitly]
        public PreferenceManager(IDirectoryLocator directoryLocator)
        {
            _directoryLocator = directoryLocator;
        }

        public UserPreferences Preferences
        {
            get
            {
                if (!File.Exists(PreferenceFilePath))
                {
                    return new UserPreferences();
                }

                var json = File.ReadAllText(PreferenceFilePath);

                return JsonConvert.DeserializeObject<UserPreferences>(json);
            }
        }
        public void UpdatePreferences(UserPreferenceMutator mutator)
        {
            var prefs = Preferences;

            mutator(prefs);

            if (!prefs.RecentFiles.RememberRecentFiles)
            {
                prefs.RecentFiles.RecentBDROMPaths.Clear();
            }

            var json = JsonConvert.SerializeObject(prefs, Formatting.Indented);

            File.WriteAllText(PreferenceFilePath, json);
        }
    }

    public class UserPreferences
    {
        [JsonProperty(PropertyName = "plugins")]
        public PluginPreferences Plugins = new PluginPreferences();

        [JsonProperty(PropertyName = "recent_files")]
        public RecentFilePreferences RecentFiles = new RecentFilePreferences();
    }

    public class PluginPreferences
    {
        [JsonProperty(PropertyName = "disabled_plugin_guids")]
        public ISet<Guid> DisabledPluginGuids = new HashSet<Guid>();

        public void SetPluginEnabled(Guid guid, bool isEnabled)
        {
            if (isEnabled)
            {
                EnablePlugin(guid);
            }
            else
            {
                DisablePlugin(guid);
            }
        }

        public void EnablePlugin(Guid guid)
        {
            DisabledPluginGuids.Remove(guid);
        }

        public void DisablePlugin(Guid guid)
        {
            DisabledPluginGuids.Add(guid);
        }
    }

    public class RecentFilePreferences
    {
        [JsonProperty(PropertyName = "remember_recent_files")]
        public bool RememberRecentFiles = true;

        [JsonProperty(PropertyName = "save_only_on_successful_scan")]
        public bool SaveOnlyOnSuccessfulScan = false;

        [JsonProperty(PropertyName = "max_recent_files")]
        public int MaxRecentFiles = 10;

        /// <summary>
        ///     Lower indexes indicated more recent access; higher indexes were used longer ago.
        /// </summary>
        [JsonProperty(PropertyName = "recent_bdrom_paths")]
        public List<string> RecentBDROMPaths = new List<string>();

        public void AddBDROM(string bdromPath)
        {
            RecentBDROMPaths.Remove(bdromPath);
            RecentBDROMPaths.Insert(0, bdromPath);

            var count = RecentBDROMPaths.Count;
            if (count > MaxRecentFiles)
            {
                RecentBDROMPaths.RemoveRange(MaxRecentFiles, count - MaxRecentFiles);
            }
        }
    }
}
