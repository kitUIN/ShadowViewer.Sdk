using System;

namespace ShadowViewer.Core.Args;

/// <summary>
/// 跳转页面事件
/// </summary>
public class NavigateToEventArgs(Type page, object? parameter, bool force) : EventArgs
{
    /// <summary>
    /// 跳转参数
    /// </summary>
    public object? Parameter { get; } = parameter;
    /// <summary>
    /// 跳转页面
    /// </summary>
    public Type Page { get; } = page;
    /// <summary>
    /// 是否强制跳转
    /// </summary>
    public bool Force { get; } = force;
    /// <summary>
    /// ToString
    /// </summary>
    public new string ToString() => $"{{NavigateToEventArgs,Page={Page},Force={Force},Parameter={Parameter}}}";
}