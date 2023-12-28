using System.Collections;

namespace ShadowViewer.Extensions;

/// <summary>
/// 历史记录拓展类,通过时间比较
/// </summary>
public class HistoryExtension : IComparer<IHistory>
{

    private readonly CaseInsensitiveComparer caseiComp = new();

    public int Compare(IHistory? x, IHistory? y)
    {
        return caseiComp.Compare( y?.Time,x?.Time);
    }
}