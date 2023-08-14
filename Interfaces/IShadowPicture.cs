namespace ShadowViewer.Interfaces;

public interface IShadowPicture
{
    /// <summary>
    /// 序号(请从1开始)
    /// </summary>
    int Index{ get; set; }
    /// <summary>
    /// 图片
    /// </summary>
    ImageSource Source{ get; set; }
    /// <summary>
    /// 其他备注
    /// </summary>
    object Tag{ get; set; }
}