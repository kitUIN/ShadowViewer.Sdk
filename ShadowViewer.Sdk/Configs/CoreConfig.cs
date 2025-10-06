using Microsoft.UI.Xaml;
using Serilog;
using ShadowObservableConfig.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Extensions;
using ShadowViewer.Sdk.Services;
using System.IO;
using DryIoc;
using Windows.Storage;

namespace ShadowViewer.Sdk.Configs;

[ObservableConfig(FileName = "core_config")]
public partial class CoreConfig
{
    /// <summary>
    /// 漫画缓存文件夹地址
    /// </summary>
    [ObservableConfigProperty(Description = "漫画缓存文件夹地址")]
    private string comicFolder = "comic";

    /// <summary>
    /// 日志文件夹地址
    /// </summary>
    [ObservableConfigProperty(Description = "日志文件夹地址")]
    private string logFolder = "log";

    /// <summary>
    /// 调试模式
    /// </summary>
    [ObservableConfigProperty(Description = "调试模式")]
    private bool isDebug;
    /// <summary>
    /// 主题
    /// </summary>
    [ObservableConfigProperty(Description = "主题")]
    private ElementTheme theme = ElementTheme.Default;

    /// <summary>
    /// 
    /// </summary>
    public void LaunchLogFolder()
    {
        var defaultPath = ApplicationData.Current.LocalFolder.Path;
        var logFolderPath = Path.Combine(defaultPath, logFolder);
        logFolderPath.LaunchFolderAsync();
    }


    partial void AfterIsDebugChanged(bool oldValue, bool newValue)
    {
        var defaultPath = ApplicationData.Current.LocalFolder.Path;
        if (newValue)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(defaultPath, logFolder, "ShadowViewer.log"),
                    outputTemplate:
                    "{Timestamp:HH:mm:ss.fff} [{Level:u4}] {SourceContext} | {Message:lj} {Exception}{NewLine}",
                    rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            Log.ForContext<CoreConfig>().Debug("调试模式开启");
        }
        else
        {
            Log.ForContext<CoreConfig>().Debug("调试模式关闭");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(Path.Combine(defaultPath, logFolder, "ShadowViewer.log"),
                    outputTemplate:
                    "{Timestamp:HH:mm:ss.fff} [{Level:u4}] {SourceContext} | {Message:lj} {Exception}{NewLine}",
                    rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
        }
    }

    partial void AfterThemeChanged(ElementTheme oldValue, ElementTheme newValue)
    {
        DiFactory.Services.Resolve<ICallableService>().ThemeChanged();
    }
}