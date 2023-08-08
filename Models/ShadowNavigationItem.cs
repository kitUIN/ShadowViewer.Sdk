namespace ShadowViewer.Models;

public partial class ShadowNavigationItem:ObservableObject
{
    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty] private object content;
    /// <summary>
    /// 图标
    /// </summary>
    [ObservableProperty] private IconElement icon;
    /// <summary>
    /// 跳转的标识符
    /// </summary>
    public string Id { get; set; }

    public bool IsDefault { get; set; }
}