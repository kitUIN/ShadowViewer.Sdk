using SqlSugar;

namespace ShadowViewer.Cache
{
    public class CacheZip
    {
        public CacheZip() { }
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable = false)]
        public string Id { get; private set; }
        public string Md5 { get; private set; }
        [SugarColumn(ColumnDataType = "Nvarchar(1024)")]
        public string Sha1 { get; private set; }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Password { get; set; }
        [SugarColumn(ColumnDataType = "Ntext")]
        public string CachePath { get; set; }
        [SugarColumn(ColumnDataType = "Nchar(32)")]
        public string ComicId { get; private set; }
        public long Size { get;private set; }
        public static CacheZip Create(string md5, string sha1)
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
            };
        }
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新CacheZip:{Id}", Id);
        }
        public void Add()
        {
            DBHelper.Add(this);
            Logger.Information("添加CacheZip:{Id}", Id);
        }
        public void Remove()
        {
            DBHelper.Remove(new CacheZip { Id = this.Id });
            Logger.Information("删除CacheZip:{Id}", Id);
        }
        public static void Remove(CacheZip zip)
        {
            zip.Remove();
        }
        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<CacheZip>();
    }
}
