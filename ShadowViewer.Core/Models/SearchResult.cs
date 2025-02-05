using Microsoft.UI.Xaml;

namespace ShadowViewer.Models;

/// <summary>
/// 搜索结果
/// </summary>
/// <param name="icon"></param>
/// <param name="name"></param>
/// <param name="method"></param>
/// <param name="source"></param>
public class SearchResult(UIElement icon, string name, string method, object source)
{
    /// <summary>
    /// 图标
    /// </summary>
    public UIElement Icon { get; set; } = icon;
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = name;
    /// <summary>
    /// 类型
    /// </summary>
    public string Method { get; set; } = method;
    /// <summary>
    /// 原始数据
    /// </summary>
    public object Source { get; set; } = source;
}