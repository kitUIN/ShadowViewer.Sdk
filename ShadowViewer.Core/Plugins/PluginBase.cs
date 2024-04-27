using SqlSugar;
using ShadowPluginLoader.WinUI;

namespace ShadowViewer.Plugins;

public abstract partial class PluginBase : AbstractPlugin
{
    protected ILogger Logger { get; }
    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected PluginLoader PluginService { get; }
    public virtual PluginMetaData MetaData { get; }

    protected PluginBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginLoader pluginService,ILogger logger):
        base()
    {
        PluginService = pluginService;
        Caller = callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        Logger = logger;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract LocalTag AffiliationTag { get; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual Type? SettingsPage => null;

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
}