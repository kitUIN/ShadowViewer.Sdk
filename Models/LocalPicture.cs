using SqlSugar;

namespace ShadowViewer.Models
{
    /// <summary>
    /// 本地漫画-页
    /// </summary>
    public class LocalPicture: IDataBaseItem
    {
        public LocalPicture() { }
        // <summary>
        /// ID
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable = false)]
        public string Id { get; set; }
        /// <summary>
        /// 所属的漫画
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)")]
        public string ComicId { get; set; }
        /// <summary>
        /// 所属的漫画-话
        /// </summary>
        [SugarColumn(ColumnDataType = "Nchar(32)")]
        public string EpisodeId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Name { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [SugarColumn(ColumnDataType = "Nvarchar(2048)")]
        public string Img { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新[{C}][{E}]Picture:{P}", ComicId, EpisodeId, Id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Add()
        {
            if (!Query().Any(x => x.Id == Id))
            {
                DBHelper.Add(this);
                Logger.Information("添加[{C}][{E}]Picture:{P}", ComicId, EpisodeId, Id);
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
            Logger.Information("删除Picture:{P}", id);
        }
        public static LocalPicture Create(string name, string episodeId, string comicId, string img, long size)
        {
            string id = Guid.NewGuid().ToString("N");
            while (DBHelper.Db.Queryable<LocalPicture>().Any(x => x.Id == id))
            {
                id = Guid.NewGuid().ToString("N");
            }
            DateTime time = DateTime.Now;
            return new LocalPicture()
            {
                Id = id,
                Name = name,
                EpisodeId = episodeId,
                ComicId = comicId,
                Img = img,
                Size = size,
                CreateTime = time,
            };
        }

        public static ISugarQueryable<LocalPicture> Query()
        {
            return DBHelper.Db.Queryable<LocalPicture>();
        }
        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<LocalPicture>();
    }
}
