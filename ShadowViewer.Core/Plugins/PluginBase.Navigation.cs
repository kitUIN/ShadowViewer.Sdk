namespace ShadowViewer.Plugins;

public abstract partial class PluginBase
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IEnumerable<ShadowNavigationItem> NavigationViewMenuItems => new List<ShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IEnumerable<ShadowNavigationItem> NavigationViewFooterItems => new List<ShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual void NavigationViewItemInvokedHandler(object tag, ref Type page, ref object parameter)
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual void Navigate(Uri uri,string[] urls)
    {
    }
}