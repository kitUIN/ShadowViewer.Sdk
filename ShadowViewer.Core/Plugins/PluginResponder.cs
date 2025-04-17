using System;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Core.Models.Interfaces;
using ShadowViewer.Core.Responders;

namespace ShadowViewer.Core.Plugins;


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