using SqlSugar;

namespace ShadowViewer.Plugins;

public abstract class PluginBase: IPlugin
{
    protected ICallableToolKit Caller { get; }
    protected ISqlSugarClient Db { get; } 
    protected CompressToolKit CompressToolKit { get; }
    protected IPluginsToolKit PluginsToolKit { get; } 
    public PluginBase(ICallableToolKit callableToolKit, ISqlSugarClient sqlSugarClient,
        CompressToolKit compressToolKit, IPluginsToolKit pluginsToolKit)
    {
        Caller= callableToolKit;
        Db = sqlSugarClient;
        CompressToolKit = compressToolKit;
        PluginsToolKit = pluginsToolKit;
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
    public abstract PluginMetaData MetaData { get; }
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