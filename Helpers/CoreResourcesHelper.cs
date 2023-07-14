using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Helpers
{
    public static class CoreResourcesHelper
    {
        private static readonly ResourceManager resourceManager = new ResourceManager();
        private static readonly string prefix = "ShadowViewer.Core/Resources/";
        public static string GetString(string key)
        {
            return resourceManager.MainResourceMap.GetValue(prefix + key.Replace(".", "/")).ValueAsString;
        }
        public static string GetString(CoreResourceKey key)
        {
            return GetString(key.ToString());
        }
    }
}
