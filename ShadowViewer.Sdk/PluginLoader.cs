using System;
using DryIoc;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using SqlSugar;

namespace ShadowViewer.Sdk;

/// <summary>
/// ShadowViewer 插件加载器
/// </summary>
[CheckAutowired]
public partial class PluginLoader : AbstractPluginLoader<PluginMetaData, AShadowViewerPlugin>
{
    /// <inheritdoc/>
    protected override void AfterLoadPlugin(Type tPlugin, AShadowViewerPlugin aPlugin, PluginMetaData meta)
    {
        var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        var responder = aPlugin.MetaData.PluginResponder;
        if (responder.NavigationResponder != null)
        {
            DiFactory.Services.Register(typeof(INavigationResponder), responder.NavigationResponder.EntryPointType,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load INavigationResponder: {TNavigationResponder}",
                meta.Id, meta.Name,
                responder.NavigationResponder.EntryPointType);
        }

        if (responder.PicViewResponder != null)
        {
            DiFactory.Services.Register(typeof(IPicViewResponder), responder.PicViewResponder.EntryPointType,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load IPicViewResponder: {TNavigationResponder}",
                meta.Id, meta.Name,
                responder.PicViewResponder.EntryPointType);
        }

        if (responder.HistoryResponder != null)
        {
            DiFactory.Services.Register(typeof(IHistoryResponder), responder.HistoryResponder.EntryPointType,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load IHistoryResponder: {TNavigationResponder}",
                meta.Id, meta.Name,
                responder.HistoryResponder.EntryPointType);
        }

        if (responder.SearchSuggestionResponder != null)
        {
            DiFactory.Services.Register(typeof(ISearchSuggestionResponder), responder.SearchSuggestionResponder.EntryPointType,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load ISearchSuggestionResponder: {TNavigationResponder}",
                meta.Id, meta.Name,
                responder.SearchSuggestionResponder.EntryPointType);
        }

        foreach (var folder in responder.SettingFolders)
        {
            DiFactory.Services.Register(typeof(ISettingFolder), folder.EntryPointType,
                Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
            Logger.Information(
                "{Id}{Name} Load ISettingFolder: {TNavigationResponder}",
                meta.Id, meta.Name,
                folder.EntryPointType);
        }

    }
}