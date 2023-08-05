using SqlSugar;

namespace ShadowViewer.Models
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
            this.comic = LocalComic.Create(CoreResourcesHelper.GetString(CoreResourceKey.LocalTag), "",id:"local",isFolder:true, parent:"",img: LocalComic.DefaultFolderImg);
            var db = DiFactory.Current.Services.GetService<ISqlSugarClient>();
            List<LocalComic> comics = db.Queryable<LocalComic>().Where(x => x.IsFolder).ToList();
            if(comics.Count > 0)
            {
                Children = comics.Where(x => !black.Contains(x.Id)).Select(x=> new ShadowPath(x)).ToList();
            }
        }
    }
}
