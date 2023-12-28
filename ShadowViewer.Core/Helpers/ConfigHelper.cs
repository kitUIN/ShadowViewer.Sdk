using System.Configuration;

namespace ShadowViewer.Helpers;

public class ConfigHelper
{
    public static readonly bool IsPackaged = true;
    private const string Container = "ShadowViewer";

    public static bool Contains(string key, string container = Container)
    {
        if (IsPackaged)
        {
            var coreSettings =
                ApplicationData.Current.LocalSettings.CreateContainer(container,
                    ApplicationDataCreateDisposition.Always);
            return coreSettings.Values.ContainsKey(key);
        }
        else
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key] != null;
        }
    }

    private static object? Get(string key, string container = Container)
    {
        if (IsPackaged)
        {
            var coreSettings =
                ApplicationData.Current.LocalSettings.CreateContainer(container,
                    ApplicationDataCreateDisposition.Always);
            return coreSettings.Values.TryGetValue(key, out var value) ? value : null;
        }
        else
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key];
        }
    }
    public static void Set<T>(T key, object value, string container = Container)where T:Enum
    {
        Set(key.ToString(), value, container);
    }
    public static void Set(string key, object value, string container = Container)
    {
        if (IsPackaged)
        {
            var coreSettings =
                ApplicationData.Current.LocalSettings.CreateContainer(container,
                    ApplicationDataCreateDisposition.Always);
            coreSettings.Values[key] = value;
            Log.ForContext<ConfigHelper>().Debug("{Container}[{Key}]={Value}", container, key, value.ToString());
        }
        else
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
                settings.Add(key, Convert.ToString(value));
            else
                settings[key].Value = Convert.ToString(value);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            Log.ForContext<ConfigHelper>().Debug("[{Key}]={Value}", key, value.ToString());
        }
    }

    public static string? GetString(string key, string container = Container)
    {
        return (string?)Get(key, container);
    }
    public static string? GetString<T>(T key, string container = Container) where T : Enum
    {
        return GetString(key.ToString(), container);
    }
    public static bool GetBoolean(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return false;
        return (bool)res;
    }
    public static bool GetBoolean<T>(T key, string container = Container)where T : Enum
    {
        return GetBoolean(key.ToString(), container);
    }
    public static int GetInt32(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (int)res;
    }
    public static int GetInt32<T>(T key, string container = Container) where T : Enum
    {
        return GetInt32(key.ToString(), container);
    }
    public static long GetInt64(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (long)res;
    }
    public static long GetInt64<T>(T key, string container = Container) where T : Enum
    {
        return GetInt64(key.ToString(), container);
    }
    public static double GetDouble(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (double)res;
    }
    public static double GetDouble<T>(T key, string container = Container) where T : Enum
    {
        return GetDouble(key.ToString(), container);
    }
    public static float GetFloat(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (float)res;
    }
    public static float GetFloat<T>(T key, string container = Container) where T : Enum
    {
        return GetFloat(key.ToString(), container);
    }
    public static DateTime GetDateTime(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return default;
        return (DateTime)res;
    }
    public static DateTime GetDateTime<T>(T key, string container = Container) where T : Enum
    {
        return GetDateTime(key.ToString(), container);
    }
}