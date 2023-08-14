namespace ShadowViewer.Models;

public partial class ShadowEpisode : ObservableObject,IShadowEpisode
{
    [ObservableProperty] private object body;

    [ObservableProperty] private string title;
}