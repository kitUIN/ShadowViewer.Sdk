namespace ShadowViewer.Plugins;

public abstract partial class PluginBase
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewMenuItems => new List<IShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewFooterItems => new List<IShadowNavigationItem>();

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