using SqlSugar;

namespace ShadowViewer.Cache
{
    /// <summary>
    /// 缓存的解压密码
    /// </summary>
    public class CacheZip: IDataBaseItem
    {
        public CacheZip() { }
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable = false)]
        public string Id { get; private set; }
        public string Md5 { get; private set; }
        [SugarColumn(ColumnDataType = "Nvarchar(1024)")]
        public string Sha1 { get; private set; }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsNullable = true)]
        public string Password { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsNullable = true)]
        public string Name { get; set; }
        [SugarColumn(ColumnDataType = "Ntext", IsNullable = true)]
        public string CachePath { get; set; }
        [SugarColumn(ColumnDataType = "Nchar(32)", IsNullable = true)]
        public string ComicId { get;  set; }
        public static CacheZip Create(string md5, string sha1,string password=null,string cachePath= null)
        {
            string id = Guid.NewGuid().ToString("N");
            while (DBHelper.Db.Queryable<CacheZip>().Any(x => x.Id == id))
            {
                id = Guid.NewGuid().ToString("N");
            }
            return new CacheZip()
            {
                Md5 = md5,
                Sha1 = sha1,
                Id = id,
                Password = password,
                CachePath = cachePath,
            };
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新CacheZip:{Id}", Id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Add()
        {
            if (!Query().Any(x => x.Id == Id))
            {
                DBHelper.Add(this);
                Logger.Information("添加CacheZip:{Id}", Id);
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
        public static void Remove(string id)
        {
            DBHelper.Remove(new CacheZip { Id = id });
            Logger.Information("删除CacheZip:{Id}", id);
        }
        public static ISugarQueryable<CacheZip> Query()
        {
            return DBHelper.Db.Queryable<CacheZip>();
        }
        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<CacheZip>();
    }
}
