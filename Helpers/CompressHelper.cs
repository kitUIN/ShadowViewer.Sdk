using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.IO;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Xml.Linq;
using ReaderOptions = SharpCompress.Readers.ReaderOptions;
using Serilog.Core;
using System.IO;
using NetTaste;
using System.Threading;

namespace ShadowViewer.Helpers
{
    public class CompressHelper
    {
        public static ILogger Logger { get; } = Log.ForContext<CompressHelper>();
        /// <summary>
        /// 检测压缩包密码是否正确
        /// </summary>
        public static bool CheckPassword(string zip, ReaderOptions readerOptions = null)
        {
            string md5 = EncryptingHelper.CreateMd5(zip);
            string sha1 = EncryptingHelper.CreateSha1(zip);
            CacheZip cacheZip = DBHelper.Db.Queryable<CacheZip>().First(x => x.Sha1 == sha1 && x.Md5 == md5);
            if (cacheZip != null && cacheZip.Password != null && cacheZip.Password != "")  
            {
                readerOptions = new ReaderOptions() { Password = cacheZip.Password };
                Logger.Information("自动填充密码:{pwd}", cacheZip.Password);
            }
            try
            {
                using (FileStream fStream = File.OpenRead(zip))
                using (NonDisposingStream stream = NonDisposingStream.Create(fStream))
                { 
                    using (IArchive archive = ArchiveFactory.Open(stream, readerOptions))
                    {
                        foreach (IArchiveEntry entry in archive.Entries.Where(entry => !entry.IsDirectory))
                        {
                            using (Stream entryStream = entry.OpenEntryStream())
                            {
                                // 密码正确添加压缩包密码存档
                                CacheZip cache = CacheZip.Create(md5, sha1, password: readerOptions?.Password);
                                cache.Add();
                                return true;
                            }
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // 密码错误就删除压缩包密码存档
                cacheZip?.Remove();
                Log.Error("解压出错:{Ex}", ex);
                return false;
            }
        }
        public static async Task<object> DeCompressAsync(string zip, string destinationDirectory, string comicId, IProgress<MemoryStream> imgAction, IProgress<double> progress,Action beginAction, CancellationToken token ,ReaderOptions readerOptions = null)
        {
            Logger.Information("进入解压流程");
            string path = Path.Combine(destinationDirectory, comicId);
            DateTime start;
            ShadowEntry root = new ShadowEntry()
            {
                Name = zip.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last(),
            };
            string md5 = EncryptingHelper.CreateMd5(zip);
            string sha1 = EncryptingHelper.CreateSha1(zip);
            CacheZip cacheZip = DBHelper.Db.Queryable<CacheZip>().First(x => x.Sha1 == sha1 && x.Md5 == md5);
            cacheZip ??= CacheZip.Create(md5, sha1);
            if (cacheZip.ComicId != null)
            {
                comicId = cacheZip.ComicId;
                path = Path.Combine(destinationDirectory, comicId);
                // 缓存文件未被删除
                if (Directory.Exists(cacheZip.CachePath))
                {
                    Logger.Information("{Zip}文件存在缓存记录,直接载入漫画{cid}", zip, cacheZip.ComicId);
                    return cacheZip;
                }
            }
            if (token.IsCancellationRequested) throw new TaskCanceledException();
            using (FileStream fStream = File.OpenRead(zip))
            using (NonDisposingStream stream = NonDisposingStream.Create(fStream))
            {
                if (token.IsCancellationRequested) throw new TaskCanceledException();
                using (IArchive archive = ArchiveFactory.Open(stream, readerOptions))
                {
                    if (token.IsCancellationRequested) throw new TaskCanceledException();
                    IEnumerable<IArchiveEntry> total = archive.Entries.Where(entry => !entry.IsDirectory && entry.Key.IsPic()).OrderBy(x=>x.Key);
                    if (token.IsCancellationRequested) throw new TaskCanceledException();
                    int totalCount = total.Count();
                    MemoryStream ms = new MemoryStream();
                    if(total.FirstOrDefault() is IArchiveEntry img)
                    {
                        using (Stream entryStream = img.OpenEntryStream())
                        {
                            await entryStream.CopyToAsync(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                        }
                        imgAction.Report(ms);
                    }
                    beginAction.Invoke();
                    Logger.Information("开始解压:{Zip}", zip);
                    start = DateTime.Now;
                    int i = 0;
                    path.CreateDirectory();
                    foreach (IArchiveEntry entry in total)
                    {
                        if (token.IsCancellationRequested) throw new TaskCanceledException();
                        entry.WriteToDirectory(path, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                        i++;
                        double result = (double)i / (double)totalCount;
                        progress.Report(Math.Round(result * 100, 2));
                        ShadowEntry.LoadEntry(entry, root);
                    }
                    root.LoadChildren();
                    DateTime stop = DateTime.Now;
                    cacheZip.ComicId = comicId;
                    cacheZip.CachePath = path;
                    cacheZip.Name = Path.GetFileNameWithoutExtension(zip).Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    if (DBHelper.Db.Queryable<CacheZip>().Any(x=>x.Id == cacheZip.Id))
                    {
                        cacheZip.Update();
                    }
                    else
                    {
                        cacheZip.Add();
                    }
                    Logger.Information("解压:{Zip} 页数:{Pages} 耗时: {Time} s", zip, totalCount, (stop - start).TotalSeconds);
                }
            }
            return root;
        }
    }
}
