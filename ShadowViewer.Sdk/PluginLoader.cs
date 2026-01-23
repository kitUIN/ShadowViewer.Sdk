using DryIoc;
using Microsoft.UI.Xaml.Controls;
using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using System;

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
        foreach (var entryPoint in aPlugin.MetaData.EntryPoints)
        {
            switch (entryPoint.Name)
            {
                case "PicViewResponder":
                    DiFactory.Services.Register(typeof(IPicViewResponder), entryPoint.EntryPointType,
                        Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                    Logger.Information(
                        "{Id}{Name} Load IPicViewResponder: {TNavigationResponder}",
                        meta.Id, meta.Name,
                        entryPoint.EntryPointType);
                    break;
                case "HistoryResponder":
                    DiFactory.Services.Register(typeof(IHistoryResponder), entryPoint.EntryPointType,
                        Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                    Logger.Information(
                        "{Id}{Name} Load IHistoryResponder: {TNavigationResponder}",
                        meta.Id, meta.Name,
                        entryPoint.EntryPointType);
                    break;
                case "SearchSuggestionResponder":
                    DiFactory.Services.Register(typeof(ISearchSuggestionResponder),
                        entryPoint.EntryPointType,
                        Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                    Logger.Information(
                        "{Id}{Name} Load ISearchSuggestionResponder: {TNavigationResponder}",
                        meta.Id, meta.Name,
                        entryPoint.EntryPointType);
                    break;
                case "NavigationResponder":
                    DiFactory.Services.Register(typeof(INavigationResponder), entryPoint.EntryPointType,
                        Reuse.Singleton, serviceKey: meta.Id, made: Parameters.Of.Type(_ => meta.Id));
                    Logger.Information(
                        "{Id}{Name} Load INavigationResponder: {TNavigationResponder}",
                        meta.Id, meta.Name,
                        entryPoint.EntryPointType);
                    DiFactory.Services.Resolve<INavigationResponder>(serviceKey: meta.Id).Register();
                    break;
                case "SettingFolders":
                    DiFactory.Services.Register(typeof(ISettingFolder), entryPoint.EntryPointType,
                        Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                    Logger.Information(
                        "{Id}{Name} Load ISettingFolder: {TNavigationResponder}",
                        meta.Id, meta.Name,
                        entryPoint.EntryPointType);
                    break;
            }
        }
    }
}