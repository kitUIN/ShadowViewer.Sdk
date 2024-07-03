using SqlSugar;
using ShadowPluginLoader.WinUI;

namespace ShadowViewer.Plugins;

public abstract partial class PluginBase : AbstractPlugin
{
    /// <summary>
    /// 日志服务
    /// </summary>
    protected ILogger Logger { get; }
    /// <summary>
    /// 通知服务
    /// </summary>
    protected ICallableService Caller { get; }
    /// <summary>
    /// 数据库服务
    /// </summary>
    protected ISqlSugarClient Db { get; }
    /// <summary>
    /// 压缩服务
    /// </summary>
    protected CompressService CompressServices { get; }
    /// <summary>
    /// 插件服务
    /// </summary>
    protected PluginLoader PluginService { get; }
    /// <summary>
    /// 插件元数据
    /// </summary>
    public abstract PluginMetaData MetaData { get; }

    /// <inheritdoc />
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