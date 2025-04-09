using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Core.Enums;
using ShadowViewer.Core.Services;

namespace ShadowViewer.Core.Args;

/// <summary>
/// <see cref="ICallableService.TopLevelControlEvent"/>
/// </summary>
public class TopLevelControlEventArgs(UIElement control, TopLevelControlMode mode) : EventArgs
{
    /// <summary>
    /// 顶层控件
    /// </summary>
    public UIElement Control { get; } = control;

    /// <summary>
    /// 模式
    /// </summary>
    public TopLevelControlMode Mode { get; } = mode;
}