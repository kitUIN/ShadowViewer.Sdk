namespace ShadowViewer.Responders;
/// <summary>
/// 历史记录触发器基类
/// </summary>
public interface IHistoryResponder : IResponder
{
    /// <summary>
    /// 获取历史记录
    /// </summary>
    /// <returns></returns>
    IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day);
    /// <summary>
    /// 点击历史记录的响应
    /// </summary>
    void ClickHistoryHandler(IHistory history);
    /// <summary>
    /// 删除历史记录
    /// </summary>
    void DeleteHistoryHandler(IHistory history);
}