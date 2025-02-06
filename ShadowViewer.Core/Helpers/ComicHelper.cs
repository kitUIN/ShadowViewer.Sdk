using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DryIoc;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Cache;
using ShadowViewer.Enums;
using ShadowViewer.Extensions;
using ShadowViewer.Models;
using SqlSugar;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace ShadowViewer.Helpers
{
    /// <summary>
    /// 本地漫画的一些帮助函数
    /// </summary>
    public class ComicHelper
    {
        public static ILogger Logger { get; } = Log.ForContext<ComicHelper>();
        // /// <summary>
        // /// 新建文件夹
        // /// </summary>
        // public static LocalComic CreateFolder(string name, string parent)
        // {
        //     string defaultName = ResourcesHelper.GetString(ResourceKey.CreateFolder);
        //     if (name == "") name = defaultName;
        //     int i = 1;
        //     var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        //     while (db.Queryable<LocalComic>().Any(x => x.Name == name))
        //     {
        //         name = $"{defaultName}({i++})";
        //     }
        //     return LocalComic.Create(name, "", img: LocalComic.DefaultFolderImg, parent: parent, isFolder: true, percent:"");
        // }
        // /// <summary>
        // /// 从文件夹导入漫画
        // /// </summary>
        // public static LocalComic ImportComicsFromFolder(StorageFolder folder, string parent, string? comicId = null, string? comicName = null)
        // {
        //     string img;
        //     var root = ShadowTreeNode.FromFolder(folder);
        //     var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        //     if (db.Queryable<CacheImg>().First(x => x.ComicId==comicId) is { } cacheImg)
        //     {
        //         img = cacheImg.Path;
        //     }
        //     else
        //     {
        //         static ShadowTreeNode? Cycle(List<ShadowTreeNode> entries)
        //         {
        //             return entries.Select(item => item.Children.FirstOrDefault(x => !x.IsDirectory)).OfType<ShadowTreeNode>().FirstOrDefault();
        //         }
        //         var two = root.GetDepthFiles(2);
        //         var imgEntry = Cycle(two);
        //         if (imgEntry == null)
        //         {
        //             two = root.GetDepthFiles(1);
        //             imgEntry = Cycle(two);
        //         }
        //         if (imgEntry == null)
        //         {
        //             throw new Exception("无效文件夹");
        //         }
        //         img = imgEntry!.Path;
        //     }
        //     var comic = LocalComic.Create(comicName ?? root.Name, root.Path, img: img, parent: parent, size: root.Size, id: comicId);
        //     db.Insertable(comic).ExecuteCommand();
        //     ShadowTreeNode.ToLocalComic(root, comic.Id);
        //     root.Dispose();
        //     return comic;
        // }
        /// <summary>
        /// 从缓存流中加载缩略图
        /// </summary>
        public static string LoadImgFromEntry(ShadowEntry root, string dir, string comicId)
        {
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            if (db.Queryable<CacheImg>().First(x => x.ComicId==comicId) is CacheImg cacheImg)
            {
                return cacheImg.Path;
            }
            static ShadowEntry? Cycle(List<ShadowEntry> entries)
            {
                ShadowEntry? imgEntry = null;
                foreach (ShadowEntry item in entries)
                {
                    imgEntry = item.Children.FirstOrDefault(x => !x.IsDirectory);
                    if (imgEntry != null) return imgEntry;
                }
                return null;
            }
            List<ShadowEntry> two = ShadowEntry.GetDepthEntries(root, 2);
            var imgEntry = Cycle(two);
            if (imgEntry == null)
            {
                two = ShadowEntry.GetDepthEntries(root, 1);
                imgEntry = Cycle(two);
            }
            return Path.Combine(dir, imgEntry!.Path);
        }
        // /// <summary>
        // /// 检查重复导入
        // /// </summary>
        // public static async Task<bool> CheckImportAgain(XamlRoot xamlRoot, string? zip = null, string? path = null)
        // {
        //     if (!ConfigHelper.GetBoolean("LocalIsImportAgain"))
        //     {
        //         ContentDialog dialog = XamlHelper.CreateMessageDialog(xamlRoot,
        //         ResourcesHelper.GetString(ResourceKey.ImportError),
        //         ResourcesHelper.GetString(ResourceKey.DuplicateImport));
        //         var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        //         if (zip != null)
        //         {
        //             string md5 = EncryptingHelper.CreateMd5(zip);
        //             string sha1 = EncryptingHelper.CreateSha1(zip);
        //             var cache = db.Queryable<CacheZip>().First(x => x.Sha1 == sha1 && x.Md5 == md5);
        //             if (cache != null)
        //             {
        //                 var comic = db.Queryable<LocalComic>().First(x => x.Id == cache.ComicId);
        //                 if (comic != null)
        //                 {
        //                     await dialog.ShowAsync();
        //                     return true;
        //                 }
        //             }
        //         }
        //         else if (path != null)
        //         {
        //             LocalComic comic = db.Queryable<LocalComic>().First(x => x.Link == path);
        //             if (comic != null)
        //             {
        //                 await dialog.ShowAsync();
        //                 return true;
        //             }
        //         }
        //     }
        //     return false;
        //
        // }
        
    }
}
