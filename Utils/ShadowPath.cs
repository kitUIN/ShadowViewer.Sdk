namespace ShadowViewer.Utils
{
    /// <summary>
    /// 路径树
    /// </summary>
    public class ShadowPath
    {
        private readonly LocalComic comic;
        public string Name { get => comic.Name; }
        public string Id { get => comic.Id; }
        public string Img { get => comic.Img; }
        public bool IsFolder { get => comic.IsFolder; }
        public List<ShadowPath> Children { get; } = new List<ShadowPath>();
        public ShadowPath(LocalComic comic)
        {
            this.comic = comic;
        }
        public void SetSize(long size)
        {
            if(comic.Parent != "")
            {
                comic.Size = size;
                comic.Update();
            }
        }
        public ShadowPath(IEnumerable<string> black)
        {
            this.comic = LocalComic.Create(I18nHelper.GetString("Shadow.Tag.Local"), "",id:"local",isFolder:true, parent:"",img: "ms-appx:///Assets/Default/folder.png");
            List<LocalComic> comics = DBHelper.Db.Queryable<LocalComic>().Where(x => x.IsFolder).ToList();
            if(comics.Count > 0)
            {
                Children = comics.Where(x => !black.Contains(x.Id)).Select(x=> new ShadowPath(x)).ToList();
            }
        }
    }
}
