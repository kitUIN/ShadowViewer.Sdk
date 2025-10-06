using System;
using System.Text.RegularExpressions;
using FluentIcons.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ShadowViewer.Sdk.Extensions;

/// <summary>
/// 
/// </summary>
public static class PluginLogoViewboxExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty PluginLogoProperty =
        DependencyProperty.RegisterAttached(
            "PluginLogo",
            typeof(string),
            typeof(PluginLogoViewboxExtensions),
            new PropertyMetadata(null, OnPluginLogoChanged));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetPluginLogo(DependencyObject obj)
    {
        return (string)obj.GetValue(PluginLogoProperty);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetPluginLogo(DependencyObject obj, string value)
    {
        obj.SetValue(PluginLogoProperty, value);
    }

    private static void OnPluginLogoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Viewbox viewbox || e.NewValue is not string value) return;
        var uri = new Uri(value);
        string glyph;
        switch (uri.Scheme)
        {
            case "ms-appx" or "http" or "https":
                viewbox.Child = new BitmapIcon()
                {
                    UriSource = uri,
                    ShowAsMonochrome = false,
                };
                break;
            case "font":
                glyph = value.Replace("font://", "");
                if (glyph.StartsWith("\\")) glyph = Regex.Unescape(glyph);
                viewbox.Child = new FontIcon()
                {
                    Glyph = glyph,
                };
                break;
            case "fluent":
                switch (uri.Host)
                {
                    case "regular":
                        glyph = value.Replace("fluent://regular/", "");
                        Enum.TryParse(glyph, out Icon regularIcon);
                        viewbox.Child = new FluentIcons.WinUI.FluentIcon()
                        {
                            Icon = regularIcon,
                            IconVariant = IconVariant.Regular
                        };
                        break;
                    case "filled":
                        glyph = value.Replace("fluent://filled/", "");
                        Enum.TryParse(glyph, out Icon filledIcon);
                        viewbox.Child = new FluentIcons.WinUI.FluentIcon()
                        {
                            Icon = filledIcon,
                            IconVariant = IconVariant.Filled
                        };
                        break;
                }

                break;
        }
    }
}