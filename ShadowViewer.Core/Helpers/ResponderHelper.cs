using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Responders;

namespace ShadowViewer.Core.Helpers;
/// <summary>
/// 响应服务帮助类
/// </summary>
public class ResponderHelper
{
    /// <summary>
    /// 获取响应类
    /// </summary>
    public static IEnumerable<TResponder> GetResponders<TResponder>() where TResponder : IResponder
    {
        return DiFactory.Services.ResolveMany<TResponder>();
    }

    /// <summary>
    /// 获取启用的响应类
    /// </summary>
    public static IEnumerable<TResponder> GetEnabledResponders<TResponder>() where TResponder : IResponder
    {
        var pluginLoader = DiFactory.Services.Resolve<PluginLoader>();
        return GetResponders<TResponder>().Where(x => pluginLoader.IsEnabled(x.Id) == true);
    }
    /// <summary>
    /// 获取启用的指定Id的响应类
    /// </summary>
    public static TResponder? GetEnabledResponder<TResponder>(string id) where TResponder : IResponder
    {
        var pluginLoader = DiFactory.Services.Resolve<PluginLoader>();
        if (GetResponders<TResponder>()
            .FirstOrDefault(x => string.Equals(id, x.Id, StringComparison.CurrentCultureIgnoreCase)) is { } responder &&
            pluginLoader.IsEnabled(id) == true) return responder;
        return default;
    }
    /// <summary>
    /// 获取指定Id的响应类
    /// </summary>
    public static TResponder? GetResponder<TResponder>(string id) where TResponder : IResponder
    {
        if (GetResponders<TResponder>()
                .FirstOrDefault(x => string.Equals(id, x.Id, StringComparison.CurrentCultureIgnoreCase)) is { } responder) return responder;
        return default;
    }
}