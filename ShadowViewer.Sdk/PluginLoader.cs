using ShadowPluginLoader.Attributes;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Plugins;
using ShadowViewer.Sdk.ResponderProcessors;
using System;

namespace ShadowViewer.Sdk;

/// <summary>
/// ShadowViewer 插件加载器
/// </summary>
[CheckAutowired]
public partial class PluginLoader : AbstractPluginLoader<PluginMetaData, AShadowViewerPlugin>
{
    static PluginLoader()
    {
        ResponderProcessorRegistry.Register(new PluginResponderProcessor());
    }

    /// <inheritdoc/>
    protected override void AfterLoadPlugin(Type tPlugin, AShadowViewerPlugin aPlugin, PluginMetaData meta)
    {
        foreach (var entryPoint in aPlugin.MetaData.EntryPoints)
        {
            if (ResponderProcessorRegistry.TryGetProcessor(entryPoint.Name, out var processor))
            {
                var flag = processor!.ResponderProcess(entryPoint, aPlugin, meta);
                if (flag)
                {
                    Logger.Information(
                        "{Id}{Name} Load {TNavigationResponder} Success",
                        meta.Id, meta.Name,
                        entryPoint.EntryPointType);
                }
            }
            else
            {
                Logger.Debug("Unknown responder {Name} in plugin {Id}，Skip", entryPoint.Name, meta.Id);
            }
        }
    }
}