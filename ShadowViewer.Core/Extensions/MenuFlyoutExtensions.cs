using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

namespace ShadowViewer.Core.Extensions;

/// <summary>
/// 
/// </summary>
public static class MenuFlyoutExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty TextConverterProperty =
        DependencyProperty.RegisterAttached(
            "TextConverter",
            typeof(IValueConverter),
            typeof(MenuFlyoutExtensions),
            new PropertyMetadata(null));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static IValueConverter GetTextConverter(DependencyObject obj)
    {
        return (IValueConverter)obj.GetValue(TextConverterProperty);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetTextConverter(DependencyObject obj, IValueConverter value)
    {
        obj.SetValue(TextConverterProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty EnumSourceProperty =
        DependencyProperty.RegisterAttached(
            "EnumSource",
            typeof(Type),
            typeof(MenuFlyoutExtensions),
            new PropertyMetadata(null, OnEnumSourceChanged));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Type GetEnumSource(DependencyObject obj)
    {
        return (Type)obj.GetValue(EnumSourceProperty);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetEnumSource(DependencyObject obj, Type value)
    {
        obj.SetValue(EnumSourceProperty, value);
    }

    private static void OnEnumSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not MenuFlyout menuFlyout || e.NewValue is not Type enumType) return;
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("EnumSource must be an Enum type.");
        }

        menuFlyout.Items.Clear();
        var converter = GetTextConverter(menuFlyout);

        foreach (var value in Enum.GetValues(enumType))
        {
            var menuItem = new MenuFlyoutItem
            {
                Text = converter != null
                    ? converter.Convert(value, typeof(string), null, null)?.ToString()
                    : value.ToString(),
                Tag = value,
            };

            menuItem.Click += (_, _) =>
            {
                SetSelectedValue(menuFlyout, (Enum)value);
                var command = GetEnumCommand(menuFlyout);
                if (command != null && command.CanExecute(value))
                {
                    command.Execute(value);
                }
            };

            menuFlyout.Items.Add(menuItem);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty EnumCommandProperty =
        DependencyProperty.RegisterAttached(
            "EnumCommand",
            typeof(System.Windows.Input.ICommand),
            typeof(MenuFlyoutExtensions),
            new PropertyMetadata(null));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static System.Windows.Input.ICommand? GetEnumCommand(DependencyObject obj)
    {
        return (System.Windows.Input.ICommand)obj.GetValue(EnumCommandProperty);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetEnumCommand(DependencyObject obj, System.Windows.Input.ICommand value)
    {
        obj.SetValue(EnumCommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.RegisterAttached(
            "SelectedValue",
            typeof(object),
            typeof(MenuFlyoutExtensions),
            new PropertyMetadata(null, OnSelectedValueChanged));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static object GetSelectedValue(DependencyObject obj)
    {
        return obj.GetValue(SelectedValueProperty);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetSelectedValue(DependencyObject obj, object value)
    {
        obj.SetValue(SelectedValueProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not MenuFlyout menuFlyout || e.NewValue is not { } selectedValue) return;
        foreach (var item in menuFlyout.Items.OfType<MenuFlyoutItem>())
        {
            var flag = Equals(item.Tag, selectedValue);
            item.Icon = flag ? new FontIcon { Glyph = "\uE7B3" } : null;
            item.FontWeight = flag ? FontWeights.Bold : FontWeights.Normal;
        }
    }
}