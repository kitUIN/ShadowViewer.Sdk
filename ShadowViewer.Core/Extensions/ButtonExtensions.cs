using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace ShadowViewer.Core.Extensions;

/// <summary>
/// Button拓展类
/// </summary>
public class ButtonExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetClickShowFlyout(ButtonBase element, bool value)
    {
        element.SetValue(ClickShowFlyoutProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool GetClickShowFlyout(ButtonBase element)
    {
        return (bool)element.GetValue(ClickShowFlyoutProperty);
    }

    /// <summary>
    /// 点击打开
    /// </summary>
    public static readonly DependencyProperty ClickShowFlyoutProperty =
        DependencyProperty.RegisterAttached("ClickShowFlyout", typeof(bool), typeof(ButtonExtensions),
            new PropertyMetadata(false, OnAutoShowFlyoutChanged));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnAutoShowFlyoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ButtonBase element || !(bool)e.NewValue || e.NewValue == e.OldValue) return;
        element.Click -= ShowFlyout;
        element.Click += ShowFlyout;
    }

    /// <summary>
    /// 显示 Flyout
    /// </summary>
    private static void ShowFlyout(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement button) return;
        var flyout = FlyoutBase.GetAttachedFlyout(button);
        flyout?.ShowAt(button);
    }
}