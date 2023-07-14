using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.IO;
using ReaderOptions = SharpCompress.Readers.ReaderOptions;
using System.Threading;

namespace ShadowViewer.Helpers
{
    public class CompressHelper
    {
        public static ILogger Logger { get; } = Log.ForContext<CompressHelper>();
        /// <summary>
        /// 检测压缩包密码是否正确
        /// </summary>
        public static bool CheckPassword(string zip, ref ReaderOptions readerOptions)
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
                using FileStream fStream = File.OpenRead(zip);
                using NonDisposingStream stream = NonDisposingStream.Create(fStream);
                using IArchive archive = ArchiveFactory.Open(stream, readerOptions);
                foreach (IArchiveEntry entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    using Stream entryStream = entry.OpenEntryStream();
                    // 密码正确添加压缩包密码存档
                    // 能正常打开一个entry就代表正确,所以这个循环只走了一次
                    if (cacheZip == null && readerOptions?.Password != null || cacheZip != null && cacheZip.Password == null && readerOptions?.Password != null)
                    {
                        CacheZip cache = CacheZip.Create(md5, sha1, password: readerOptions.Password);
                        cache.Add();
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                // 密码错误就删除压缩包密码存档
                cacheZip?.Remove();
                Log.Error("解压出错:{Ex}", ex);
                return false;
            }
        }
        
    }
}
