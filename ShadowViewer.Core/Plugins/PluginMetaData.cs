using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Models;

namespace ShadowViewer.Core.Plugins;

/// <summary>
/// 插件元数据
/// </summary>
[ExportMeta]
public record PluginMetaData : AbstractPluginMetaData
{
    /// <summary>
    /// 介绍
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    /// 作者
    /// </summary>
    public string Authors { get; init; } = null!;

    /// <summary>
    /// 项目地址
    /// </summary>
    public string WebUri { get; init; } = null!;

    /// <summary>
    /// 图标<br/>
    /// 1.本地文件,以ms-appx://开头<br/>
    /// 2.FontIcon,以font://开头<br/>
    /// 3.FluentRegularIcon,以fluent://regular开头
    /// 4.FluentFilledIcon,以fluent://filled
    /// <example><br/>
    /// 1.ms-appx:///Assets/Icons/Logo.png<br/>
    /// 2.font://\uE714<br/>
    /// 3.fluent://regular/Apps
    /// 4.fluent://filled/Apps
    /// </example>
    /// </summary>
    public string Logo { get; init; } = null!;

    /// <summary>
    /// <inheritdoc cref="Plugins.PluginManage"/>
    /// </summary>
    [Meta(Required = false)]
    public PluginManage PluginManage { get; init; } = new();

    /// <summary>
    /// <inheritdoc cref="Plugins.PluginResponder"/>
    /// </summary>
    [Meta(Required = false)]
    public PluginResponder PluginResponder { get; init; } = new();

    /// <summary>
    /// 分类标签
    /// </summary>
    [Meta(Required = false)]
    public ShadowTag? AffiliationTag { get; init; }

    /// <summary>
    /// Core 最低支持版本号
    /// </summary>
    public string CoreVersion { get; init; } = null!;
}