using CustomExtensions.WinUI;
using Microsoft.UI.Xaml;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Helpers;
using ShadowViewer.Sdk.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

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


    protected override IEnumerable<string> ResourceDictionaries { get; } = [];

    /// <summary>
    /// Init
    /// </summary>
    protected override void Init()
    {
        WindowHelper.ActiveWindows.First()!.DispatcherQueue.TryEnqueue(() =>
        {
            foreach (var item in ResourceDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(
                    new ResourceDictionary()
                    {
                        Source = new Uri(item.PluginPath())
                    });
            }
        });
    }
}