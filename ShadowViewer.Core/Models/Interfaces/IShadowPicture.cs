using Microsoft.UI.Xaml.Media;

namespace ShadowViewer.Models.Interfaces;

public interface IShadowPicture
{
    /// <summary>
    /// 序号(请从1开始)
    /// </summary>
    int Index { get; set; }
    /// <summary>
    /// 图片
    /// </summary>
    ImageSource Source { get; set; }
}