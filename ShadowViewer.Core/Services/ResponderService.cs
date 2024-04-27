using ShadowViewer.Responders;

namespace ShadowViewer.Services;

public class ResponderService
{
    private PluginLoader PluginService { get; }

    public ResponderService(PluginLoader pluginService)
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
            .FirstOrDefault(x => String.Equals(id , x.Id, StringComparison.CurrentCultureIgnoreCase)) is { } responder &&
            PluginService.IsEnabled(id) == true) return responder;
        return default;
    }
}