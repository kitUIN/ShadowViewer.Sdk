namespace ShadowViewer.Responders;

public interface IHistoryResponder:IResponder
{
    /// <summary>
    /// 获取历史记录
    /// </summary>
    /// <returns></returns>
    IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day);
    /// <summary>
    /// 点击历史记录的响应
    /// </summary>
    void HistoryHandler(IHistory history);
}