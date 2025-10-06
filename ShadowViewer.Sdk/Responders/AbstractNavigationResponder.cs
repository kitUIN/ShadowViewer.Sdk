using System;
using System.Collections.Generic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Utils;

namespace ShadowViewer.Sdk.Responders;

/// <summary>
/// 导航触发器抽象类
/// </summary>
public abstract partial class AbstractNavigationResponder : INavigationResponder
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [Autowired]
    public string Id { get; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } =
        new List<IShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } =
        new List<IShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual ShadowNavigation? NavigationViewItemInvokedHandler(IShadowNavigationItem item)
    {
        return null;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual ShadowNavigation? Navigate(Uri uri, string[] urls)
    {
        return null;
    }
}