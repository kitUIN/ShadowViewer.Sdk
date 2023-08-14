namespace ShadowViewer.Helpers
{
    public static class CoreResourcesHelper
    {
        private static readonly ResourceManager resourceManager = new ResourceManager();
        private static readonly string prefix = "ShadowViewer.Core/Resources/";
        public static string GetString(string key)
        {
            return resourceManager.MainResourceMap.GetValue(prefix + key).ValueAsString;
        }
        public static string GetString(CoreResourceKey key)
        {
            return GetString(key.ToString());
        }
    }
}
