using System.Collections;
using CommunityToolkit.WinUI.UI.Converters;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.VisualBasic;

namespace ShadowViewer.Converters;

public class CollectionCountToVisibilityConverter: EmptyObjectToObjectConverter
{
    protected override bool CheckValueIsEmpty(object value)
    {
        var isEmpty = true;
        if (value is int i)
        {
            isEmpty = i == 0;
        }
        return isEmpty;
    }
}