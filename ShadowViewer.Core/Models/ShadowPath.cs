using SqlSugar;

namespace ShadowViewer.Models
{
    /// <summary>
    /// 路径树
    /// </summary>
    public class ShadowPath
    {
        private readonly LocalComic comic;
        public string Name => comic.Name;
        public string Id => comic.Id;
        public string Img => comic.Img;
        public bool IsFolder => comic.IsFolder;
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
            this.comic = LocalComic.Create(ResourcesHelper.GetString(ResourceKey.LocalTag), "",id:"local",isFolder:true, parent:"",img: LocalComic.DefaultFolderImg);
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            List<LocalComic> comics = db.Queryable<LocalComic>().Where(x => x.IsFolder).ToList();
            if(comics.Count > 0)
            {
                Children = comics.Where(x => !black.Contains(x.Id)).Select(x=> new ShadowPath(x)).ToList();
            }
        }
    }
}
