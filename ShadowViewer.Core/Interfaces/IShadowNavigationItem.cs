namespace ShadowViewer.Interfaces;

public interface IShadowNavigationItem
{
    /// <summary>
    /// 内容
    /// </summary>
    object? Content { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    IconElement? Icon { get; set; }

    /// <summary>
    /// 跳转的标识符
    /// </summary>
    public string? Id { get; set; }
}