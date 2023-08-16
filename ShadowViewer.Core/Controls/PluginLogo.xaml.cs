namespace ShadowViewer.Controls;

public sealed partial class PluginLogo : UserControl
{
    public PluginLogo()
    {
        InitializeComponent();
    }

    public string Logo
    {
        set
        {
            if (value.StartsWith("ms-appx://"))
            {
                BitmapIcon.Visibility = Visibility.Visible;
                BitmapIcon.UriSource = new Uri(value);
            }
            else if (value.StartsWith("font://"))
            {
                FontIcon.Visibility = Visibility.Visible;
                FontIcon.Glyph = value.Replace("font://", "");
            }
            else if (value.StartsWith("fluent://"))
            {
                FluentIcon.Visibility = Visibility.Visible;
                FluentIcon.Glyph = value.Replace("fluent://", "");
            }
        }
    }

    /// <summary>
    /// 获取或设置Content的值
    /// </summary>  
    public int FontIconSize
    {
        get => (int)GetValue(FontIconSizeProperty);
        set => SetValue(FontIconSizeProperty, value);
    }

    /// <summary>
    /// 标识 Content 依赖属性。
    /// </summary>
    public static readonly DependencyProperty FontIconSizeProperty =
        DependencyProperty.Register(nameof(FontIconSize), typeof(int), typeof(PluginLogo),
            new PropertyMetadata(16, OnFontIconSizeChanged));

    private static void OnFontIconSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var target = obj as PluginLogo;
        target.FontIconSize = (int)args.NewValue;
    }
}