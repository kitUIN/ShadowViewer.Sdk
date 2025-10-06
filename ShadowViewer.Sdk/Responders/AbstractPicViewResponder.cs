using ShadowPluginLoader.Attributes;

namespace ShadowViewer.Sdk.Responders;

/// <summary>
/// 图片阅读器触发器抽象类
/// </summary>
public abstract partial class AbstractPicViewResponder : IPicViewResponder
{

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void CurrentEpisodeIndexChanged(object sender, PicViewContext ctx, int oldValue, int newValue)
    {
        
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void CurrentPageIndexChanged(object sender, PicViewContext ctx, int oldValue, int newValue)
    {
        
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual void PicturesLoadStarting(object sender, PicViewContext ctx)
    {
        
    }
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    [Autowired]
    public string Id { get; }
}