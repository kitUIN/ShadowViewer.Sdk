
using SqlSugar;

namespace ShadowViewer.Responders;
/// <summary>
/// 导航触发器抽象类
/// </summary>
public abstract class AbstractNavigationResponder : INavigationResponder
{
    /// <inheritdoc />
    public string Id { get; }
    /// <inheritdoc/>
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; } = new List<IShadowNavigationItem>();

    /// <inheritdoc />
    public virtual IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; } = new List<IShadowNavigationItem>();

    /// <inheritdoc />
    public virtual ShadowNavigation? NavigationViewItemInvokedHandler(IShadowNavigationItem item)
    {
        return null;
    }

    /// <inheritdoc />
    public virtual void Navigate(Uri uri, string[] urls)
    {
        
    }

    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected PluginLoader PluginService { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callableService"></param>
    /// <param name="sqlSugarClient"></param>
    /// <param name="compressServices"></param>
    /// <param name="pluginService"></param>
    /// <param name="id"></param>
    protected AbstractNavigationResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginLoader pluginService, string id)
    {
        Caller = callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        PluginService = pluginService;
        Id = id;
    }
    
}