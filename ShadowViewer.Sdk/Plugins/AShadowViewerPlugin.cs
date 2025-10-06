using System;
using System.Collections.Generic;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Services;
using SqlSugar;

namespace ShadowViewer.Sdk.Plugins;

/// <summary>
/// ShadowViewer提供的抽象插件类
/// </summary> 
public abstract partial class AShadowViewerPlugin : AbstractPlugin<PluginMetaData>
{
    /// <summary>
    /// 触发器服务
    /// </summary>
    [Autowired]
    public ICallableService Caller { get; }

    /// <summary>
    /// 数据库服务
    /// </summary>
    [Autowired]
    public ISqlSugarClient Db { get; }

    /// <summary>
    /// 响应器服务
    /// </summary>
    [Autowired]
    public PluginLoader PluginService { get; }

    /// <summary>
    /// 通知服务
    /// </summary>
    [Autowired]
    public INotifyService Notifier { get; }

    /// <summary>
    /// 注册自定义接收者
    /// </summary>
    public virtual Dictionary<string, Type> RegisterForResponders { get; } = new();
}