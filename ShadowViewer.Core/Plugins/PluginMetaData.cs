using System;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Models;
using ShadowViewer.Core.Models.Interfaces;
using ShadowViewer.Core.Responders;

namespace ShadowViewer.Core.Plugins;

/// <summary>
/// 插件管理器使用数据
/// </summary>
public record PluginManage
{
    /// <summary>
    /// 能否开关
    /// </summary>
    [Meta(Required = false)]
    public bool CanSwitch { get; init; } = true;

    /// <summary>
    /// 能否打开文件夹
    /// </summary>
    [Meta(Required = false)]
    public bool CanOpenFolder { get; init; } = true;

    /// <summary>
    /// 设置页面
    /// </summary>
    [Meta(Required = false, EntryPointName = nameof(SettingsPage))]
    public Type? SettingsPage { get; init; }
}

/// <summary>
/// 插件触发器
/// </summary>
public record PluginResponder
{
    /// <summary>
    /// <inheritdoc cref="AbstractHistoryResponder"/>
    /// </summary>
    [Meta(Required = false, EntryPointName = nameof(HistoryResponder))]
    public Type? HistoryResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="AbstractNavigationResponder"/>
    /// </summary>
    [Meta(Required = false, EntryPointName = nameof(NavigationResponder))]
    public Type? NavigationResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="AbstractPicViewResponder"/>
    /// </summary>
    [Meta(Required = false, EntryPointName = nameof(PicViewResponder))]
    public Type? PicViewResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="AbstractSearchSuggestionResponder"/>
    /// </summary>
    [Meta(Required = false, EntryPointName = nameof(SearchSuggestionResponder))]
    public Type? SearchSuggestionResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="ISettingFolder"/>
    /// </summary>
    [Meta(Required = false, EntryPointName = nameof(SettingFolders))]
    public Type[] SettingFolders { get; init; } = [];
}

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