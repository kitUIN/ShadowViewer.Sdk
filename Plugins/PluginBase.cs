using SqlSugar;

namespace ShadowViewer.Plugins;

public abstract class PluginBase: IPlugin
{
    protected ICallableToolKit Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressToolKit CompressToolKit { get; }
    protected IPluginsToolKit PluginsToolKit { get; }
    protected PluginBase()
    {
        Caller = DiFactory.Current.Services.GetService<ICallableToolKit>();
        Db = DiFactory.Current.Services.GetService<ISqlSugarClient>();
        PluginsToolKit = DiFactory.Current.Services.GetService<IPluginsToolKit>();
        CompressToolKit = DiFactory.Current.Services.GetService<CompressToolKit>();
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
    public abstract void NavigationViewMenuItemsHandler(ObservableCollection<NavigationViewItem> menus);

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract void NavigationViewFooterItemsHandler(ObservableCollection<NavigationViewItem> menus);
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