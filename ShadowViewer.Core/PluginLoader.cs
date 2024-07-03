using ShadowPluginLoader.WinUI;

namespace ShadowViewer;

/// <summary>
/// ShadowView 插件加载器
/// </summary>
public class PluginLoader : AbstractPluginLoader<PluginMetaData, PluginBase>
{
    public string MinVersion => "2024.7.3";
}
