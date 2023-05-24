using SharpCompress.Common;

namespace ShadowViewer.Helpers
{
    public class ComicHelper
    {
        public static ILogger Logger { get; } = Log.ForContext<ComicHelper>();
        public static LocalComic CreateFolder(string name, string parent)
        { 
            if (name == "") name = I18nHelper.GetString("Shadow.String.CreateFolder.Title");
            return LocalComic.Create(name, "", img: "ms-appx:///Assets/Default/folder.png", parent: parent, isFolder: true, percent:"");
        }
        /// <summary>
        /// 从文件夹导入漫画
        /// </summary> 
        public static async Task<LocalComic> ImportComicsFromFolder(StorageFolder folder,string parent)
        {
            static ShadowFile Cycle(List<ShadowFile> entries)
            {
                ShadowFile imgEntry = null;
                foreach (ShadowFile item in entries)
                {
                    imgEntry = item.Children.FirstOrDefault(x => x.Self is StorageFile f && f.IsPic());
                    if (imgEntry != null) return imgEntry;
                }
                return null;
            }
            ShadowFile root = await ShadowFile.Create(folder);
            List<ShadowFile> two = ShadowFile.GetDepthFiles(root, 2);
            ShadowFile imgEntry = Cycle(two);
            if (imgEntry == null)
            {
                two = ShadowFile.GetDepthFiles(root, 1);
                imgEntry = Cycle(two);
            } 
            LocalComic comic = LocalComic.Create(((StorageFolder)root.Self).DisplayName, root.Self.Path, img: imgEntry?.Self.Path, parent: parent, size: root.Size);
            comic.Add();
            ShadowFile.ToLocalComic(root, comic.Id);
            root.Dispose();
            return comic;
        }
        /// <summary>
        /// 从缓存流中加载缩略图
        /// </summary> 
        public static string LoadImgFromEntry(ShadowEntry root, string dir)
        {
            static ShadowEntry Cycle(List<ShadowEntry> entries)
            {
                ShadowEntry imgEntry = null;
                foreach (ShadowEntry item in entries)
                {
                    imgEntry = item.Children.FirstOrDefault(x => !x.IsDirectory);
                    if (imgEntry != null) return imgEntry;
                }
                return null;
            }
            List<ShadowEntry> two = ShadowEntry.GetDepthEntries(root, 2);
            ShadowEntry imgEntry = Cycle(two); 
            if (imgEntry == null)
            {
                two = ShadowEntry.GetDepthEntries(root, 1);
                imgEntry = Cycle(two);
            } 
            return Path.Combine(dir, imgEntry.Path);
        }
        public static async Task<string> ZipPasswordDialog(XamlRoot xamlRoot)
        {
            string password = "";
            ContentDialog dialog = XamlHelper.CreateOneLineTextBoxDialog(
                I18nHelper.GetString("Shadow.String.ZipPassword"), xamlRoot, "");
            dialog.PrimaryButtonClick += (s, e) =>
            {
                password = ((TextBox)((StackPanel)((StackPanel)dialog.Content).Children[0]).Children[1]).Text;
            };
            await dialog.ShowAsync();
            return password;
        }
         
        /// <summary>
        /// Long 大小 转换成 字符串型 大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
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
