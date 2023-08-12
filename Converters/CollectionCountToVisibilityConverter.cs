using System.Collections;
using CommunityToolkit.WinUI.UI.Converters;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.VisualBasic;

namespace ShadowViewer.Converters;

public class CollectionToVisibilityConverter: EmptyObjectToObjectConverter
{
    protected override bool CheckValueIsEmpty(object value)
    {
        var isEmpty = true;
        if (value is ICollection collection)
        {
            isEmpty = collection.Count == 0;
            Log.Information("{r}11",isEmpty);
        }
        return isEmpty;
    }
}