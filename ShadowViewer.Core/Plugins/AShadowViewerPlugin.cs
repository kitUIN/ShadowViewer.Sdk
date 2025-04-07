using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Models;
using ShadowViewer.Core.Services;
using SqlSugar;
using System;
using Microsoft.IdentityModel.Tokens;
using ShadowPluginLoader.Attributes;

namespace ShadowViewer.Core.Plugins;

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

}