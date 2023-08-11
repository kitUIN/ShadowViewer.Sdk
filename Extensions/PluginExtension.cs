namespace ShadowViewer.Extensions;

public static class PluginExtension
{
    public static PluginMetaData GetPluginMetaData<T>(this T plugin) where T : IPlugin
    {
        return plugin.GetType().GetTypeInfo().GetCustomAttribute<PluginMetaData>();
    }
    public static PluginMetaData GetPluginMetaData(this Type plugin)  
    {
        return plugin.GetTypeInfo().GetCustomAttribute<PluginMetaData>();
    }
}