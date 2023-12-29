using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Helpers
{
    public static class ResourcesHelper
    {
        private static readonly ResourceManager resourceManager = new();
        public static string GetString(string key,string prefix)
        {
            return resourceManager.MainResourceMap.GetValue(prefix + key).ValueAsString;
        }
        public static string GetString<T>(T key, string prefix) where T:Enum
        {
            return GetString(key.ToString(), prefix);
        }
    }
}
