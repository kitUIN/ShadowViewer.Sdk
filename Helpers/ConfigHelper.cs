using System.ComponentModel;
using System.Configuration;

namespace ShadowViewer.Helpers
{
    public class ConfigHelper
    {
        public static readonly bool IsPackaged = true;
        const string Container = "ShadowViewer";
       
        public static bool Contains(string key, string container = Container)
        {
            if (IsPackaged)
            {
                var coreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
                return coreSettings.Values.ContainsKey(key);
            }
            else
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] != null;
            }
        }
        private static object Get(string key, string container = Container)
        {
            if (IsPackaged)
            {
                var coreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
                return coreSettings.Values.TryGetValue(key, out var value) ? value : null;
            }
            else
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key];
            }
        }
        public static void Set(string key, object value, string container = Container)
        {
            if (IsPackaged)
            {
                var coreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
                coreSettings.Values[key] = value;
                Log.ForContext<ConfigHelper>().Information("{Container}[{Key}]={Value}", container, key, value.ToString());
            }
            else
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, Convert.ToString(value));
                }
                else
                {
                    settings[key].Value = Convert.ToString(value);
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                Log.ForContext<ConfigHelper>().Information("[{Key}]={Value}", key, value.ToString());
            }
        }

        public static string GetString(string key, string container = Container)
        {
            return (string)Get(key, container);
        }
        public static bool GetBoolean(string key, string container = Container)
        {
            object res = Get(key, container);
            if (res == null)
            {
                return false;
            }
            return (bool)res;
        }
        public static int GetInt32(string key, string container = Container)
        {
            object res = Get(key, container);
            if (res == null)
            {
                return 0;
            }
            return (int)res;
        }
        public static long GetInt64(string key, string container = Container)
        {
            object res = Get(key, container);
            if (res == null)
            {
                return 0;
            }
            return (long)res;
        }
        public static double GetDouble(string key, string container = Container)
        {
            object res = Get(key, container);
            if (res == null)
            {
                return 0;
            }
            return (double)res;
        }
        public static float GetFloat(string key, string container = Container)
        {
            object res = Get(key, container);
            if (res == null)
            {
                return 0;
            }
            return (float)res;
        }
        public static DateTime GetDateTime(string key, string container = Container)
        {
            object res = Get(key, container);
            if (res == null)
            {
                return default;
            }
            return (DateTime)res;
        }
    }
}
