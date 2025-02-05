using System.Collections.Generic;
using Windows.Storage;

namespace ShadowViewer.Args
{
    public class ImportComicEventArgs
    {
        public IReadOnlyList<IStorageItem> Items { get; }
        public string[] Passwords { get; }
        public int Index { get; }
        public ImportComicEventArgs(IReadOnlyList<IStorageItem> items, string[] passwords, int index)
        {
            Items = items;
            Passwords = passwords;
            Index = index;
        }
    }
}
