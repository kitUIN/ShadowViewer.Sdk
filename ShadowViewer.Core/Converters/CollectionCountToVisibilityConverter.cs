using System.Collections;
using CommunityToolkit.WinUI.Converters;

namespace ShadowViewer.Converters;
/// <summary>
/// 检查集合是否为空,请传入Count
/// </summary>
public class CollectionCountToVisibilityConverter: EmptyObjectToObjectConverter
{
    protected override bool CheckValueIsEmpty(object value)
    {
        var isEmpty = true;
        if (value is int i)
        {
            isEmpty = i == 0;
        }
        else if (value is IList j)
        {
            isEmpty = j.Count == 0;
        }
        return isEmpty;
    }
}