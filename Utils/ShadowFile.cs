using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Extensions;
using SqlSugar;

namespace ShadowViewer.Utils
{
    public class ShadowFile : IDisposable
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Depth { get; set; } = 0 ;
        public int Counts { get; set; } = 0;
        public long Size { get; set; } = 0;
        public bool IsDirectory { get; set; }
        public List<ShadowFile> Children { get; } = new List<ShadowFile>();
        public ShadowFile(string folder) 
        {
            GetFiles(folder, this);
        }
        public ShadowFile(StorageFolder folder) : this(folder.Path) { }
        public ShadowFile() { }

        private void GetFiles(string filePath, ShadowFile node)
        {
            if (filePath.IsFolder())
            {
                DirectoryInfo folder = new DirectoryInfo(filePath);
                node.Name = folder.Name;
                node.Path = folder.FullName;
                node.IsDirectory = true;
                FileInfo[] chldFiles = folder.GetFiles("*.*");
                foreach (FileInfo chlFile in chldFiles)
                {
                    if (chlFile.FullName.IsPic())
                    {
                        ShadowFile chldNode = new ShadowFile()
                        {
                            Name = chlFile.Name,
                            Path = chlFile.FullName,
                            Size = chlFile.Length,
                            Depth = 0,
                            Counts = 1,
                            IsDirectory=false,
                        };
                        node.Size += chlFile.Length;
                        node.Counts += 1;
                        node.Children.Add(chldNode);
                    }
                }
                DirectoryInfo[] chldFolders = folder.GetDirectories();
                foreach (DirectoryInfo chldFolder in chldFolders)
                {
                    ShadowFile chldNode = new ShadowFile();
                    node.Children.Add(chldNode);
                    GetFiles(chldFolder.FullName, chldNode);
                    node.Size += chldNode.Size;
                    node.Counts += chldNode.Counts;
                }
                if(node.Children.Count > 0)
                {
                    node.Depth = node.Children.Max(x => x.Depth) + 1;
                }
                else
                {
                    node.Depth = 1;
                }
                
            }
            else if (filePath.IsFile())
            {
                return;
            }
        }
        public static List<ShadowFile> GetDepthFiles(ShadowFile root, int depth = 1)
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
            var db = DiFactory.Current.Services.GetService<ISqlSugarClient>();
            
            List<ShadowFile> one = GetDepthFiles(root);
            int order = 1;
            foreach (ShadowFile child in one)
            {
                if(child.Children.Count > 0)
                {
                    LocalEpisode ep = LocalEpisode.Create(child.Name, order, comicId, child.Children.Count, child.Size);
                    db.Insertable(ep).ExecuteCommand();
                    order++;
                    foreach (ShadowFile item in child.Children)
                    {
                        LocalPicture pic = LocalPicture.Create(child.Name, ep.Id, comicId, item.Path, item.Size);
                        db.Insertable(pic).ExecuteCommand();
                    }
                }
                
            }
            if (db.Queryable<LocalComic>().First(x => x.Id == comicId) is LocalComic comic)
            {
                comic.EpisodeCounts = db.Queryable<LocalEpisode>().Where(x => x.ComicId == comicId).Count();
                comic.Counts = db.Queryable<LocalPicture>().Where(x => x.ComicId == comicId).Count();
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
