using ShadowViewer.Extensions;

namespace ShadowViewer.Services;

public class PluginService : IPluginService
{
    private ILogger Logger { get; }
    public const int MinVersion = 20230808;
    private ICallableService Caller { get; }
    private Queue<string> PluginQueue { get; } = new Queue<string>();

    /// <summary>
    /// 所有插件
    /// </summary>
    private ObservableCollection<IPlugin> Instances { get; } = new();

    public PluginService(ICallableService callableService, ILogger logger)
    {
        Logger = logger;
        Caller = callableService;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task ImportAsync()
    {
        var path = ConfigHelper.GetString("PluginsPath");
        var dir = new DirectoryInfo(path);
        foreach (var item in dir.GetDirectories())
        foreach (var file in item.GetFiles("ShadowViewer.Plugin.*.dll"))
        {
            PluginQueue.Enqueue(file.FullName);
        }

        while (PluginQueue.Count != 0)
        {
            var p = PluginQueue.Dequeue();
            await ImportAsync(p);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task ImportAsync(string path)
    {
        var asm = await ApplicationExtensionHost.Current.LoadExtensionAsync(path);
        foreach (var instance in asm.ForeignAssembly.GetExportedTypes()
                     .Where(type => type.IsAssignableTo(typeof(IPlugin))))
            Import(instance);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Import(Type type)
    {
        try
        {
            if (!type.IsAssignableTo(typeof(IPlugin))) return;
            var meta = type.GetPluginMetaData();
            if (meta.MinVersion < MinVersion)
            {
                Logger.Error("[插件控制器]{Name}插件版本有误(所需>={MinVersion},当前:{Meta})", meta.Name, MinVersion,
                    meta.MinVersion);
                return;
            }
            var plugins = DiFactory.Services.ResolveMany<IPlugin>();
            if (meta.Require == null || meta.Require.Length == 0 || meta.Require != null && meta.Require.All(x=>plugins.Any(y=>y.MetaData.Id.Equals(x,StringComparison.OrdinalIgnoreCase))))
            {
                if (plugins.FirstOrDefault(x => meta.Id.Equals(x.MetaData.Id, StringComparison.OrdinalIgnoreCase)) is IPlugin p)
                {
                    Logger.Warning("[插件控制器]{Name}插件重复加载", meta.Name);
                }
                else
                {
                    DiFactory.Services.Register(typeof(IPlugin), type, Reuse.Singleton);
                    var plugin = DiFactory.Services.ResolveMany<IPlugin>().FirstOrDefault(x => meta.Id.Equals(x.MetaData.Id, StringComparison.OrdinalIgnoreCase));
                    if (plugin is null) return;
                    Instances.Add(plugin);
                    var isEnabled = true;
                    if (ConfigHelper.Contains(plugin.MetaData.Id))
                        isEnabled = ConfigHelper.GetBoolean(meta.Id);
                    else
                        ConfigHelper.Set(plugin.MetaData.Id, true);
                    plugin.Loaded(isEnabled);
                    Logger.Information("[插件控制器]加载{Name}插件成功", plugin.MetaData.Name);
                }
            }
            else
            {
                var lost = meta.Require.Where(require => !plugins.Any(y => y.MetaData.Id.Equals(require, StringComparison.OrdinalIgnoreCase)));
                Logger.Warning("[插件控制器]{Name}插件缺少依赖{Lost}", meta.Name,string.Join(",",lost));
            }
            
        }catch(Exception ex)
        {
            Logger.Error("[插件控制器]插件加载出错:{E}", ex);
        }
        
    }

    public void Import<T>() where T : IPlugin
    {
        Import(typeof(T));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void PluginEnabled(string id)
    {
        if (GetPlugin(id) is not { } plugin) return;
        plugin.IsEnabled = true;
        Logger.Information("插件{Id}启动成功", id);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void PluginDisabled(string id)
    {
        if (GetPlugin(id) is not { } plugin) return;
        plugin.IsEnabled = false;
        Logger.Information("插件{Id}禁用成功", id);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IPlugin? GetPlugin(string id)
    {
        return Instances.FirstOrDefault(x => id.Equals(x.MetaData.Id,StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IEnumerable<IPlugin> EnabledPlugins => Instances.Where(x => x.IsEnabled);

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public LocalTag GetAffiliationTag(string id)
    {
        return id == "Local"
            ? new LocalTag(CoreResourcesHelper.GetString(CoreResourceKey.LocalTag), "#000000", "#ffd657")
            : GetPlugin(id)?.AffiliationTag;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public ObservableCollection<IPlugin> Plugins => Instances;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IPlugin GetEnabledPlugin(string id)
    {
        return Instances.FirstOrDefault(x => id.Equals(x.MetaData.Id,StringComparison.OrdinalIgnoreCase) && x.IsEnabled);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            if (GetPlugin(id) is IPlugin plugin)
            {
                var file = await plugin.GetType().Assembly.Location.GetFile();
                var folder = await file.GetParentAsync();
                plugin.IsEnabled = false;
                plugin.PluginDeleting();
                Instances.Remove(plugin);
                //ApplicationExtensionHost.Current.
                await folder.DeleteAsync();
                return true;
            }
        }catch(Exception ex)
        {
            Logger.Error("删除插件错误:{E}", ex);
        }
        return false;
    }
}