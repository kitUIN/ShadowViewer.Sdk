using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace ShadowViewer.Helpers
{
    public class CompressHelper
    {
        public static ILogger Logger { get; } = Log.ForContext<CompressHelper>();
        public static void DeCompress(string zip, string destinationDirectory)
        { 
            destinationDirectory.CreateDirectory();
            Logger.Information("解压:{Zip}", zip);
            switch (Path.GetExtension(zip).ToLower())
            { 
                case ".7z":
                    SevenZipDeCompress(zip, destinationDirectory);
                    break;
                case ".rar":
                    RarDeCompress(zip, destinationDirectory);
                    break;
                default:
                    ZipDeCompress(zip, destinationDirectory);
                    break;
            }  
        }
        public static void RarDeCompress(string zip, string destinationDirectory)
        {
            using (var archive = RarArchive.Open(zip))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(destinationDirectory, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        } 
        public static void SevenZipDeCompress(string zip, string destinationDirectory)
        {
            var archive = ArchiveFactory.Open(zip);
            foreach (var entry in archive.Entries.Where(x=> !x.IsDirectory))
            {
                entry.WriteToDirectory(destinationDirectory, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
            }
        }
        
        public static void ZipDeCompress(string zip, string destinationDirectory)
        {
            
            using (Stream stream = File.OpenRead(zip))
            using (var reader = ReaderFactory.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                         reader.WriteEntryToDirectory(destinationDirectory, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                    }
                }
            }
        }
        public static async Task<ShadowEntry> DeCompress(string zip)
        { 
            switch (Path.GetExtension(zip).ToLower())
            {
                case ".7z":
                    return await SevenZipDeCompress(zip);
 
                default:
                    return await ZipDeCompress(zip);
            }
        }

        //Zip，GZip，BZip2，Tar，Rar，LZip和XZ。
        public static async Task<ShadowEntry> ZipDeCompress(string zip)
        {
            using (Stream stream = File.OpenRead(zip))
            using (IReader reader = ReaderFactory.Open(stream))
            {
                ShadowEntry root = new ShadowEntry()
                {
                    Name = zip.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last(),
                };
                while (reader.MoveToNextEntry())
                {
                    await ShadowEntry.LoadEntry(reader, root);
                }
                root.LoadChildren();
                return root;
            }
        }
         public static async Task<ShadowEntry> SevenZipDeCompress(string zip)
        {

            using (var fileStream = File.OpenRead(zip))
            using (var archive =  SevenZipArchive.Open(fileStream))
            {
                ShadowEntry root = new ShadowEntry()
                {
                    Name = zip.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last(),
                };
                foreach (SevenZipArchiveEntry entry in archive.Entries)
                {
                    await ShadowEntry.LoadEntry(entry, root);
                }
                root.LoadChildren();
                return root;
            }
        }
    }
}
