using SqlSugar;

namespace ShadowViewer.Plugins;

public abstract class PluginBase: IPlugin
{
    protected ICallableToolKit Caller { get; } = DiFactory.Current.Services.GetService<ICallableToolKit>();
    protected ISqlSugarClient Db { get; } = DiFactory.Current.Services.GetService<ISqlSugarClient>();
    protected CompressToolKit CompressToolKit { get; } = DiFactory.Current.Services.GetService<CompressToolKit>();
    protected IPluginsToolKit PluginsToolKit { get; } = DiFactory.Current.Services.GetService<IPluginsToolKit>();

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