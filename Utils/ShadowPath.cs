namespace ShadowViewer.Utils
{
    /// <summary>
    /// 路径树
    /// </summary>
    public class ShadowPath
    { 
        private LocalComic comic;
        public string Name { get => comic.Name; }
        public string Id { get => comic.Id; }
        public string Img { get => comic.Img; }
        public bool IsFolder { get => comic.IsFolder; }
        public List<ShadowPath> Children { get; } 
        public ShadowPath(LocalComic comic)
        {
            this.comic = comic;
            Children = new List<ShadowPath>();
        }

        public ShadowPath(IEnumerable<string> black)
        { 
            this.comic = LocalComic.Create("", "", img: "ms-appx:///Assets/Default/folder.png");
            Children = DBHelper.Db.Queryable<LocalComic>().Where(x => x.Parent == "local" && x.IsFolder&& !black.Contains(x.Id)).Select(c => new ShadowPath(c)).ToList();
        }
    }
}
