namespace ShadowViewer.Responders;

public abstract class NavigationResponderBase: INavigationResponder
{
    public string Id { get; set; } = "";
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } = new List<IShadowNavigationItem>();
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } = new List<IShadowNavigationItem>();

    public virtual void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type? page,
        ref object? parameter)
    {
        
    }

    public virtual void Navigate(Uri uri, string[] urls)
    {
        
    }

    protected NavigationResponderBase(string id)
    {
        Id = id;
    }
    
}