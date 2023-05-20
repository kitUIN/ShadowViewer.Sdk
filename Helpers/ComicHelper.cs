 

namespace ShadowViewer.Helpers
{
    public static class ComicHelper
    {
        public static Dictionary<string, ShadowEntry> Entrys { get; private set; } = new Dictionary<string, ShadowEntry>();
        public static LocalComic CreateFolder(string name,string img, string parent)
        {
            string id = Guid.NewGuid().ToString("N");
            while (DBHelper.Db.Queryable<LocalComic>().Any(x => x.Id == id))
            {
                id = Guid.NewGuid().ToString("N");
            }
            var time = DateTime.Now;
            if (img == "") { img = "ms-appx:///Assets/Default/folder.png"; }
            if (name == "") { name = id; }
            var comic =  new LocalComic(id, name, time, time, id, img: img, parent: parent, isFolder: true, percent:"");
            return comic;
        }
        public static LocalComic CreateComic(string name, string img, string parent, string link, string affiliation = "Local",long size=0,string id=null,bool isTemp=false)
        {
            if (id == null)
            {
                id = Guid.NewGuid().ToString("N");
                while (DBHelper.Db.Queryable<LocalComic>().Any(x => x.Id == id))
                {
                    id = Guid.NewGuid().ToString("N");
                }
            }
            if(img is null) { img = "ms-appx:///Assets/Default/DefaultComic.png"; }
            var time = DateTime.Now;
            var comic = new LocalComic(id, name, time, time, link, img: img, size: size,
                affiliation: affiliation, parent: parent, isTemp: isTemp);
            return comic;
        } 
        public static string LoadImgFromEntry(ShadowEntry entry,string dir)
        {
            MemoryStream Cycle(ShadowEntry entry)
            {
                ShadowEntry temp = entry.Children.FirstOrDefault(x => !x.IsDirectory && x.Source != null);
                if(temp is null)
                {
                    foreach (ShadowEntry item in entry.Children)
                    {
                        return Cycle(item);
                    }
                }
                else
                {
                    return temp.Source;
                }
                return null;
            }

            var stream = Cycle(entry);
            if (stream is null) return null;
            string img = System.IO.Path.Combine(dir, Guid.NewGuid().ToString("N") + ".png");
            using (var fileStream = new FileStream(img, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(fileStream);
            }
            return img;
        }
        public static LocalComic ImportComicsFromEntry(string path, string parent, string img)
        {
            if (!Entrys.ContainsKey(path)) return null;
            ShadowEntry entry = Entrys[path];
            string fileName = Path.GetFileNameWithoutExtension(path).Split(new char[] { '\\', '/' }).Last();
            return CreateComic(fileName, img, parent, path, size: entry.Size , isTemp:true);

        }
        public static async Task<LocalComic> ImportComicsFromZip(string path, string imgPath)
        {
            Entrys[path] = await CompressHelper.DeCompress(path);
            var img = LoadImgFromEntry(Entrys[path], imgPath);
            LocalComic comic = ImportComicsFromEntry(path, "local", img);
            comic.Add();
            return comic;
        }
        public static void EntryToComic(string comicPath,LocalComic comic, string path)
        {
            string uri = System.IO.Path.Combine(comicPath, comic.Id);
            CompressHelper.DeCompress(path, uri);
            comic.IsTemp = false;
            comic.Link = uri;
            ComicHelper.Entrys.Remove(path);
        }
        /// <summary>
        /// 字母顺序A-Z
        /// </summary>
        public static int AZSort(LocalComic x, LocalComic y) => x.Name.CompareTo(y.Name);
        /// <summary>
        /// 字母顺序Z-A
        /// </summary>
        public static int ZASort(LocalComic x, LocalComic y) => y.Name.CompareTo(x.Name);
        /// <summary>
        /// 阅读时间早-晚
        /// </summary>
        public static int RASort(LocalComic x, LocalComic y) => x.LastReadTime.CompareTo(y.LastReadTime);
        /// <summary>
        /// 阅读时间晚-早(默认)
        /// </summary>
        public static int RZSort(LocalComic x, LocalComic y) => y.LastReadTime.CompareTo(x.LastReadTime);
        /// <summary>
        /// 创建时间早-晚
        /// </summary>
        public static int CASort(LocalComic x, LocalComic y) => x.CreateTime.CompareTo(y.CreateTime);
        /// <summary>
        /// 创建时间晚-早
        /// </summary>
        public static int CZSort(LocalComic x, LocalComic y) => y.CreateTime.CompareTo(x.CreateTime);
        /// <summary>
        /// 阅读进度小-大
        /// </summary>
        public static int PASort(LocalComic x, LocalComic y) => x.Percent.CompareTo(y.Percent);
        /// <summary>
        /// 阅读进度大-小
        /// </summary>
        public static int PZSort(LocalComic x, LocalComic y) => y.Percent.CompareTo(x.Percent);
        
    }
}
