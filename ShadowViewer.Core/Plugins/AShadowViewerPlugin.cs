using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core;
using ShadowViewer.Core.Models;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Services;
using SqlSugar;
using System;

namespace ShadowViewer.Core.Plugins;

/// <summary>
/// ShadowViewer提供的抽象插件类
/// </summary> 
public abstract partial class AShadowViewerPlugin(
    ICallableService caller,
    ISqlSugarClient db,
    PluginEventService pluginEventService,
    CompressService compressService,
    ILogger logger,
    PluginLoader pluginService,
    INotifyService notifyService) : AbstractPlugin(
    logger, pluginEventService)
{
    /// <summary>
    /// 触发器服务
    /// </summary>
    public ICallableService Caller { get; } = caller;

    /// <summary>
    /// 数据库服务
    /// </summary>
    public ISqlSugarClient Db { get; } = db;

    /// <summary>
    /// 解压缩服务
    /// </summary>
    public CompressService Compressor { get; } = compressService;

    /// <summary>
    /// 响应器服务
    /// </summary>
    public PluginLoader PluginService { get; } = pluginService;

    /// <summary>
    /// 通知服务
    /// </summary>
    public INotifyService Notifier { get; } = notifyService;

    /// <summary>
    /// 插件元数据
    /// </summary>
    public abstract PluginMetaData MetaData { get; }

    /// <summary>
    /// 类别标签
    /// </summary>
    public abstract ShadowTag AffiliationTag { get; }

    /// <summary>
    /// 设置页面
    /// </summary>
    public virtual Type? SettingsPage => null;

    /// <summary>
    /// 能否开关
    /// </summary>
    public virtual bool CanSwitch { get; } = true;

    /// <summary>
    /// 能否删除
    /// </summary>
    public virtual bool CanDelete { get; } = true;

    /// <summary>
    /// 能否打开文件夹
    /// </summary>
    public virtual bool CanOpenFolder { get; } = true;
}