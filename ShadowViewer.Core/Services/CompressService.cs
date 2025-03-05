using System;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.IO;
using ReaderOptions = SharpCompress.Readers.ReaderOptions;
using System.Threading;
using SqlSugar;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DryIoc;
using Serilog;
using ShadowPluginLoader.WinUI;
using Microsoft.UI.Xaml;
using ShadowPluginLoader.MetaAttributes;
using ShadowViewer.Core.Cache;
using ShadowViewer.Core.Models;
using ShadowViewer.Core.Extensions;
using ShadowViewer.Core.Helpers;
using ShadowViewer.Core.I18n;

namespace ShadowViewer.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CompressService
    {
        [Autowired] private ILogger Logger { get; }

        [Autowired] private ICallableService Caller { get; }
        [Autowired] private ISqlSugarClient Db { get; }
        //
        // /// <summary>
        // /// 检测压缩包密码是否正确
        // /// </summary>
        // public static bool CheckPassword(string zip, ref ReaderOptions readerOptions)
        // {
        //     var md5 = EncryptingHelper.CreateMd5(zip);
        //     var sha1 = EncryptingHelper.CreateSha1(zip);
        //     var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        //     var cacheZip = db.Queryable<CacheZip>().First(x => x.Sha1 == sha1 && x.Md5 == md5);
        //     if (cacheZip is { Password: not null } && cacheZip.Password != "")
        //     {
        //         readerOptions = new ReaderOptions() { Password = cacheZip.Password };
        //         Log.Information("自动填充密码:{Pwd}", cacheZip.Password);
        //     }
        //
        //     try
        //     {
        //         using var fStream = File.OpenRead(zip);
        //         using var stream = NonDisposingStream.Create(fStream);
        //         using var archive = ArchiveFactory.Open(stream, readerOptions);
        //         foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
        //         {
        //             using var entryStream = entry.OpenEntryStream();
        //             // 密码正确添加压缩包密码存档
        //             // 能正常打开一个entry就代表正确,所以这个循环只走了一次
        //             if (cacheZip == null && readerOptions?.Password != null ||
        //                 cacheZip is { Password: null } && readerOptions?.Password != null)
        //             {
        //                 var cache = CacheZip.Create(md5, sha1, password: readerOptions.Password);
        //                 db.Storageable(cache).ExecuteCommand();
        //             }
        //
        //             return true;
        //         }
        //
        //         return true;
        //     }
        //     catch (Exception ex)
        //     {
        //         // 密码错误就删除压缩包密码存档
        //         db.DeleteableByObject(cacheZip).ExecuteCommand();
        //         Log.Error("解压出错:{Ex}", ex);
        //         return false;
        //     }
        // }

        /// <summary>
        /// 直接解压
        /// </summary>
        public static async Task DeCompressAsync(string zip, string destinationDirectory,
            Action<double>? report = null, XamlRoot? root = null, string? pwd = null,
            CancellationToken cancellationToken = default)
        {
            // Logger.Information("进入解压流程");
            var readerOptions = new ReaderOptions() { Password = pwd };
            try
            {
                await using var fStream = File.OpenRead(zip);
                await using var stream = NonDisposingStream.Create(fStream);
                using var archive = ArchiveFactory.Open(stream, readerOptions);
                archive.ExtractToDirectory(destinationDirectory, report, cancellationToken);
            }
            catch (CryptographicException ex)
            {
                if (root != null)
                {
                    var dialog = XamlHelper.CreateOneTextBoxDialog(root,
                        Path.GetFileName(zip) + I18N.PasswordError,
                        "", I18N.ZipPasswordPlaceholder, "",
                        async (sender, args, text) =>
                        {
                            sender.Hide();
                            await DeCompressAsync(zip, destinationDirectory, report, root, text);
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
        /// 解压压缩包并且导入
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="destinationDirectory"></param>
        /// <param name="comicId"></param>
        /// <param name="thumbProgress"></param>
        /// <param name="token"></param>
        /// <param name="readerOptions"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<object> DeCompressImportAsync(string zip, string destinationDirectory,
            CancellationToken token,
            IProgress<MemoryStream>? thumbProgress = null, 
            IProgress<double>? progress = null,
            ReaderOptions? readerOptions = null)
        {
            var comicId = SnowFlakeSingle.Instance.NextId();
            Logger.Information("进入{Zip}解压流程", zip);
            var path = Path.Combine(destinationDirectory, comicId.ToString());
            var md5 = EncryptingHelper.CreateMd5(zip);
            var sha1 = EncryptingHelper.CreateSha1(zip);
            var start = DateTime.Now;
            var cacheZip = await Db.Queryable<CacheZip>().FirstAsync(x => x.Sha1 == sha1 && x.Md5 == md5, token);
            // cacheZip ??= CacheZip.Create(md5, sha1);
            if (cacheZip.ComicId != null)
            {
                comicId = (long)cacheZip.ComicId;
                path = Path.Combine(destinationDirectory, comicId.ToString());
                // 缓存文件未被删除
                if (Directory.Exists(cacheZip.CachePath))
                {
                    Logger.Information("{Zip}文件存在缓存记录,直接载入漫画{cid}", zip, cacheZip.ComicId);
                    return cacheZip;
                }
            }

            await using var fStream = File.OpenRead(zip);
            await using var stream = NonDisposingStream.Create(fStream);
            if (token.IsCancellationRequested) throw new TaskCanceledException();
            using var archive = ArchiveFactory.Open(stream, readerOptions);
            if (token.IsCancellationRequested) throw new TaskCanceledException();
            var total = archive.Entries.Where(
                    entry => !entry.IsDirectory && (entry.Key?.IsPic() ?? false))
                .OrderBy(x => x.Key);
            if (token.IsCancellationRequested) throw new TaskCanceledException();
            var totalCount = total.Count();
            var ms = new MemoryStream();
            if (total.FirstOrDefault() is { } img)
            {
                await using (var entryStream = img.OpenEntryStream())
                {
                    await entryStream.CopyToAsync(ms, token);
                }
                var bytes = ms.ToArray();
                // CacheImg.CreateImage(CoreSettings.TempPath, bytes, comicId.ToString());
                thumbProgress?.Report(new MemoryStream(bytes));
            }

            Logger.Information("开始解压:{Zip}", zip);

            var i = 0;
            path.CreateDirectory();
            foreach (var entry in total)
            {
                if (token.IsCancellationRequested) throw new TaskCanceledException();
                entry.WriteToDirectory(path, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                i++;
                var result = i / (double)totalCount;
                progress?.Report(Math.Round(result * 100, 2) - 0.01D);
            }

            var node = ShadowTreeNode.FromFolder(path);
            var stop = DateTime.Now;
            cacheZip.ComicId = comicId;
            cacheZip.CachePath = path;
            cacheZip.Name = Path.GetFileNameWithoutExtension(zip)
                .Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
            await Db.Storageable(cacheZip).ExecuteCommandAsync(token);
            Logger.Information("解压成功:{Zip} 页数:{Pages} 耗时: {Time} s", zip, totalCount, (stop - start).TotalSeconds);
            return node;
        }
    }
}