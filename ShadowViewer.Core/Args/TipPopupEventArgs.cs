using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Enums;

namespace ShadowViewer.Args;

/// <summary>
/// 小弹窗通知Args
/// </summary>
public class TipPopupEventArgs(InfoBar tipPopup,
    TipPopupPosition position = TipPopupPosition.Center,double displaySeconds = 2)
{
    /// <summary>
    /// 通知内容
    /// </summary>
    public InfoBar TipPopup { get; } = tipPopup;

    /// <summary>
    /// 通知位置
    /// </summary>
    public TipPopupPosition Position { get; } = position;

    /// <summary>
    /// 显示时长
    /// </summary>
    public double DisplaySeconds { get; } = displaySeconds;
}