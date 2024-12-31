
using SqlSugar;

namespace ShadowViewer.Responders;

public abstract class AbstractHistoryResponder : IHistoryResponder
{
    public string Id { get; }
    public abstract IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day);

    public abstract void ClickHistoryHandler(IHistory history);
    
    public abstract void DeleteHistoryHandler(IHistory history);

    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected PluginLoader PluginService { get; }
    /// <summary>
    /// Plugin
    /// </summary>
    protected AShadowViewerPlugin? Plugin => PluginService.GetPlugin(Id);
    protected AbstractHistoryResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginLoader pluginService,string id)
    {
        Caller = callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        PluginService = pluginService;
        Id = id;
    }
}