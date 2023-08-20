using ShadowViewer.Responders;

namespace ShadowViewer.Services;

public class ResponderService
{
    private PluginService pluginService;
    public ResponderService(PluginService pluginService)
    {
        this.pluginService = pluginService;
    }
    public IEnumerable<INavigationResponder> NavigationViewResponders()
    {
        return DiFactory.Services.ResolveMany<INavigationResponder>();
    }
    public IEnumerable<INavigationResponder> EnabledNavigationViewResponders()
    {
        return DiFactory.Services.ResolveMany<INavigationResponder>();
    }
}