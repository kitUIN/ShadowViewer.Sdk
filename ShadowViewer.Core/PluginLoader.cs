using System;
using System.Reflection;
using DryIoc;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Responders;
using SqlSugar;
using ShadowViewer.Core.Plugins;
using ShadowViewer.Core.Models.Interfaces;

namespace ShadowViewer.Core;

/// <summary>
/// ShadowView 插件加载器
/// </summary>
public class PluginLoader(ILogger logger, PluginEventService pluginEventService)
    : AbstractPluginLoader<PluginMetaData, AShadowViewerPlugin>(logger, pluginEventService)
{
    /// <summary>
    /// 加载器版本
    /// </summary>
    public static Version CoreVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version!;

    /// <inheritdoc/>
    protected override void LoadPluginDi(Type tPlugin, AShadowViewerPlugin aPlugin, PluginMetaData meta)
    {
        Type? navigationViewResponder = null;
        Type? picViewResponder = null;
        Type? historyResponder = null;
        var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        foreach (var type in tPlugin.Assembly.GetExportedTypes())
            if (type.IsAssignableTo(typeof(AbstractNavigationResponder))) navigationViewResponder = type;
            else if (type.IsAssignableTo(typeof(AbstractPicViewResponder))) picViewResponder = type;
            else if (type.IsAssignableTo(typeof(AbstractHistoryResponder))) historyResponder = type;
            else if (type.IsAssignableTo(typeof(IHistory)))
            {
                db.CodeFirst.InitTables(type);
            }
            else if (type.IsAssignableTo(typeof(ISettingFolder)))
            {
                DiFactory.Services.Register(typeof(ISettingFolder),
                    type,
                    Reuse.Transient,
                    made: Parameters.Of.Type(_ => meta.Id));
                Logger.Information(
                    "{Id}{Name} Load {INavigationResponder}:{TNavigationResponder}",
                    meta.Id, meta.Name,
                    typeof(ISettingFolder), type.Name);
            }

        if (navigationViewResponder is not null)
        {
            DiFactory.Services.Register(typeof(INavigationResponder), navigationViewResponder,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load INavigationResponder: {TNavigationResponder}",
                meta.Id, meta.Name,
                navigationViewResponder.Name);

        }

        if (picViewResponder is not null)
        {
            DiFactory.Services.Register(typeof(IPicViewResponder), picViewResponder,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load IPicViewResponder: {TNavigationResponder}",
                meta.Id, meta.Name,
                picViewResponder.Name);

        }

        if (historyResponder is not null)
        {
            DiFactory.Services.Register(typeof(IHistoryResponder), historyResponder,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load IHistoryResponder: {TNavigationResponder}",
                meta.Id, meta.Name,
                historyResponder.Name);
        }
    }

    /// <inheritdoc />
    protected override string PluginFolder => CoreSettings.PluginsPath;
    /// <inheritdoc />
    protected override string TempFolder => CoreSettings.TempPath;

}