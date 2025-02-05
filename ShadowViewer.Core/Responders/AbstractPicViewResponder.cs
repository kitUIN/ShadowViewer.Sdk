using ShadowPluginLoader.MetaAttributes;
using ShadowViewer.Args;
using ShadowViewer.Services;
using SqlSugar;

namespace ShadowViewer.Responders;
/// <summary>
/// 图片阅读器触发器抽象类
/// </summary>
public abstract partial class AbstractPicViewResponder : IPicViewResponder
{

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
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected ICallableService Caller { get; }
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected ISqlSugarClient Db { get; }
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected CompressService CompressServices { get; }
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected PluginLoader PluginService { get; }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [Autowired]
    public string Id { get; }
}