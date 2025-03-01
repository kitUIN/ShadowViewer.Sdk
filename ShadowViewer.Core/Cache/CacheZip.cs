using Serilog;
using SqlSugar;

namespace ShadowViewer.Core.Cache
{
    /// <summary>
    /// 缓存的解压密码
    /// </summary>
    public class CacheZip
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }
        public string? Md5 { get; set; }
        [SugarColumn(ColumnDataType = "Nvarchar(1024)")]
        public string? Sha1 { get; set; }
        [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsNullable = true)]
        public string? Password { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(2048)", IsNullable = true)]
        public string? Name { get; set; }
        [SugarColumn(ColumnDataType = "TEXT", IsNullable = true)]
        public string? CachePath { get; set; }
        [SugarColumn(IsNullable = true)]
        public long? ComicId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="sha1"></param>
        /// <param name="password"></param>
        /// <param name="cachePath"></param>
        /// <returns></returns>
        public static CacheZip Create(string md5, string sha1, 
            string? password = null, string? cachePath = null)
        {
            
            return new CacheZip()
            {
                Md5 = md5,
                Sha1 = sha1,
                Id = SnowFlakeSingle.Instance.NextId(),
                Password = password,
                CachePath = cachePath,
            };
        }

        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<CacheZip>();
    }
}
