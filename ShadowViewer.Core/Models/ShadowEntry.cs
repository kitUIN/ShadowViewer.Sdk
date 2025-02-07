using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Extensions;
using SharpCompress.Archives;
using SqlSugar;

namespace ShadowViewer.Core.Models
{
    public class ShadowEntry : IDisposable
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; } = 0;
        /// <summary>
        /// 个数
        /// </summary>
        public int Counts { get; set; } = 0;
        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; } = 0;
        /// <summary>
        /// 地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 是否是文件夹
        /// </summary>
        public bool IsDirectory { get => Children.Count > 0; }
        /// <summary>
        /// 含有
        /// </summary>
        public List<ShadowEntry> Children { get; } = new List<ShadowEntry>();
        public ShadowEntry() { }
        public static void LoadEntry(IArchiveEntry entry, ShadowEntry root)
        {
            string[] names = entry.Key.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            ShadowEntry temp = root;
            ShadowEntry tmp = null;
            for (int i = 0; i < names.Length; i++)
            {
                tmp = temp.Children.FirstOrDefault(x => x.Name == names[i]);
                if (tmp is null)
                {
                    if (i == names.Length - 1)
                    {
                        if (entry.IsDirectory)
                        {
                            temp.Children.Add(new ShadowEntry()
                            {
                                Name = names[i],
                                Path = string.Join("/", names.Take(i + 1))
                            });
                        }
                        else if (names[i].IsPic())
                        {
                            temp.Children.Add(new ShadowEntry()
                            {
                                Name = names[i],
                                Size = entry.Size,
                                Path = string.Join("/", names.Take(i + 1))
                            });
                        }
                    }
                    else
                    {
                        temp.Children.Add(new ShadowEntry()
                        {
                            Name = names[i],
                            Path = string.Join("/", names.Take(i + 1))
                        });
                    }
                }
                else
                {
                    temp = tmp;
                }

            }
        }
        public void LoadChildren()
        {
            if (Children.Count > 0)
            {
                foreach (ShadowEntry child in Children)
                {
                    child.LoadChildren();
                }
                Size = Children.Sum(x => x.Size);
                Depth = Children.Max(x => x.Depth) + 1;
                Counts = Children.Sum(x => x.Counts);
            }
            else
            {
                Counts = 1;
            }
        }
        public static List<ShadowEntry> GetDepthEntries(ShadowEntry root, int depth = 1)
        {
            if (root.Depth == depth)
            {
                return new List<ShadowEntry> { root };
            }
            else
            {
                List<ShadowEntry> result = new List<ShadowEntry>();
                foreach (ShadowEntry child in root.Children)
                {
                    result.AddRange(GetDepthEntries(child, depth));
                }
                return result;
            }
        }
        /// <summary>
        /// 转化为本地漫画
        /// </summary>
        public static void ToLocalComic(ShadowEntry root, string initPath, long comicId)
        {
            var one = GetDepthEntries(root);
            var order = 0;
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            foreach (var child in one)
            {
                if (child.Children.Count > 0)
                {
                    var ep = LocalEpisode.Create(child.Name, order, comicId, child.Children.Count, child.Size);
                    db.Insertable(ep).ExecuteCommand();
                    order++;
                    foreach (var item in child.Children)
                    {
                        var pic = LocalPicture.Create(item.Name, ep.Id, comicId, System.IO.Path.Combine(initPath, item.Path), item.Size);
                        db.Insertable(pic).ExecuteCommand();
                    }
                }
            }
            // 销毁资源
            // root.Dispose();
            // if (db.Queryable<LocalComic>().First(x => x.Id == comicId) is LocalComic comic)
            // {
            //     comic.EpisodeCounts = db.Queryable<LocalEpisode>().Where(x => x.ComicId == comicId).Count();
            //     comic.Counts = db.Queryable<LocalPicture>().Where(x => x.ComicId == comicId).Count();
            //     comic.Update();
            // }
        }
        /// <summary>
        /// 销毁资源
        /// </summary>
        public void Dispose()
        {
        }
    }
}
