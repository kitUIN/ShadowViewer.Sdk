using ShadowViewer.Extensions;
using SqlSugar;

namespace ShadowViewer.Plugins;

public abstract class PluginBase: IPlugin
{
    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; } 
    protected CompressService CompressServices { get; }
    protected IPluginService PluginService { get; }
    public PluginMetaData MetaData { get; } 
    public PluginBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, IPluginService pluginService)
    {
        MetaData = this.GetPluginMetaData();
        Caller= callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        PluginService = pluginService;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual void Loaded(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract LocalTag AffiliationTag { get; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract Type SettingsPage { get; }
    private bool enabled;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsEnabled
    {
        get => enabled;
        set
        {
            if (enabled != value)
            {
                enabled = value;
                ConfigHelper.Set(MetaData.Id, value);
                if (IsEnabled)
                {
                    PluginEnabled();
                    Caller.PluginEnabled(this,MetaData.Id,IsEnabled);
                }
                else
                {
                    PluginDisabled();
                    Caller.PluginDisabled(this,MetaData.Id,IsEnabled);
                }
            }
        } 
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IList<ShadowNavigationItem> NavigationViewMenuItems => new List<ShadowNavigationItem>();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IList<ShadowNavigationItem> NavigationViewFooterItems => new List<ShadowNavigationItem>();
 
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract void NavigationViewItemInvokedHandler(object tag, ref Type page, ref object parameter);
    /// <summary>
    /// 插件启动后触发
    /// </summary>
    protected abstract void PluginEnabled();
    /// <summary>
    /// 插件禁用后触发
    /// </summary>
    protected abstract void PluginDisabled();
}