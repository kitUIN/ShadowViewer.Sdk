using System;
using CommunityToolkit.WinUI.Animations;
using Microsoft.UI.Xaml.Controls;

namespace ShadowViewer.Core.Models.Interfaces;

/// <summary>
/// 导航项
/// </summary>
public interface IShadowNavigationItem : IPluginId
{
    /// <summary>
    /// 内容
    /// </summary>
    object? Content { get; }

    /// <summary>
    /// 图标
    /// </summary>
    IconElement? Icon { get; }

    /// <summary>
    /// 跳转的标识符
    /// </summary>
    public string? Id { get; }
}