namespace ShadowViewer.Helpers
{
    public static class TagsHelper
    {
        public static Dictionary<string, ShadowTag> Affiliations = new Dictionary<string,ShadowTag>();
        public static List<ShadowTag> ShadowTags = new List<ShadowTag>();
         
        public static void Init()
        {
            Affiliations["Local"] = new ShadowTag(I18nHelper.GetString("Shadow.Tag.Local"), "#000000", "#ffd657");
            foreach (string name in PluginHelper.EnabledPlugins)
            {
                Affiliations[name] = PluginHelper.PluginInstances[name].AffiliationTag();
            }
            ShadowTags = DBHelper.GetAll<ShadowTag>();
        }
    }
}
