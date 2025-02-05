using ShadowPluginLoader.MetaAttributes;
using SqlSugar;

namespace ShadowViewer.Responders;
/// <summary>
/// 导航触发器抽象类
/// </summary>
public abstract partial class AbstractNavigationResponder : INavigationResponder
{
    /// <inheritdoc />
    [Autowired]
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
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected ICallableService Caller { get; }
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected ISqlSugarClient Db { get; }
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected CompressService CompressServices { get; }
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected PluginLoader PluginService { get; }
    
}