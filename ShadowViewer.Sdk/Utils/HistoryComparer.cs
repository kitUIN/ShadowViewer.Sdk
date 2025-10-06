using System.Collections;
using System.Collections.Generic;
using ShadowViewer.Sdk.Models.Interfaces;

namespace ShadowViewer.Sdk.Utils;

/// <summary>
/// 历史记录拓展类,通过时间比较
/// </summary>
public class HistoryComparer : IComparer<IHistory>
{

    private readonly CaseInsensitiveComparer caseInsensitiveComparer = new();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public int Compare(IHistory? x, IHistory? y)
    {
        return caseInsensitiveComparer.Compare(y?.LastReadDateTime, x?.LastReadDateTime);
    }
}