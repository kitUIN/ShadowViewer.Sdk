using ShadowViewer.Responders;

namespace ShadowViewer.Services;

public class ResponderService
{
    public ObservableCollection<INavigationViewResponder> NavigationViewResponders { get; } = new();
}