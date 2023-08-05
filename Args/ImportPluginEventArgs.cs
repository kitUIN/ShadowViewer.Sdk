namespace ShadowViewer.Args;

public class ImportPluginEventArgs
{
    public IReadOnlyList<IStorageItem> Items { get; }

    public ImportPluginEventArgs(IReadOnlyList<IStorageItem> items)
    {
        Items = items;
    }
}