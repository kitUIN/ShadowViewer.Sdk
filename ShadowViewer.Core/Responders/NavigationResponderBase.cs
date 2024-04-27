
using SqlSugar;

namespace ShadowViewer.Responders;

public abstract class NavigationResponderBase: INavigationResponder
{
    public string Id { get; }
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } = new List<IShadowNavigationItem>();
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } = new List<IShadowNavigationItem>();

    public virtual void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type? page,
        ref object? parameter)
    {
        
    }

    public virtual void Navigate(Uri uri, string[] urls)
    {
        
    }

    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected PluginLoader PluginService { get; }
    
    protected PluginBase? Plugin { get; }
    protected NavigationResponderBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginLoader pluginService, string id)
    {
        Caller = callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        PluginService = pluginService;
        Id = id;
        Plugin = PluginService.GetPlugin(Id);
    }
    
}