using DryIoc;
using SqlSugar;

namespace ShadowViewer.Cache
{
    /// <summary>
    /// 缓存的临时缩略图
    /// </summary>
    public class CacheImg
    {
        public CacheImg() { }

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(ColumnDataType = "Nchar(32)",  IsNullable = false)]
        public string Md5 { get; set; }
        [SugarColumn(ColumnDataType = "Ntext")]
        public string Path { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)")]
        public string ComicId { get; set; }

        public static void CreateImage(string dir,byte[] bytes,string comicId)
        {
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            var md5 = EncryptingHelper.CreateMd5(bytes);
            var path = System.IO.Path.Combine(dir, md5 + ".png");
            if (db.Queryable<CacheImg>().First(x => x.Md5 == md5) is CacheImg cache)
            {
                db.Insertable(new CacheImg
                {
                    Md5 = md5,
                    ComicId = cache.ComicId,
                    Path = cache.Path,
                }).ExecuteReturnIdentity();
            }
            else
            {
                db.Insertable(new CacheImg
                {
                    Md5 = md5,
                    Path = path,
                    ComicId = comicId,
                }).ExecuteReturnIdentity();
                System.IO.File.WriteAllBytes(path, bytes);
            }
        }

        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<CacheImg>();
    }
}
