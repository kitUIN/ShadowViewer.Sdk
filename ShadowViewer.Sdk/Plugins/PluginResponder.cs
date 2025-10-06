using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI.Models;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Responders;
using System;

namespace ShadowViewer.Sdk.Plugins;


/// <summary>
/// 插件触发器
/// </summary>
public record PluginResponder
{
    /// <summary>
    /// <inheritdoc cref="AbstractHistoryResponder"/>
    /// </summary>
    [Meta(Exclude = false)]
    public PluginEntryPointType? HistoryResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="AbstractNavigationResponder"/>
    /// </summary>
    [Meta(Exclude = false)]
    public PluginEntryPointType? NavigationResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="AbstractPicViewResponder"/>
    /// </summary>
    [Meta(Exclude = false)]
    public PluginEntryPointType? PicViewResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="AbstractSearchSuggestionResponder"/>
    /// </summary>
    [Meta(Exclude = false)]
    public PluginEntryPointType? SearchSuggestionResponder { get; init; }

    /// <summary>
    /// <inheritdoc cref="ISettingFolder"/>
    /// </summary>
    [Meta(Exclude = false)]
    public PluginEntryPointType[] SettingFolders { get; init; } = [];
}