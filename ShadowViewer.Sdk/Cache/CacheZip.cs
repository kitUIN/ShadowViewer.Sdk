

namespace ShadowViewer.Sdk.Cache
{
    /// <summary>
    /// 缓存的解压密码
    /// </summary>
    public class CacheZip
    {
        /// <summary>
        /// MD5
        /// </summary>
        public string Md5 { get; set; } = null!;

        /// <summary>
        /// SHA1
        /// </summary>
        public string Sha1 { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 路径
        /// </summary>
        public string? CachePath { get; set; }

        /// <summary>
        /// ComicId
        /// </summary>
        public long? ComicId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static CacheZip Create(string md5, string sha1, string name, string? cachePath = null,
            string? password = null)
        {
            return new CacheZip()
            {
                Md5 = md5,
                Sha1 = sha1,
                Password = password,
                Name = name,
                CachePath = cachePath,
            };
        }
    }
}