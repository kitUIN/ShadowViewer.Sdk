
using SqlSugar;

namespace ShadowViewer.Models
{
    /// <summary>
    /// 本地漫画-话
    /// </summary>
    public class LocalEpisode
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
        
        public static LocalEpisode Create(string name, int order, string comicId, int counts, long size)
        {
            var db = DiFactory.Services.Resolve<ISqlSugarClient>();
            string id = Guid.NewGuid().ToString("N");
            while (db.Queryable<LocalEpisode>().Any(x => x.Id == id))
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
