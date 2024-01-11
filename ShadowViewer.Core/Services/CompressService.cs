using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.IO;
using ReaderOptions = SharpCompress.Readers.ReaderOptions;
using System.Threading;
using ShadowViewer.Extensions;
using SqlSugar;
using System.Diagnostics;
using ShadowViewer.Cache;

namespace ShadowViewer.Services
{
    public class CompressService
    {
        private ILogger Logger { get; } 
        private readonly ICallableService caller;
        public CompressService(ICallableService callableService,ILogger logger)
        {
            caller = callableService;
            Logger = logger;
        }
        /// <summary>
        /// 检测压缩包密码是否正确
        /// </summary>
        public static bool CheckPassword(string zip, ref ReaderOptions readerOptions)
        {
            var md5 = EncryptingHelper.CreateMd5(zip);
            var sha1 = EncryptingHelper.CreateSha1(zip);
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            var cacheZip = db.Queryable<CacheZip>().First(x => x.Sha1 == sha1 && x.Md5 == md5);
            if (cacheZip is { Password: not null } && cacheZip.Password != "")  
            {
                readerOptions = new ReaderOptions() { Password = cacheZip.Password };
                Log.Information("自动填充密码:{Pwd}", cacheZip.Password);
            }
            try
            {
                using var fStream = File.OpenRead(zip);
                using var stream = NonDisposingStream.Create(fStream);
                using var archive = ArchiveFactory.Open(stream, readerOptions);
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    using var entryStream = entry.OpenEntryStream();
                    // 密码正确添加压缩包密码存档
                    // 能正常打开一个entry就代表正确,所以这个循环只走了一次
                    if (cacheZip == null && readerOptions?.Password != null || cacheZip is { Password: null } && readerOptions?.Password != null)
                    {
                        var cache = CacheZip.Create(md5, sha1, password: readerOptions.Password);
                        db.Storageable(cache).ExecuteCommand();
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                // 密码错误就删除压缩包密码存档
                db.DeleteableByObject(cacheZip).ExecuteCommand();
                Log.Error("解压出错:{Ex}", ex);
                return false;
            }
        }
        /// <summary>
        /// 直接解压
        /// </summary>
        public static async Task DeCompress(string zip, string destinationDirectory,
            Action<double>? report=null, XamlRoot? root = null, string? pwd = null,
            CancellationToken cancellationToken = default)
        {
            var readerOptions = new ReaderOptions() { Password = pwd }; 
            try
            {
                using FileStream fStream = File.OpenRead(zip);
                using NonDisposingStream stream = NonDisposingStream.Create(fStream);
                using (IArchive archive = ArchiveFactory.Open(stream, readerOptions))
                {
                    archive.ExtractToDirectory(destinationDirectory, report, cancellationToken);
                }
            }
            catch (SharpCompress.Common.CryptographicException ex)
            {
                if(root != null)
                {
                    var dialog = XamlHelper.CreateOneTextBoxDialog(root,
                        ResourcesHelper.GetString(ResourceKey.PasswordError),
                        "密码",
                        "请输入压缩包密码", "",
                    async (sender, args, text) =>
                    {
                        sender.Hide();
                        await DeCompress(zip, destinationDirectory, report, root, text);
                    });
                    await dialog.ShowAsync();
                }
                else
                {
                    Log.Error("解压出错:{Ex}", ex);
                }
            }
            catch (Exception ex)
            {
                Log.Error("解压出错:{Ex}", ex);
            }
        }

        /// <summary>
        /// 解压流程
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="destinationDirectory"></param>
        /// <param name="comicId"></param>
        /// <param name="token"></param>
        /// <param name="readerOptions"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<object> DeCompressAsync(string zip, string destinationDirectory,
            string comicId, CancellationToken token, ReaderOptions readerOptions = null)
        {
            Logger.Information("进入解压流程");
            var path = Path.Combine(destinationDirectory, comicId);
            ShadowEntry root = new ShadowEntry()
            {
                Name = zip.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last(),
            };
            string md5 = EncryptingHelper.CreateMd5(zip);
            string sha1 = EncryptingHelper.CreateSha1(zip);
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            var start = DateTime.Now;
            CacheZip cacheZip = db.Queryable<CacheZip>().First(x => x.Sha1 == sha1 && x.Md5 == md5);
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
                using var archive = ArchiveFactory.Open(stream, readerOptions);
                if (token.IsCancellationRequested) throw new TaskCanceledException();
                var total = archive.Entries.Where(entry => !entry.IsDirectory && entry.Key.IsPic()).OrderBy(x => x.Key);
                if (token.IsCancellationRequested) throw new TaskCanceledException();
                var totalCount = total.Count();
                var ms = new MemoryStream();
                if (total.FirstOrDefault() is IArchiveEntry img)
                {
                    await using (var entryStream = img.OpenEntryStream())
                    {
                        await entryStream.CopyToAsync(ms);
                        // ms.Seek(0, SeekOrigin.Begin);
                    }
                    var bytes = ms.ToArray();
                    CacheImg.CreateImage(Config.TempPath, bytes, comicId);
                    caller.ImportComicThumb(new MemoryStream(bytes));
                }
                Logger.Information("开始解压:{Zip}", zip);
                
                var i = 0;
                path.CreateDirectory();
                foreach (IArchiveEntry entry in total)
                {
                    if (token.IsCancellationRequested) throw new TaskCanceledException();
                    entry.WriteToDirectory(path, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                    i++;
                    double result = (double)i / (double)totalCount;
                    caller.ImportComicProgress(Math.Round(result * 100, 2));
                    ShadowEntry.LoadEntry(entry, root);
                }
                root.LoadChildren();
                var stop = DateTime.Now;
                cacheZip.ComicId = comicId;
                cacheZip.CachePath = path;
                cacheZip.Name = Path.GetFileNameWithoutExtension(zip).Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                db.Storageable(cacheZip).ExecuteCommand();
                Logger.Information("解压:{Zip} 页数:{Pages} 耗时: {Time} s", zip, totalCount, (stop - start).TotalSeconds);
            }
            return root;
        }
    }
}
