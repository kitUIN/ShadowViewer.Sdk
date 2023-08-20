using ShadowViewer.Extensions;
using SqlSugar;

namespace ShadowViewer.Plugins;

public abstract partial class PluginBase : IPlugin
{
    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected PluginService PluginService { get; }
    public abstract PluginMetaData MetaData { get; }

    protected PluginBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginService pluginService)
    {
        foreach (var item in ResourceDictionaries)
        {
            Application.Current.Resources.MergedDictionaries.Add(item);
        }
        Caller = callableService;
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
    public virtual Type? SettingsPage => null;

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
                    Caller.PluginEnabled(this, MetaData.Id, IsEnabled);
                }
                else
                {
                    PluginDisabled();
                    Caller.PluginDisabled(this, MetaData.Id, IsEnabled);
                }
            }
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual bool CanSwitch { get; } = true;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual bool CanDelete { get; } = true;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual bool CanOpenFolder { get; } = true;
    
    /// <summary>
    ///  <inheritdoc/>
    /// </summary>
    public abstract void PluginDeleting();
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual IEnumerable<ResourceDictionary> ResourceDictionaries => new List<ResourceDictionary>();

    /// <summary>
    /// 插件启动后触发
    /// </summary>
    protected abstract void PluginEnabled();

    /// <summary>
    /// 插件禁用后触发
    /// </summary>
    protected abstract void PluginDisabled();
}