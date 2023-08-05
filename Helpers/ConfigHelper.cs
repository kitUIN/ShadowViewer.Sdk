namespace ShadowViewer.Helpers
{
    public partial class ConfigHelper
    {
        private const string Container = "ShadowViewer";

        public static bool Contains(string container, string key)
        {
            var coreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            return coreSettings.Values.ContainsKey(key);
        }
        private static object Get(string container,string key)
        {
            var coreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            return coreSettings.Values.TryGetValue(key, out var value) ? value : null;
        }
        public static void Set(string container, string key, object value)
        {
            var coreSettings = ApplicationData.Current.LocalSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            coreSettings.Values[key] = value;
            Log.ForContext<ConfigHelper>().Information("{Container}[{Key}]={Value}", container, key, value.ToString());
        }
        
        public static void Set(string key, object value)
        {
            Set(Container, key, value);
        }
        public static bool Contains(string key)
        {
            return Contains(Container, key);
        }
        public static string GetString(string key)
        {
            return GetString(Container, key);
        }
        public static bool GetBoolean(string key)
        {
            return GetBoolean(Container, key);
        }
        public static int GetInt32(string key)
        {
            return GetInt32(Container, key);
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
