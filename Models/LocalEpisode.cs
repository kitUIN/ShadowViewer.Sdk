using SqlSugar;

namespace ShadowViewer.Models
{
    /// <summary>
    /// 本地漫画-话
    /// </summary>
    public class LocalEpisode: IDataBaseItem
    {
        public LocalEpisode() { }
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable = false)]
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Name { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 所属的漫画
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)")]
        public string ComicId { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCounts { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        public static ISugarQueryable<LocalEpisode> Query()
        {
            return DBHelper.Db.Queryable<LocalEpisode>();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新[{C}]Episode:{Episode}",ComicId, Id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Add()
        {
            if (!Query().Any(x => x.Id == Id))
            {
                DBHelper.Add(this);
                Logger.Information("添加[{C}]Episode:{Episode}", ComicId, Id);
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
        public static void Remove(string id)
        {
            DBHelper.Remove(new LocalEpisode { Id = id });
            Logger.Information("删除Episode:{Episode}", id);
        }
        public static LocalEpisode Create(string name, int order, string comicId, int counts, long size)
        {
            string id = Guid.NewGuid().ToString("N");
            while (DBHelper.Db.Queryable<LocalEpisode>().Any(x => x.Id == id))
            {
                id = Guid.NewGuid().ToString("N");
            }
            DateTime time = DateTime.Now;
            return new LocalEpisode()
            {
                Id = id,
                Name = name,
                Order = order,
                ComicId = comicId,
                PageCounts = counts,
                Size = size,
                CreateTime = time,
            };
        }
        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<LocalEpisode>();
    }
}
