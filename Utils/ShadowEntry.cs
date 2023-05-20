using Microsoft.UI.Xaml.Shapes;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System.IO;

namespace ShadowViewer.Utils
{
    public class ShadowEntry
    {
        public string Name { get; set; }
        public MemoryStream Source { get => stream; set => stream = value; }
        private MemoryStream stream;
        public int Depth { get; set; } = 0;
        public int Counts { get; set; } = 0;
        public long Size { get; set; } = 0;
        public bool IsDirectory { get => Children.Count > 0; }
        public List<ShadowEntry> Children { get; } = new List<ShadowEntry>();
        public ShadowEntry(){ }
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
        public static async Task LoadEntry(IArchiveEntry entry, ShadowEntry root)
        { 
            string[] names = entry.Key.Split("/");
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
                            });
                        }
                        else if (FileHelper.IsPic(names[i]))
                        {
                            MemoryStream ms = new MemoryStream();
                            using (Stream entryStream = entry.OpenEntryStream())
                            {
                                await entryStream.CopyToAsync(ms);
                                ms.Seek(0, SeekOrigin.Begin);
                            }
                            temp.Children.Add(new ShadowEntry()
                            {
                                Name = names[i],
                                Source = ms,
                                Size = entry.Size,
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
        public void LoadChildren()
        {
            if (!IsDirectory)
            {
                foreach (ShadowEntry child in Children)
                {
                    child.LoadChildren();
                } 
            }
            else
            {
                Size = Children.Sum(x => x.Size);
                Depth = Children.Max(x => x.Depth) + 1;
                Counts = Children.Sum(x => x.Counts);
            }
        }
        public static ShadowEntry GetTwo(ShadowEntry root)
        {
            if (root.Depth < 2) return null;
            if (root.Depth == 2) return root;
            foreach (ShadowEntry child in root.Children)
            { 
                if (GetTwo(child) is ShadowEntry result)
                {
                    return result;
                }
            } 
            return null;
        }
    }
}
