using System;
using System.Numerics;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;

namespace ShadowViewer.Sdk.Behaviors;
/// <summary>
/// 提供旋转行为的附加属性，使任何 <see cref="FrameworkElement"/> 都能实现围绕中心旋转的动画。
/// 基于高性能的 Composition API 实现。
/// </summary>
public class RotationBehavior : DependencyObject
{
    #region Dependency Properties

    /// <summary>
    /// 标识 IsRotating 附加属性。
    /// </summary>
    public static readonly DependencyProperty IsRotatingProperty =
        DependencyProperty.RegisterAttached(
            "IsRotating",
            typeof(bool),
            typeof(RotationBehavior),
            new PropertyMetadata(false, OnIsRotatingChanged));

    /// <summary>
    /// 标识 Duration 附加属性。
    /// </summary>
    public static readonly DependencyProperty DurationProperty =
        DependencyProperty.RegisterAttached(
            "Duration",
            typeof(double),
            typeof(RotationBehavior),
            new PropertyMetadata(2.0));

    #endregion

    #region Accessors

    /// <summary>
    /// 获取指定元素是否正在旋转。
    /// </summary>
    /// <param name="element">要查询的目标元素。</param>
    /// <returns>如果正在旋转则返回 true，否则返回 false。</returns>
    public static bool GetIsRotating(DependencyObject element) => (bool)element.GetValue(IsRotatingProperty);

    /// <summary>
    /// 设置指定元素是否开始旋转。
    /// </summary>
    /// <param name="element">目标元素。</param>
    /// <param name="value">设为 true 则开始旋转动画，false 则停止。</param>
    public static void SetIsRotating(DependencyObject element, bool value) => element.SetValue(IsRotatingProperty, value);

    /// <summary>
    /// 获取旋转动画完成一周所需的时间（秒）。
    /// </summary>
    /// <param name="element">目标元素。</param>
    /// <returns>旋转时长（秒）。</returns>
    public static double GetDuration(DependencyObject element) => (double)element.GetValue(DurationProperty);

    /// <summary>
    /// 设置旋转动画完成一周所需的时间（秒）。默认为 2.0 秒。
    /// </summary>
    /// <param name="element">目标元素。</param>
    /// <param name="value">时长（秒）。</param>
    public static void SetDuration(DependencyObject element, double value) => element.SetValue(DurationProperty, value);

    #endregion

    #region Private Methods

    /// <summary>
    /// 当 IsRotating 属性值发生更改时触发的回调。
    /// </summary>
    private static void OnIsRotatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement fe)
        {
            if ((bool)e.NewValue) Start(fe);
            else Stop(fe);
        }
    }

    /// <summary>
    /// 初始化并启动合成层旋转动画。
    /// </summary>
    /// <param name="fe">目标框架元素。</param>
    private static void Start(FrameworkElement fe)
    {
        // 确保元素已加载，以便正确获取 Visual 和计算尺寸
        if (!fe.IsLoaded)
        {
            fe.Loaded += (s, a) => Start(fe);
            return;
        }

        var visual = ElementCompositionPreview.GetElementVisual(fe);
        var compositor = visual.Compositor;

        double seconds = GetDuration(fe);

        // 创建标量关键帧动画，目标是 RotationAngleInDegrees 属性
        var animation = compositor.CreateScalarKeyFrameAnimation();

        // 使用线性缓动函数以确保匀速旋转
        var linearEasing = compositor.CreateLinearEasingFunction();
        animation.InsertKeyFrame(1f, 360f, linearEasing);

        animation.Duration = TimeSpan.FromSeconds(seconds);
        animation.IterationBehavior = AnimationIterationBehavior.Forever;

        // 初始化中心点并订阅尺寸变更事件以保持中心点准确
        UpdateCenter(fe, visual);
        fe.SizeChanged -= OnSizeChanged;
        fe.SizeChanged += OnSizeChanged;

        visual.StartAnimation("RotationAngleInDegrees", animation);
    }

    /// <summary>
    /// 停止旋转动画并释放事件订阅。
    /// </summary>
    /// <param name="fe">目标框架元素。</param>
    private static void Stop(FrameworkElement fe)
    {
        var visual = ElementCompositionPreview.GetElementVisual(fe);
        visual.StopAnimation("RotationAngleInDegrees");
        fe.SizeChanged -= OnSizeChanged;
    }

    /// <summary>
    /// 当元素尺寸发生变化时，重新计算旋转中心点。
    /// </summary>
    private static void OnSizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateCenter((FrameworkElement)sender, ElementCompositionPreview.GetElementVisual((FrameworkElement)sender));

    /// <summary>
    /// 将 Visual 的中心点设置为元素的像素中心。
    /// </summary>
    private static void UpdateCenter(FrameworkElement fe, Visual v) =>
        v.CenterPoint = new Vector3((float)fe.ActualWidth / 2, (float)fe.ActualHeight / 2, 0);

    #endregion
}