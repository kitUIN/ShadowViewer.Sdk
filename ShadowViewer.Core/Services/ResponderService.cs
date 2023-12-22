using ShadowViewer.Responders;
using ShadowViewer.Services.Interfaces;

namespace ShadowViewer.Services;

public class ResponderService
{
    private IPluginService PluginService { get; }

    public ResponderService(IPluginService pluginService)
    {
        PluginService = pluginService;
    }

    public IEnumerable<TResponder> GetResponders<TResponder>() where TResponder: IResponder
    {
        return DiFactory.Services.Resolve<IEnumerable<TResponder>>();
    }

    /// <summary>
    /// 获取启用的导航响应类
    /// </summary>
    public IEnumerable<TResponder> GetEnabledResponders<TResponder>() where TResponder: IResponder
    {
        return GetResponders<TResponder>().Where(x => PluginService.IsEnabled(x.Id) == true);
    }

    public TResponder? GetEnabledResponder<TResponder>(string id)where TResponder: IResponder
    {
        if (GetResponders<TResponder>()
            .FirstOrDefault(x => id == x.Id) is { } responder &&
            PluginService.IsEnabled(id) == true) return responder;
        return default;
    }
}