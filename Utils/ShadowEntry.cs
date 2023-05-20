namespace ShadowViewer.Utils
{
    public class ShadowEntry
    {
        public string Name { get; set; }
        public MemoryStream Source { get => stream; set => stream = value; }
        private MemoryStream stream;
        public int Depth { get; set; } = 0;
        public int Counts { get; set; } = 0;
        public long Size { get; set; } = 0;
        public bool IsDirectory { get => Children.Count > 0; }
        public List<ShadowEntry> Children { get; } = new List<ShadowEntry>();
        public ShadowEntry(){ }
         
    }
}
