using Microsoft.UI.Xaml.Shapes;
using Serilog.Core;
using SharpCompress.Common;
using SharpCompress.Compressors.Xz;
using SqlSugar;
using System.IO;
using System.Security.Cryptography;

namespace ShadowViewer.Cache
{
    public class CacheImage
    {
        public CacheImage() { }
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable = false)]
        public string Id { get; private set; }
        [SugarColumn(ColumnDataType = "Nvarchar(1024)")]
        public string Md5 { get; private set; }
        [SugarColumn(ColumnDataType = "Nvarchar(1024)")]
        public string Sha1 { get; private set; }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Path { get; set; }
        public static CacheImage Create(string md5,string sha1, string dir, MemoryStream stream, bool create)
        {
            string id = Guid.NewGuid().ToString("N");
            while (DBHelper.Db.Queryable<CacheImage>().Any(x => x.Id == id))
            {
                id = Guid.NewGuid().ToString("N");
            }
            string path = System.IO.Path.Combine(dir, id + ".png");
            if (create)
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    stream.WriteTo(fileStream);
                }
            }
            return new CacheImage
            {
                Md5 = md5,
                Sha1 = sha1,
                Path = path,
                Id = id,
            };
        }
        public static CacheImage Create(string md5, string sha1, string path)
        {
            string id = Guid.NewGuid().ToString("N");
            while (DBHelper.Db.Queryable<CacheImage>().Any(x => x.Id == id))
            {
                id = Guid.NewGuid().ToString("N");
            }
            return new CacheImage
            {
                Md5 = md5,
                Sha1 = sha1,
                Path = path,
                Id = id,
            };
        }
        public static CacheImage Create(string dir, MemoryStream stream)
        {
            string md5 = CreateMd5(stream);
            string sha1 = CreateSha1(stream);
            List<CacheImage> res = DBHelper.Db.Queryable<CacheImage>().Where(x => x.Sha1 == sha1 && x.Md5 == md5).ToList();
            if(res.Count == 0)
            { 
                CacheImage img = Create(md5, sha1, dir, stream, true);
                img.Add();
                return img;
            }
            else
            {
                Logger.Information("获取CacheImage:{Id}", res[0].Id);
                return res[0];
            }
            
        }
        public static string CreateSha1(string path)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    byte[] hash = sha1.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static string CreateMd5(string path)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static string CreateSha1(Stream stream)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
        public static string CreateMd5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新CacheImage:{Id}", Id);
        }
        public void Add()
        {
            DBHelper.Add(this);
            Logger.Information("添加CacheImage:{Id}", Id);
        }
        public void Remove()
        {
            DBHelper.Remove(new CacheImage { Id = this.Id });
            Logger.Information("删除CacheImage:{Id}", Id);
        }
        public static void Remove(CacheImage img)
        {
            img.Remove();
        }
        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<CacheImage>();
    }
}
