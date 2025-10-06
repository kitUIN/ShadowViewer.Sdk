using Microsoft.UI.Xaml.Controls;

namespace ShadowViewer.Sdk.Models.Interfaces;

/// <summary>
/// 搜索项
/// </summary>
public interface IShadowSearchItem
{
    /// <summary>
    /// 主信息
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// 附信息
    /// </summary>
    string SubTitle { get; set; }

    /// <summary>
    /// 插件Id
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    IconSource Icon { get; set; }
}