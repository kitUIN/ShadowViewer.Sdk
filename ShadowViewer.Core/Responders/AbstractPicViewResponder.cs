using ShadowPluginLoader.Attributes;
using ShadowViewer.Core.Args;
using ShadowViewer.Core.Services;
using SqlSugar;

namespace ShadowViewer.Core.Responders;

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
    public virtual void CurrentPageIndexChanged(object sender, string affiliation, int oldValue, int newValue)
    {
        
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void PicturesLoadStarting(object sender, PicViewArg arg)
    {
        
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [Autowired]
    public string Id { get; }
}