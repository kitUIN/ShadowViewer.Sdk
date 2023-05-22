 

namespace ShadowViewer.Helpers
{
    public static class ComicHelper
    {
        public static Dictionary<string, ShadowEntry> Entrys { get; private set; } = new Dictionary<string, ShadowEntry>();
        public static LocalComic CreateFolder(string name, string parent)
        { 
            if (name == "") name = I18nHelper.GetString("Shadow.String.CreateFolder.Title");
            return LocalComic.Create(name, "", img: "ms-appx:///Assets/Default/folder.png", parent: parent, isFolder: true, percent:"");
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
        public static LocalComic ImportComicsFromEntry(string path, string parent, string img,long size)
        {
            if (!Entrys.ContainsKey(path)) return null; 
            string fileName = Path.GetFileNameWithoutExtension(path).Split(new char[] { '\\', '/' }).Last();
            return LocalComic.Create(fileName, path, img: img, parent:parent,   size: size, isTemp:true,isFromZip:true);
        }
        public static async Task<LocalComic> ImportComicsFromZip(string path, string imgPath)
        {
            Entrys[path] = await CompressHelper.DeCompress(path);
            string img = LoadImgFromEntry(Entrys[path], imgPath);
            long size = Entrys[path].Size;
            LocalComic comic = ImportComicsFromEntry(path, "local", img, size);
            comic.Add();
            return comic;
        }
        public static void EntryToComic(string comicPath,LocalComic comic, string path)
        {
            string uri = System.IO.Path.Combine(comicPath, comic.Id);
            CompressHelper.DeCompress(path, uri);
            comic.IsTemp = false;
            comic.Link = uri;
            ShadowEntry.InitLocal(Entrys[path], uri, comic.Id);
            Entrys[path].Dispose();
            GC.SuppressFinalize(Entrys[path]); // 销毁资源
            Entrys.Remove(path);
        }

        public static string ShowSize(long size)
        {
            long KB = 1024;
            long MB = KB * 1024;
            long GB = MB * 1024;
            if (size / GB >= 1)
            {
                return $"{Math.Round(size / (float)GB, 2)} GB";
            }
            else if (size / MB >= 1)
            {
                return $"{Math.Round(size / (float)MB, 2)} MB";
            }
            else if (size / KB >= 1)
            {
                return $"{Math.Round(size / (float)KB, 2)} KB";
            }
            return $"{size} B";
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
