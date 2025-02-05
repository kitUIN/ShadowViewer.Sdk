using System.Collections;
using CommunityToolkit.WinUI.Converters;

namespace ShadowViewer.Converters;
/// <summary>
/// 检查集合是否为空,请传入Count
/// </summary>
public class CollectionCountToVisibilityConverter: EmptyObjectToObjectConverter
{
    /// <inheritdoc />
    protected override bool CheckValueIsEmpty(object value)
    {
        var isEmpty = value switch
        {
            int i => i == 0,
            IList j => j.Count == 0,
            _ => true
        };
        return isEmpty;
    }
}