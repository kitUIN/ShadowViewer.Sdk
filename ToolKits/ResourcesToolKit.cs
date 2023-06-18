namespace ShadowViewer.ToolKits
{
    public static class ResourcesToolKit
    {
        private static ResourceLoader resourceLoader = new ResourceLoader();
        public static string GetString(string key)
        {
            return resourceLoader.GetString(key.Replace(".","/"));
        }
    }
}
