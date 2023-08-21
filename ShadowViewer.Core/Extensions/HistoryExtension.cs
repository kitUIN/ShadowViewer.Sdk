using System.Collections;

namespace ShadowViewer.Extensions;

public class HistoryExtension : IComparer<IHistory>
{

    private readonly CaseInsensitiveComparer caseiComp = new();

    public int Compare(IHistory x, IHistory y)
    {
        return caseiComp.Compare( y.Time,x.Time);
    }
}