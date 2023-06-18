using System.Diagnostics;

namespace ShadowViewer.ToolKits
{
    public class BikaResourcesToolKit: IResourcesToolKit
    {
        private ResourceManager resourceManager;
        private string prefix;
        public BikaResourcesToolKit()
        {
            resourceManager = new ResourceManager();
            prefix = "ShadowViewer.Plugin.Bika/Resources/";
        }
        public string GetString(string key)
        {
            return resourceManager.MainResourceMap.GetValue(this.prefix + key.Replace(".", "/")).ValueAsString;
        }
    }
}
