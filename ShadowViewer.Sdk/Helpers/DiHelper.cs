using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Checkers;

using System.IO;
using Windows.Storage;
using ShadowViewer.Sdk.Database;
using ShadowViewer.Sdk.Plugins;

namespace ShadowViewer.Sdk.Helpers;

/// <summary>
/// 依赖注入帮助类
/// </summary>
public static class DiHelper
{
    /// <summary>
    /// 初始化DI
    /// </summary>
    public static void Init()
    {
        var defaultPath = ApplicationData.Current.LocalFolder.Path;
        DatabaseRegistration.Register<ShadowDbContext>(DiFactory.Services, Path.Combine(defaultPath, "ShadowViewer.sqlite"),
            "__EFMigrationsHistory_Sdk", options => new ShadowDbContext(options));
        DiFactory.Init<AShadowViewerPlugin, PluginMetaData>();
        DiFactory.RegisterPluginLoader<PluginLoader>();
    }
}
