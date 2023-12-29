namespace ShadowViewer.Helpers
{
    internal static class CoreResourcesHelper
    {
        private static readonly string prefix = "ShadowViewer.Core/Resources/";
        public static string GetString(string key)
        {
            return ResourcesHelper.GetString(key,prefix);
        }
        public static string GetString(CoreResourceKey key)
        {
            return ResourcesHelper.GetString(key, prefix);
        }
    }
}
