using ShadowViewer.Responders;

namespace ShadowViewer.Services;

public class ResponderService
{
    private PluginService pluginService;

    public ResponderService(PluginService pluginService)
    {
        this.pluginService = pluginService;
    }

    public IEnumerable<INavigationResponder> GetNavigationViewResponders()
    {
        return DiFactory.Services.ResolveMany<INavigationResponder>();
    }

    /// <summary>
    /// 获取启用的导航响应类
    /// </summary>
    public IEnumerable<INavigationResponder> GetEnabledNavigationViewResponders()
    {
        return DiFactory.Services.ResolveMany<INavigationResponder>().Where(x => pluginService.IsEnabled(x.Id));
    }

    public INavigationResponder? GetEnabledNavigationViewResponder(string id)
    {
        if (DiFactory.Services.ResolveMany<INavigationResponder>().FirstOrDefault(x => id.Equals(x.Id, StringComparison.OrdinalIgnoreCase)) is { } responder &&
            pluginService.IsEnabled(id)) return responder;
        return null;
    }
}