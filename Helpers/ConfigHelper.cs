using System.Configuration;

namespace ShadowViewer.Helpers
{
    public partial class ConfigHelper
    {
        static string Read(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key];
        }
        static bool HasKey(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] != null;
        }
        static void Write(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
        public static void Set(string key, string value)
        {
            Write(key, value);
        }
        public static void Set(string key, int value)
        {
            Set(key, Convert.ToString(value));
        }
        public static void Set(string key, double value)
        {
            Set(key, Convert.ToString(value));
        }
        public static void Set(string key, float value)
        {
            Set(key, Convert.ToString(value));
        }
        public static void Set(string key, bool value)
        {
            Set(key, Convert.ToString(value));
        }
        public static void Set(string key, long value)
        {
            Set(key, Convert.ToString(value));
        }
        public static void Set(string key, DateTime value)
        {
            Set(key, Convert.ToString(value));
        }
        public static bool Contains(string key)
        {
            return HasKey(key);
        }
        public static string GetString(string key)
        {
            return Read(key);
        }
        public static bool GetBoolean(string key)
        {
            return Convert.ToBoolean(Read(key));
        }
        public static int GetInt32(string key)
        {
            return Convert.ToInt32(Read(key));
        }
        public static long GetInt64(string key)
        {
            return Convert.ToInt64(Read(key));
        }
        public static double GetDouble(string key)
        {
            return Convert.ToDouble(Read(key));
        }
        public static float GetFloat(string key)
        {
            return Convert.ToSingle(Read(key));
        }
        public static DateTime GetDateTime(string key)
        {
            return Convert.ToDateTime(Read(key));
        }
    }
}
