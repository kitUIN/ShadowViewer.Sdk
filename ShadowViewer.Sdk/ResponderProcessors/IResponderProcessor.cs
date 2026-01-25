using ShadowPluginLoader.WinUI.Models;
using ShadowViewer.Sdk.Plugins;
using System;

namespace ShadowViewer.Sdk.ResponderProcessors;

/// <summary>
/// 
/// </summary>
public interface IResponderProcessor
{
    /// <summary>
    /// Gets the name of the support responder.
    /// </summary>
    /// <value>
    /// The name of the support responder.
    /// </value>
    string[] SupportResponderName { get; }

    /// <summary>
    /// 
    /// </summary>
    bool ResponderProcess(PluginEntryPointType entryPoint, AShadowViewerPlugin aPlugin, PluginMetaData meta);

}