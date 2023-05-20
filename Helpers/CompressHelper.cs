using Aspose.Zip;
using Aspose.Zip.SevenZip;
using SharpCompress.Common;
using SharpCompress.Compressors.Xz;
using SharpCompress.Readers;
using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Collections;
using Windows.Storage.Compression;
using Windows.Storage.Streams;

namespace ShadowViewer.Helpers
{
    public static class CompressHelper
    {

        public static void DeCompress(string zip, string destinationDirectory)
        {
            if (zip.EndsWith(".zip"))
            {
                ZipDeCompress(zip, destinationDirectory);
            } else if (zip.EndsWith(".7z"))
            {
                SevenZipDeCompress(zip, destinationDirectory);
            }

        }
        public static void SevenZipDeCompress(string zip, string destinationDirectory)
        {
            using (SevenZipArchive archive = new SevenZipArchive(zip))
            {
                archive.ExtractToDirectory(destinationDirectory);
            }
        }
        public static async Task LoadEntry(IReader reader, ShadowEntry root)
        {
            string[] names = reader.Entry.Key.Split("/");
            ShadowEntry temp = root;
            ShadowEntry tmp = null;
            for (int i = 0; i < names.Length; i++)
            {
                tmp = temp.Children.FirstOrDefault(x => x.Name == names[i]);
                if (tmp is null)
                {
                    if (i == names.Length - 1)
                    {
                        if (reader.Entry.IsDirectory)
                        {
                            temp.Children.Add(new ShadowEntry()
                            {
                                Name = names[i],
                            });
                        }
                        else if (FileHelper.IsPic(names[i]))
                        {
                            MemoryStream ms = new MemoryStream();
                            using (EntryStream entryStream = reader.OpenEntryStream())
                            { 
                                await entryStream.CopyToAsync(ms);
                                ms.Seek(0, SeekOrigin.Begin); 
                            }  
                            temp.Children.Add(new ShadowEntry()
                            {
                                Name = names[i],
                                Source = ms,
                                Size = reader.Entry.Size,
                            }); 
                        }

                    }
                    else
                    {
                        temp.Children.Add(new ShadowEntry()
                        {
                            Name = names[i],
                        });
                        
                    }
                }
                else
                {
                    temp = tmp;
                }

            }
        }
        public static void ZipDeCompress(string zip, string destinationDirectory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (FileStream zipFile = File.Open(zip, FileMode.Open))
            {
                using (var archive = new Archive(zipFile))
                {
                     archive.ExtractToDirectory(destinationDirectory);
                }
            }
        }
        public static async Task<ShadowEntry> ZipDeCompress(string zip)
        {
            using (Stream stream = File.OpenRead(zip))
            using (IReader reader = ReaderFactory.Open(stream))
            {
                ShadowEntry root = new ShadowEntry()
                {
                    Name = zip.Split("/").Last(),

                };
                while (reader.MoveToNextEntry())
                {
                    await LoadEntry(reader, root);
                     
                }
                return root;
            }
            
        }
    }
}
