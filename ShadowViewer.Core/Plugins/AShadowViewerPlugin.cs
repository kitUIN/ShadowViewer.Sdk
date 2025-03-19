using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Models;
using ShadowViewer.Core.Services;
using SqlSugar;
using System;
using ShadowPluginLoader.MetaAttributes;

namespace ShadowViewer.Core.Plugins;

/// <summary>
/// ShadowViewer提供的抽象插件类
/// </summary> 
public abstract partial class AShadowViewerPlugin : AbstractPlugin
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