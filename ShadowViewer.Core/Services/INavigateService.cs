using System;
using Microsoft.UI.Xaml.Media.Animation;
using ShadowViewer.Core.Args;

namespace ShadowViewer.Core.Services;

/// <summary>
/// 导航服务类
/// </summary>
public interface INavigateService
{
    /// <summary>
    /// 尝试选中左侧导航栏事件
    /// </summary>
    event EventHandler<TrySelectItemEventArgs>? TrySelectItemEvent;

    /// <summary>
    /// 跳转页面
    /// </summary>
    /// <param name="page">跳转页面</param>
    /// <param name="parameter">跳转参数</param>
    /// <param name="info">跳转动画</param>
    /// <param name="force">是否强制跳转(当前页相同时)</param>
    /// <param name="selectItemId">ItemId</param>
    void Navigate(Type page, object? parameter = null,
        NavigationTransitionInfo? info = null, bool force = false, string? selectItemId = null);

    /// <summary>
    /// 根据uri进行导航
    /// </summary>
    /// <param name="uri">Uri</param>
    void Navigate(Uri uri);
}