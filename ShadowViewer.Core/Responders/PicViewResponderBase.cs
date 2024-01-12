
using SqlSugar;

namespace ShadowViewer.Responders;

public abstract class PicViewResponderBase:IPicViewResponder
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void CurrentEpisodeIndexChanged(object sender, string affiliation, int oldValue, int newValue)
    {
        
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void PicturesLoadStarting(object sender, PicViewArg arg)
    {
        
    }
    protected ICallableService Caller { get; }
    protected ISqlSugarClient Db { get; }
    protected CompressService CompressServices { get; }
    protected IPluginService PluginService { get; }
    
    protected IPlugin? Plugin { get; }
    protected PicViewResponderBase(ICallableService callableService, ISqlSugarClient sqlSugarClient,
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