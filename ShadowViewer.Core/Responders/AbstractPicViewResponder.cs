
using SqlSugar;

namespace ShadowViewer.Responders;

public abstract class AbstractPicViewResponder : IPicViewResponder
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
    protected PluginLoader PluginService { get; }
    /// <summary>
    /// Plugin
    /// </summary>
    protected AShadowViewerPlugin? Plugin => PluginService.GetPlugin(Id);
    protected AbstractPicViewResponder(ICallableService callableService, ISqlSugarClient sqlSugarClient,
        CompressService compressServices, PluginLoader pluginService,string id)
    {
        Caller = callableService;
        Db = sqlSugarClient;
        CompressServices = compressServices;
        PluginService = pluginService;
        Id = id;
    }
}