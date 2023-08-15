namespace ShadowViewer.Extensions;

public static class PluginExtension
{
    public static PluginMetaData GetPluginMetaData<T>(this T plugin) where T : IPlugin
    {
        return plugin.GetType().GetTypeInfo().GetCustomAttribute<PluginMetaData>();
    }
    public static PluginMetaData GetPluginMetaData(this Type plugin)  
    {
        var meta = plugin.GetTypeInfo().GetCustomAttribute<PluginMetaData>();
        if (meta.Logo.StartsWith("/") && meta.Logo != "/")
        {
            meta.Logo = meta.Logo.AssetPath(plugin);
        }
        return meta;
    }
}