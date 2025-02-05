using ShadowViewer.Args;

namespace ShadowViewer.Responders;

/// <summary>
/// 图片阅读器触发器基类
/// </summary>
public interface IPicViewResponder: IResponder
{
    /// <summary>
    /// 图片界面章节改变
    /// </summary>
    public void CurrentEpisodeIndexChanged(object sender, string affiliation, int oldValue, int newValue);
    /// <summary>
    /// 图片页面加载图片
    /// </summary>
    public void PicturesLoadStarting(object sender, PicViewArg arg);
}