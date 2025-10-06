using System;
using System.Reflection;
using FluentIcons.Common;
using Microsoft.UI.Xaml.Data;
using ShadowViewer.Controls.Attributes;

namespace ShadowViewer.Sdk.Converters;

/// <summary>
/// 
/// </summary>
public class MenuFlyoutEnumItemConverter<T> : IValueConverter where T : Enum
{
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object parameter, string language)
    {
        if (value == null) return null;
        if (targetType == typeof(string)) return GetI18N(value.ToString()!);
        if (value is not T mode) return null;
        var field = typeof(T).GetField(mode.ToString()!);
        var icon = field?.GetCustomAttribute<MenuFlyoutItemIconAttribute>();
        if (targetType == typeof(Icon)) return icon?.Icon;
        return icon?.IconVariant;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual string GetI18N(string value)
    {
        return I18n.ResourcesHelper.GetString(value);
    }
    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}