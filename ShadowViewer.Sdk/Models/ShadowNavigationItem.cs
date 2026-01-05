using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Navigation;

namespace ShadowViewer.Sdk.Models;

/// <summary>
/// 
/// </summary>
public partial class ShadowNavigationItem : ObservableObject, IShadowNavigationItem
{
    /// <summary />
    /// <param name="pluginId"></param>
    /// <param name="id"></param>
    /// <param name="icon"></param>
    /// <param name="content"></param>
    public ShadowNavigationItem(string pluginId, string id, ShadowUri uri, IconElement? icon, object? content)
    {
        Icon = icon;
        Id = id;
        Uri = uri;
        Content = content;
        PluginId = pluginId;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }

    [ObservableProperty] public partial IconElement? Icon { get; set; }


    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string? Id { get; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public ShadowUri? Uri { get; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string PluginId { get; }
}