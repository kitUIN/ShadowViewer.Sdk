namespace ShadowViewer.Utils
{
    public class ShadowFile : IDisposable
    {
        public IStorageItem Self { get; set; }
        public int Depth { get; set; } = 0 ;
        public int Counts { get; set; } = 0;
        public long Size { get; set; } = 0;
        public bool IsDirectory { get => Self is StorageFolder; }
        public List<ShadowFile> Children { get; } = new List<ShadowFile>();
        ShadowFile(IStorageItem item) 
        {
            Self = item;
        }
        private async Task LoadChildren()
        {
            if(Self is StorageFolder folder)
            {
                foreach (IStorageItem item in await folder.GetItemsAsync())
                {
                    ShadowFile file = new ShadowFile(item);
                    await file.LoadChildren();
                    Children.Add(file);
                }
                if(Children.Count == 0)
                {
                    Size = 0;
                    Depth = 0;
                    Counts = 1;
                }
                else
                {
                    Size = Children.Sum(x => x.Size);
                    Depth = Children.Max(x => x.Depth) + 1;
                    Counts = Children.Sum(x => x.Counts) + 1;
                }
                
            }
            else if(Self is StorageFile file)
            {
                Size = (long)(await file.GetBasicPropertiesAsync()).Size;
                Depth = 0;
                Counts = 1;
            }
            
        }
        public static async Task<ShadowFile> Create(IStorageItem item)
        {
            ShadowFile f = new ShadowFile(item);
            await f.LoadChildren();
            return f;
        }
        public static List<ShadowFile> GetDepthFiles(ShadowFile root,int depth = 1)
        {
            if (root.Depth == depth && root.IsDirectory)
            {
                return new List<ShadowFile> { root };
            }
            else
            {
                List<ShadowFile> result = new List<ShadowFile>();
                foreach (ShadowFile child in root.Children)
                {
                    result.AddRange(GetDepthFiles(child, depth));
                }
                return result;
            }
        }
        /// <summary>
        /// 转化为本地漫画
        /// </summary>
        /// <param name="root"></param>
        /// <param name="comicId"></param>
        public static void ToLocalComic(ShadowFile root, string comicId)
        {
            List<ShadowFile> one = GetDepthFiles(root);
            int order = 1;
            foreach (ShadowFile child in one)
            {
                LocalEpisode ep = LocalEpisode.Create(((StorageFolder)child.Self).DisplayName, order, comicId, child.Children.Count, child.Size);
                ep.Add();
                order++;
                foreach (ShadowFile item in child.Children)
                {
                    LocalPicture pic = LocalPicture.Create(((StorageFile)item.Self).DisplayName, ep.Id, comicId, item.Self.Path, item.Size);
                    pic.Add();
                }
            }
            if (DBHelper.Db.Queryable<LocalComic>().First(x => x.Id == comicId) is LocalComic comic)
            {
                comic.EpisodeCounts = DBHelper.Db.Queryable<LocalEpisode>().Where(x => x.ComicId == comicId).Count();
                comic.Counts = DBHelper.Db.Queryable<LocalPicture>().Where(x => x.ComicId == comicId).Count();
                comic.Update();
            }
        } 
        /// <summary>
        /// 销毁资源
        /// </summary>
        public void Dispose()
        {
             
        }
    }
}
