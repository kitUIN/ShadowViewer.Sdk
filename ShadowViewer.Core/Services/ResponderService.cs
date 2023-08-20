using ShadowViewer.Responders;

namespace ShadowViewer.Services;

public class ResponderService
{
    private PluginService PluginService { get; }

    public ResponderService(PluginService pluginService)
    {
        PluginService = pluginService;
    }

    public IEnumerable<TResponder> GetResponders<TResponder>() where TResponder: IResponder
    {
        return DiFactory.Services.ResolveMany<TResponder>();
    }

    /// <summary>
    /// 获取启用的导航响应类
    /// </summary>
    public IEnumerable<TResponder> GetEnabledResponders<TResponder>() where TResponder: IResponder
    {
        return DiFactory.Services.ResolveMany<TResponder>().Where(x => PluginService.IsEnabled(x.Id));
    }

    public TResponder? GetEnabledResponder<TResponder>(string id)where TResponder: IResponder
    {
        if (DiFactory.Services.ResolveMany<TResponder>().FirstOrDefault(x => id.Equals(x.Id, StringComparison.OrdinalIgnoreCase)) is { } responder &&
            PluginService.IsEnabled(id)) return responder;
        return default;
    }
}