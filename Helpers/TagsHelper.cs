namespace ShadowViewer.Helpers
{
    public static class TagsHelper
    {
        public static Dictionary<string, ShadowTag> Affiliations = new Dictionary<string,ShadowTag>();
        /// <summary>
        /// 初始化归属标签
        /// </summary>
        public static void Init()
        {
            Affiliations["Local"] = new ShadowTag(I18nHelper.GetString("Shadow.Tag.Local"), "#000000", "#ffd657");
            foreach (string name in PluginHelper.EnabledPlugins)
            {
                Affiliations[name] = PluginHelper.PluginInstances[name].AffiliationTag();
            }
        }
    }
}
