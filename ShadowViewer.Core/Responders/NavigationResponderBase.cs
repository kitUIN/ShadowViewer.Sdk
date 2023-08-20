using SqlSugar;

namespace ShadowViewer.Responders;

public abstract class NavigationResponderBase: INavigationResponder
{
    public string Id { get; set; }
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } = new List<IShadowNavigationItem>();
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } = new List<IShadowNavigationItem>();

    public virtual void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type? page,
        ref object? parameter)
    {
        
    }

    public virtual void Navigate(Uri uri, string[] urls)
    {
        
    }

    protected ICallableService callerService;
    protected ISqlSugarClient db;
    protected CompressService compressServices;
    protected PluginService pluginService;
    protected NavigationResponderBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginService pluginService,string id)
    {
        this.callerService = callableService;
        this.db = sqlSugarClient;
        this.compressServices = compressServices;
        this.pluginService = pluginService;
        this.Id = id;
    }
    
}