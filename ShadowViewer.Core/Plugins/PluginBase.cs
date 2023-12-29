using Serilog.Core;
using ShadowViewer.Extensions;
using ShadowViewer.Services.Interfaces;
using SqlSugar;

namespace ShadowViewer.Plugins;

public abstract partial class PluginBase : IPlugin
{
    protected ILogger Logger { get; }
    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected IPluginService PluginService { get; }
    public virtual PluginMetaData MetaData { get; }

    protected PluginBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, IPluginService pluginService,ILogger logger)
    {
        MetaData = this.GetType().GetPluginMetaData();
        foreach (var item in ResourceDictionaries)
        {
            Application.Current.Resources.MergedDictionaries.Add(item);
        }
        Caller = callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        PluginService = pluginService;
        Logger = logger;
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