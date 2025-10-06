using System;
using Microsoft.UI.Xaml.Media.Animation;

namespace ShadowViewer.Sdk.Utils;

/// <summary>
/// 跳转页面参数
/// </summary>
/// <param name="Page">跳转页面</param>
/// <param name="Parameter">跳转参数</param>
/// <param name="Info">跳转动画</param>
/// <param name="Force">是否强制跳转(当前页相同时)</param>
/// <param name="SelectItemId">ShadowNavigationItem的Id</param>
public record ShadowNavigation(
    Type Page,
    object? Parameter = null,
    NavigationTransitionInfo? Info = null,
    bool Force = false,
    string? SelectItemId = null);