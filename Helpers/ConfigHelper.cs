namespace ShadowViewer.Helpers
{
    public partial class ConfigHelper
    {
        private static string container = "ShadowViewer";
        public static bool Contains(string container, string key)
        {
            ApplicationDataContainer CoreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            return CoreSettings.Values.ContainsKey(key);
        }
        private static object Get(string container,string key)
        {
            ApplicationDataContainer CoreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            if (CoreSettings.Values.ContainsKey(key))
            {
                return CoreSettings.Values[key];
            }
            return null;
        }
        public static void Set(string container, string key, object value)
        {
            ApplicationDataContainer CoreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            CoreSettings.Values[key] = value;
            Log.ForContext<ConfigHelper>().Information("{Container}[{Key}]={Value}", container, key, value.ToString());
        }
        public static ApplicationDataCompositeValue CreateDict()
        {
            return new ApplicationDataCompositeValue();
        }
        public static ApplicationDataCompositeValue GetDict(string container, string key)
        {
            ApplicationDataContainer CoreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            if (CoreSettings.Values.ContainsKey(key))
            {
                return CoreSettings.Values[key] as ApplicationDataCompositeValue;
            }
            return null;
        }
        public static void Set(string key, object value)
        {
            Set(container, key, value);
        }
        public static bool Contains(string key)
        {
            return Contains(container, key);
        }
        public static string GetString(string key)
        {
            return GetString(container, key);
        }
        public static bool GetBoolean(string key)
        {
            return GetBoolean(container, key);
        }
        public static int GetInt32(string key)
        {
            return GetInt32(container, key);
        }
        public static ApplicationDataCompositeValue GetDict(string key)
        {
            return GetDict(container, key);
        } 
        public static string GetString(string container, string key)
        {
            return (string)Get(container,key);
        }
        public static bool GetBoolean(string container, string key)
        {
            object res = Get(container, key);
            if (res == null)
            {
                return false;
            }
            return (bool)res;
        }
        public static int GetInt32(string container, string key)
        {
            object res = Get(container, key);
            if (res == null)
            {
                return 0;
            }
            return (int)res;
        }
    }
}
