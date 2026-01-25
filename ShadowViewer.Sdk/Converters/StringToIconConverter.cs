using CustomExtensions.WinUI;
using FluentIcons.Common;
using FluentIcons.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Text.RegularExpressions;
using static System.Enum;

namespace ShadowViewer.Sdk.Converters;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.UI.Xaml.Data.IValueConverter" />
public partial class StringToIconConverter : IValueConverter
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object parameter, string language)
    {
        if (value is not string valueString) return null;
        var uri = new Uri(valueString);
        string glyph;
        switch (uri.Scheme)
        {
            case "ms-plugin" or "ms-appx" or "http" or "https":
                if (uri.Scheme == "ms-plugin")
                {
                    uri = new Uri(valueString.PluginPath());
                }

                return new ImageIcon
                {
                    Source = new BitmapImage(uri)
                };
            case "font":
                glyph = valueString.Replace("font://", "");
                if (glyph.StartsWith('\\')) glyph = Regex.Unescape(glyph);
                return new FontIcon()
                {
                    Glyph = glyph
                };

            case "symbol":
                glyph = valueString.Replace("symbol://", "");
                return new Microsoft.UI.Xaml.Controls.SymbolIcon(
                    Parse<Microsoft.UI.Xaml.Controls.Symbol>(glyph, ignoreCase: true));
            case "fluent":
                switch (uri.Host)
                {
                    case "regular":
                        glyph = valueString.Replace("fluent://regular/", "");
                        if (glyph.StartsWith('\\'))
                        {
                            glyph = Regex.Unescape(glyph);
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Regular,
                                Glyph = glyph
                            };
                        }

                        if (TryParse(glyph, out Icon regularIcon))
                        {
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Regular,
                                Icon = regularIcon
                            };
                        }

                        break;
                    case "filled":
                        glyph = valueString.Replace("fluent://filled/", "");
                        if (glyph.StartsWith('\\'))
                        {
                            glyph = Regex.Unescape(glyph);
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Filled,
                                Glyph = glyph,
                            };
                        }

                        if (TryParse(glyph, out Icon filledIcon))
                        {
                            return new FluentIcon()
                            {
                                IconVariant = IconVariant.Filled,
                                Icon = filledIcon
                            };
                        }

                        break;
                }

                break;
        }

        return null;
    }

    /// <summary>
    /// Converts the back.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}