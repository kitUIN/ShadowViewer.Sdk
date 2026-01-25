using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Models;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.Responders;
using System;

namespace ShadowViewer.Sdk.ResponderProcessors;

/// <summary>
/// 
/// </summary>
/// <seealso cref="ShadowViewer.Sdk.ResponderProcessors.IResponderProcessor" />
public class PluginResponderProcessor : IResponderProcessor
{
    /// <inheritdoc/>
    public string[] SupportResponderName { get; } = [
        "PicViewResponder",
        "HistoryResponder",
        "SearchSuggestionResponder",
        "NavigationResponder",
        "SettingFolders"
    ];

    /// <inheritdoc/>
    public bool ResponderProcess(PluginEntryPointType entryPoint, AShadowViewerPlugin aPlugin, PluginMetaData meta)
    {
        switch (entryPoint.Name)
        {
            case "PicViewResponder":
                DiFactory.Services.Register(typeof(IPicViewResponder), entryPoint.EntryPointType,
                    Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                return true;
            case "HistoryResponder":
                DiFactory.Services.Register(typeof(IHistoryResponder), entryPoint.EntryPointType,
                    Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                return true;
            case "SearchSuggestionResponder":
                DiFactory.Services.Register(typeof(ISearchSuggestionResponder),
                    entryPoint.EntryPointType,
                    Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                return true;
            case "NavigationResponder":
                DiFactory.Services.Register(typeof(INavigationResponder), entryPoint.EntryPointType,
                    Reuse.Singleton, serviceKey: meta.Id, made: Parameters.Of.Type(_ => meta.Id));
                DiFactory.Services.Resolve<INavigationResponder>(serviceKey: meta.Id).Register();
                return true;
            case "SettingFolders":
                DiFactory.Services.Register(typeof(ISettingFolder), entryPoint.EntryPointType,
                    Reuse.Transient, made: Parameters.Of.Type(_ => meta.Id));
                return true;
        }
        return false;
    }
}