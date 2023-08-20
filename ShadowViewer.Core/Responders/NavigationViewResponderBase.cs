namespace ShadowViewer.Responders;

public abstract class NavigationViewResponderBase: INavigationViewResponder
{
    public string Id { get; set; } = "";
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } = new List<IShadowNavigationItem>();
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } = new List<IShadowNavigationItem>();

    public abstract void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type page,
        ref object parameter);
}