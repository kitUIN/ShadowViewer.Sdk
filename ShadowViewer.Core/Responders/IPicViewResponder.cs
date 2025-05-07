using ShadowViewer.Core.Args;

namespace ShadowViewer.Core.Responders;

/// <summary>
/// 图片阅读器触发器基类
/// </summary>
public interface IPicViewResponder : IResponder
{
    /// <summary>
    /// 图片界面当前阅读章节改变
    /// </summary>
    public void CurrentEpisodeIndexChanged(object sender, PicViewContext ctx, int oldValue, int newValue);
    /// <summary>
    /// 图片界面当前阅读页改变
    /// </summary>
    public void CurrentPageIndexChanged(object sender, PicViewContext ctx, int oldValue, int newValue);
    /// <summary>
    /// 图片页面加载图片
    /// </summary>
    public void PicturesLoadStarting(object sender, PicViewContext ctx);
}