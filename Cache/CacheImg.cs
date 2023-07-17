using SqlSugar;

namespace ShadowViewer.Cache
{
    /// <summary>
    /// 缓存的临时缩略图
    /// </summary>
    public class CacheImg: IDataBaseItem
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新CacheImg:{Id}", Id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Add()
        {
            if (!Query().Any(x => x.Id == Id))
            {
                DBHelper.Add(this);
                Logger.Information("添加CacheImg:{Id}", Id);
            }
            else
            {
                Update();
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Remove()
        {
            Remove(Id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public static void Remove(int id)
        {
            DBHelper.Remove(new CacheImg { Id = id });
            Logger.Information("删除CacheImg:{Id}", id);
        }
        public static ISugarQueryable<CacheImg> Query()
        {
            return DBHelper.Db.Queryable<CacheImg>();
        }

        public static void CreateImage(string dir,byte[] bytes,string comicId)
        {
            string md5 = EncryptingHelper.CreateMd5(bytes);
            string path = System.IO.Path.Combine(dir, md5 + ".png");
            if (CacheImg.Query().First(x => x.Md5 == md5) is CacheImg cache)
            {
                DBHelper.Add(new CacheImg
                {
                    ComicId = cache.ComicId,
                    Path = cache.Path,
                });
            }
            else
            {
                DBHelper.Add(new CacheImg
                {
                    Md5 = md5,
                    Path = path,
                    ComicId = comicId,
                });

                System.IO.File.WriteAllBytes(path, bytes);
            }
        }

        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<CacheImg>();
    }
}
