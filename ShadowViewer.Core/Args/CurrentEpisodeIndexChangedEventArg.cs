namespace ShadowViewer.Core.Args;

public class CurrentEpisodeIndexChangedEventArg
{
    public int NewValue { get; set; }
    public int OldValue { get; set; }
    public string Affiliation { get; set; }

    public CurrentEpisodeIndexChangedEventArg(string affiliation, int oldValue, int newValue)
    {
        Affiliation = affiliation;
        OldValue = oldValue;
        NewValue = newValue;
    }
}