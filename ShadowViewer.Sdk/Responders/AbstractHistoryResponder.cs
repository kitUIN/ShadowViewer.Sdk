using System.Collections.Generic;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Sdk.Enums;
using ShadowViewer.Sdk.Models.Interfaces;

namespace ShadowViewer.Sdk.Responders;
/// <summary>
/// 历史记录触发器抽象类
/// </summary>
public abstract partial class AbstractHistoryResponder : IHistoryResponder
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract IEnumerable<IHistory> GetHistories(HistoryMode mode = HistoryMode.Day);
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract void ClickHistoryHandler(IHistory history);
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public abstract void DeleteHistoryHandler(IHistory history);
    /// <inheritdoc/>
    [Autowired]
    public string Id { get; }

}