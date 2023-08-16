namespace ShadowViewer.Extensions;

public static class PluginExtension
{
    public static PluginMetaData GetPluginMetaData<T>() where T:IPlugin
    {
        return typeof(T).GetPluginMetaData();
    }
    public static PluginMetaData GetPluginMetaData(this Type plugin)  
    {
        var meta = plugin.GetTypeInfo().GetCustomAttribute<PluginMetaData>();
        if (meta == null) return null;
        if (!string.IsNullOrEmpty(meta.Logo))
        {
            if (meta.Logo.StartsWith("/") && meta.Logo != "/")
            {
                meta.Logo = meta.Logo.AssetPath(plugin);
            }else if (meta.Logo.StartsWith("ms-appx:///"))
            {
                meta.Logo = meta.Logo.Replace("ms-appx://","").AssetPath(plugin);
            }else if (meta.Logo.StartsWith("ms-appx://"))
            {
                meta.Logo = meta.Logo.Replace("ms-appx://","/").AssetPath(plugin);
            }
        }
        return meta;
    }
}