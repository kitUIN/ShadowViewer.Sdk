using ShadowPluginLoader.Attributes;

namespace ShadowViewer.Core.Enums;

/// <summary>
/// 核心设置
/// </summary>
[ShadowSettingClass(ClassName = "CoreSettings")]
public enum CoreSettings
{
    /// <summary>
    /// 漫画缓存文件夹地址
    /// </summary>
    [ShadowSetting(typeof(string), "Comics", comment: "漫画缓存文件夹地址", isPath: true,
        baseFolder: SettingsBaseFolder.LocalFolder)]
    ComicsPath,

    /// <summary>
    /// 插件文件夹地址
    /// </summary>
    [ShadowSetting(typeof(string), "Plugins", comment: "插件文件夹地址", isPath: true,
        baseFolder: SettingsBaseFolder.LocalFolder)]
    PluginsPath,

    /// <summary>
    /// 临时文件夹地址
    /// </summary>
    [ShadowSetting(typeof(string), "Temps", comment: "临时文件夹地址", isPath: true,
        baseFolder: SettingsBaseFolder.LocalFolder)]
    TempPath,

    /// <summary>
    /// 调试模式
    /// </summary>
    [ShadowSetting(typeof(bool), "false", comment: "调试模式")]
    IsDebug,
}