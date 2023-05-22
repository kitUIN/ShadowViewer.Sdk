using SqlSugar;

namespace ShadowViewer.Models
{
    public class LocalPicture
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
        public long Size { get; set; }
        public DateTime CreateTime { get; set; }
        public void Update()
        {
            DBHelper.Update(this);
            Logger.Information("更新Picture:{Picture}", Id);
        }
        public void Add()
        {
            DBHelper.Add(this);
            Logger.Information("添加Picture:{Picture}", Id);
        }
        public void Remove()
        {
            DBHelper.Remove(new LocalPicture { Id = this.Id });
            Logger.Information("删除Picture:{Picture}", Id);
        }
        public static void Remove(LocalPicture localPicture)
        {
            localPicture.Remove();
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

        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<LocalPicture>();
    }
}
