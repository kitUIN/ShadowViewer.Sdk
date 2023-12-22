using DryIoc;
using ShadowViewer.Extensions;
using ShadowViewer.Plugins;
using ShadowViewer.Responders;
using ShadowViewer.Services.Interfaces;
using SqlSugar;

namespace ShadowViewer.Services;

public class PluginService : IPluginService
{
    #region DI
    private ILogger Logger { get; }
    private ICallableService Caller { get; }
    private ISqlSugarClient Db { get; }
    public PluginService(ICallableService callableService, ILogger logger, ISqlSugarClient sqlSugarClient)
    {
        Logger = logger;
        Caller = callableService;
        Db = sqlSugarClient;
    }
    #endregion

    /// <summary>
    /// <inheritdoc/>
    /// </summary>

    private readonly Dictionary<string, IPlugin> plugins = new();

    private readonly Dictionary<string, SortPluginData> tempSortPlugins = new();
    private readonly List<SortPluginData> sortLoader = new();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task ImportFromPluginsPathAsync()
    {
        await GetPathFromPluginsPathAsync(ConfigHelper.GetString("PluginsPath")); 
        var sorted = PluginSortExtension.SortPlugin(sortLoader, x => x.Requires, tempSortPlugins);
        LoadPlugins(sorted);
        tempSortPlugins.Clear();
        sortLoader.Clear();
    }
    public void ImportOnePlugin<T>() where T:IPlugin
    {
        LoadPlugin(typeof(T));
    }
    /// <summary>
    /// 从插件文件夹中获取所有插件路径
    /// </summary>
    private async Task GetPathFromPluginsPathAsync(string path)
    {
        var dir = new DirectoryInfo(path);
        foreach (var item in dir.GetDirectories())
        {
            foreach (var file in item.GetFiles("ShadowViewer.Plugin.*.dll"))
            {
                var asm = await ApplicationExtensionHost.Current.LoadExtensionAsync(file.FullName);
                var t = asm.ForeignAssembly.GetExportedTypes().FirstOrDefault(x => x.IsAssignableTo(typeof(IPlugin)));
                if (t != null)
                {
                    var data = t.GetPluginMetaData();
                    if (data != null)
                    {
                        var sortData = new SortPluginData
                        {
                            Requires = data.Require,
                            Id = data.Id,
                            PluginType = t,
                        };
                        if (!tempSortPlugins.ContainsKey(sortData.Id))
                        {
                            Logger.Warning("[插件管理器]插件{Path}[{ID}({Name})]重复加载",
                            path, data.Id, data.Name);
                            return;
                        }
                        sortLoader.Add(sortData);
                        tempSortPlugins[sortData.Id] = sortData;
                    }
                }
            }
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void PluginEnabled(string id)
    {
        if (plugins.ContainsKey(id))
        {
            plugins[id].IsEnabled = true;
            Logger.Information("[插件管理器]插件[{Id}]启动成功", id);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void PluginDisabled(string id)
    {
        if (plugins.ContainsKey(id))
        {
            plugins[id].IsEnabled = false;
            Logger.Information("[插件管理器]插件[{Id}]禁用成功", id);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool? IsEnabled(string id)
    {
        if (plugins.ContainsKey(id))
        {
            return plugins[id].IsEnabled;
        }
        return null;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void Delete(string id)
    {
        /*        try
                {
                    if (GetPlugin(id) is { } plugin)
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
                }
                catch (Exception ex)
                {
                    Logger.Error("删除插件错误:{E}", ex);
                }

                return false;*/
    }

    private void LoadPlugins(IList<SortPluginData> sortLoader)
    {
        foreach(var data in sortLoader) 
        {
            LoadPlugin(data.PluginType);
        }
    }
    /// <summary>
    /// 加载插件本体类
    /// </summary>
    private void LoadPlugin(Type? plugin)
    {
        if (plugin != null)
        {
            var meta = plugin.GetPluginMetaData();
            if (meta == null)
            {
                Logger.Warning("[插件管理器]插件[{Namespace}]获取不到主类版本信息", plugin.Namespace);
                return;
            }
            if (meta.MinVersion < IPluginService.MinVersion)
            {
                Logger.Warning("[插件管理器]插件[{ID}({Name})]内核版本错误,需要{MinVersion},当前{Version}",
                    meta.Id, meta.Name, IPluginService.MinVersion, meta.MinVersion);
                return;
            }
            if (meta.Require != null)
            {
                var lost = new List<string>();
                foreach (var item in meta.Require)
                {
                    if (!plugins.ContainsKey(item))
                    {
                        lost.Add(item);
                    }
                }
                if (lost.Count > 0)
                {
                    Logger.Warning("[插件控制器]{Name}插件缺少依赖{Lost}", meta.Name, string.Join(",", lost));
                    return;
                }
            }
            DiFactory.Services.Register(typeof(IPlugin), plugin, Reuse.Singleton);
            var instance = DiFactory.Services.ResolveMany<IPlugin>().FirstOrDefault(x =>
                        meta.Id== x.MetaData.Id);
            if (instance is null) return;
            Logger.Information("[插件管理器]插件[{ID}]加载主类成功", meta.Id);
            LoadPluginDI(plugin, meta);
            plugins[meta.Id] = instance;
            var isEnabled = true;
            if (ConfigHelper.Contains(meta.Id))
                isEnabled = ConfigHelper.GetBoolean(meta.Id);
            else
                ConfigHelper.Set(meta.Id, true);
            instance.Loaded(isEnabled);
            Logger.Information("[插件管理器]插件[{ID}({Name})]加载成功", meta.Id,meta.Name);
        }
    }

    /// <summary>
    /// 加载插件依赖注入
    /// </summary>
    private void LoadPluginDI(Type plugin, PluginMetaData meta)
    {
        foreach (var type in plugin.Assembly.GetExportedTypes())
        {
            if (type.IsAssignableTo(typeof(INavigationResponder)))
            {
                DiFactory.Services.Register(typeof(INavigationResponder), type,
                Reuse.Singleton, made: Parameters.Of.Type<string>(_ => meta.Id));
                Logger.Information("[插件管理器]插件[{ID}]加载导航{CLASS}注入成功", meta.Id, type.Name);
            }

            else if (type.IsAssignableTo(typeof(IPicViewResponder)))
            {
                DiFactory.Services.Register(typeof(IPicViewResponder), type,
                Reuse.Singleton, made: Parameters.Of.Type<string>(_ => meta.Id));
                Logger.Information("[插件管理器]插件[{ID}]加载图片页{CLASS}注入成功", meta.Id, type.Name);
            }
            else if (type.IsAssignableTo(typeof(IHistoryResponder)))
            {
                DiFactory.Services.Register(typeof(IHistoryResponder), type,
                Reuse.Singleton, made: Parameters.Of.Type<string>(_ => meta.Id));
                Logger.Information("[插件管理器]插件[{ID}]加载图片页{CLASS}注入成功", meta.Id, type.Name);
            }
            else if (type.IsAssignableTo(typeof(IHistory)))
            {
                Db.CodeFirst.InitTables(type);
                Logger.Information("[插件管理器]插件[{ID}]加载数据库实体类{CLASS}注入成功", meta.Id, type.Name);
            }
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IPlugin? GetPlugin(string id)
    {
        if(plugins.ContainsKey(id)) return plugins[id];
        return null;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public IList<IPlugin> GetEnabledPlugins()
    {
        var res = new List<IPlugin>();
        foreach (var plugin in plugins)
        {
            if (plugin.Value.IsEnabled)
            {
                res.Add(plugin.Value);
            }
        }
        return res;
    }

    public IPlugin? GetEnabledPlugin(string id)
    {
        if(GetPlugin(id)is IPlugin plugin && plugin.IsEnabled) return plugin;
        return null;
    }
}