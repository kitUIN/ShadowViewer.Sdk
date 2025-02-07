using System.Collections.Generic;
using SqlSugar;
using ShadowPluginLoader.MetaAttributes;
using ShadowViewer.Core.Responders;
using ShadowViewer.Core.Models.Interfaces;
using ShadowViewer.Core;
using ShadowViewer.Core.Services;
using ShadowViewer.Core.Enums;

namespace ShadowViewer.Core.Responders;
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

    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected ICallableService Caller { get; }

    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected ISqlSugarClient Db { get; }

    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected CompressService CompressServices { get; }

    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    protected PluginLoader PluginService { get; }

}