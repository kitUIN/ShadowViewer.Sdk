using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Responders;

namespace ShadowViewer.Services;
/// <summary>
/// 响应类服务
/// </summary>
public class ResponderService(PluginLoader pluginService)
{
    private PluginLoader PluginService { get; } = pluginService;
    /// <summary>
    /// 获取响应类
    /// </summary>
    public static IEnumerable<TResponder> GetResponders<TResponder>() where TResponder: IResponder
    {
        return DiFactory.Services.ResolveMany<TResponder>();
    }

    /// <summary>
    /// 获取启用的响应类
    /// </summary>
    public IEnumerable<TResponder> GetEnabledResponders<TResponder>() where TResponder: IResponder
    {
        return GetResponders<TResponder>().Where(x => PluginService.IsEnabled(x.Id) == true);
    }
    /// <summary>
    /// 获取启用的指定Id的响应类
    /// </summary>
    public TResponder? GetEnabledResponder<TResponder>(string id)where TResponder: IResponder
    {
        if (GetResponders<TResponder>()
            .FirstOrDefault(x => string.Equals(id , x.Id, StringComparison.CurrentCultureIgnoreCase)) is { } responder &&
            PluginService.IsEnabled(id) == true) return responder;
        return default;
    }
    /// <summary>
    /// 获取指定Id的响应类
    /// </summary>
    public TResponder? GetResponder<TResponder>(string id) where TResponder : IResponder
    {
        if (GetResponders<TResponder>()
                .FirstOrDefault(x => string.Equals(id, x.Id, StringComparison.CurrentCultureIgnoreCase)) is { } responder) return responder;
        return default;
    }
}