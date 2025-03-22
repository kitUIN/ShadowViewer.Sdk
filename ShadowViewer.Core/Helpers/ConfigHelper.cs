using System;
using System.Configuration;
using Windows.Storage;
using Serilog;

namespace ShadowViewer.Core.Helpers;

/// <summary>
/// 
/// </summary>
public class ConfigHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly bool IsPackaged = true;

    private const string Container = "ShadowViewer";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    public static void Set<T>(T key, object value, string container = Container) where T : Enum
    {
        Set(key.ToString(), value, container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="container"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static string? GetString(string key, string container = Container)
    {
        return (string?)Get(key, container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string? GetString<T>(T key, string container = Container) where T : Enum
    {
        return GetString(key.ToString(), container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static bool GetBoolean(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return false;
        return (bool)res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool GetBoolean<T>(T key, string container = Container) where T : Enum
    {
        return GetBoolean(key.ToString(), container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static int GetInt32(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (int)res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int GetInt32<T>(T key, string container = Container) where T : Enum
    {
        return GetInt32(key.ToString(), container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static long GetInt64(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (long)res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static long GetInt64<T>(T key, string container = Container) where T : Enum
    {
        return GetInt64(key.ToString(), container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static double GetDouble(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (double)res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static double GetDouble<T>(T key, string container = Container) where T : Enum
    {
        return GetDouble(key.ToString(), container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static float GetFloat(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return 0;
        return (float)res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static float GetFloat<T>(T key, string container = Container) where T : Enum
    {
        return GetFloat(key.ToString(), container);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static DateTime GetDateTime(string key, string container = Container)
    {
        var res = Get(key, container);
        if (res == null) return default;
        return (DateTime)res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="container"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static DateTime GetDateTime<T>(T key, string container = Container) where T : Enum
    {
        return GetDateTime(key.ToString(), container);
    }
}