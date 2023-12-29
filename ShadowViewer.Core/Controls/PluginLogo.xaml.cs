using FluentIcon.WinUI;

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
            var url = new Uri(value);
            if (url != null )
            {
                switch(url.Scheme)
                {
                    case "ms-appx":
                        BitmapIcon.Visibility = Visibility.Visible;
                        BitmapIcon.UriSource = url;
                        break;
                    case "font":
                        FontIcon.Visibility = Visibility.Visible;
                        FontIcon.Glyph = value.Replace("font://", "");
                        break;
                    case "fluent":
                        if(url.Host == "regular")
                        {
                            FluentRegularIcon.Visibility = Visibility.Visible;
                            FluentRegularIcon.Glyph = value.Replace("fluent://regular/", "");
                        }else if (url.Host == "filled")
                        {
                            FluentRegularIcon.Visibility = Visibility.Visible;
                            FluentRegularIcon.Glyph = value.Replace("fluent://filled/", "");
                        }
                        
                        break;
                }
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