namespace ShadowViewer.Args;

public class ImportPluginEventArg
{
    public IReadOnlyList<IStorageItem> Items { get; }

    public ImportPluginEventArg(IReadOnlyList<IStorageItem> items)
    {
        Items = items;
    }
}