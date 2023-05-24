using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.IO;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Media.Imaging;

namespace ShadowViewer.Helpers
{
    public class CompressHelper
    {
        public static ILogger Logger { get; } = Log.ForContext<CompressHelper>();

        public static async Task<Tuple<ShadowEntry, CacheZip>> DeCompressAsync(string zip, string destinationDirectory, IProgress<MemoryStream> imgAction, IProgress<double> progress, ReaderOptions readerOptions = null)
        {
            
            destinationDirectory.CreateDirectory();
            Logger.Information("开始解压:{Zip}", zip);
            DateTime start = DateTime.Now;
            ShadowEntry root = new ShadowEntry()
            {
                Name = zip.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last(),
            };
            using (var fStream = File.OpenRead(zip))
            using (NonDisposingStream stream = NonDisposingStream.Create(fStream))
            {
                /*string md5 = EncryptingHelper.CreateMd5(stream);
                string sha1 = EncryptingHelper.CreateSha1(stream);
                CacheZip[] items = DBHelper.Db.Queryable<CacheZip>().Where(x => x.Sha1 == sha1 && x.Md5 == md5 && x.Size == stream.Length).ToArray();
                if(items.Length > 0 && Directory.Exists(items[0].CachePath))
                {
                    Logger.Information("{Zip}文件存在缓存记录,直接载入漫画{cid}", zip, items[0].ComicId);
                    return Tuple.Create<ShadowEntry, CacheZip>(null, items[0]);
                } */
                using (IArchive archive = ArchiveFactory.Open(stream, readerOptions))
                {
                    IOrderedEnumerable<IArchiveEntry> total = archive.Entries.Where(entry => !entry.IsDirectory).OrderBy(x => x.Key);
                    int totalCount = total.Count();
                    IArchiveEntry img = total.FirstOrDefault(x => x.Key.IsPic());
                    MemoryStream ms = new MemoryStream();
                    
                    using (Stream entryStream = img.OpenEntryStream())
                    {
                        await entryStream.CopyToAsync(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        
                    }
                    imgAction.Report(ms);
                    int i = 0;
                    foreach (IArchiveEntry entry in total)
                    {
                        entry.WriteToDirectory(destinationDirectory, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                        i++;
                        double result = (double)i / (double)totalCount;
                        progress.Report(Math.Round(result * 100, 2));
                        ShadowEntry.LoadEntry(entry, root);
                    }
                    root.LoadChildren();
                }
            }
            DateTime stop = DateTime.Now;
            Logger.Information("解压:{Zip}耗时: {Time} s", zip,(stop - start).TotalSeconds);
            return Tuple.Create<ShadowEntry, CacheZip>(root, null);
        }
    }
}
