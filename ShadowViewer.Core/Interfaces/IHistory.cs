namespace ShadowViewer.Interfaces;

public interface IHistory
{
    /// <summary>
    /// 名称
    /// </summary>
    string? Title { get; set; }
    /// <summary>
    /// ID
    /// </summary>
    string Id { get; set; }
    /// <summary>
    /// 图片
    /// </summary>
    string? Icon { get; set; }
    /// <summary>
    /// 阅读时间
    /// </summary>
    DateTime Time { get; set; }
    /// <summary>
    /// 来源插件
    /// </summary>
    string Plugin { get; }
}