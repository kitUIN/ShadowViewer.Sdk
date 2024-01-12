
using SqlSugar;

namespace ShadowViewer.Responders;

public abstract class HistoryResponderBase:IHistoryResponder
{
    public string Id { get; }
    public abstract IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day);

    public abstract void ClickHistoryHandler(IHistory history);
    
    public abstract void DeleteHistoryHandler(IHistory history);

    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected IPluginService PluginService { get; }
    
    protected IPlugin? Plugin { get; }
    protected HistoryResponderBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, IPluginService pluginService,string id)
    {
        Caller = callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        PluginService = pluginService;
        Id = id;
        Plugin = PluginService.GetPlugin(Id);
    }
}