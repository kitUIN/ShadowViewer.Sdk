using SqlSugar;

namespace ShadowViewer.Cache
{
    /// <summary>
    /// 缓存的临时缩略图
    /// </summary>
    public class CacheImg: IDataBaseItem
    {
        public CacheImg() { }
        [SugarColumn(ColumnDataType = "Nchar(32)", IsPrimaryKey = true, IsNullable = false)]
        public string Id { get; set; }
        [SugarColumn(ColumnDataType = "Ntext")]
        public string Path { get; set; }
        private ObservableCollection<string> comicIds = new ObservableCollection<string>();
        /// <summary>
        /// 标签
        /// </summary>
        [SugarColumn(IsJson = true, ColumnDataType = "Ntext")]
        public ObservableCollection<string> ComicId
        {
            get => comicIds;
            set
            {
                comicIds = value;
                if (comicIds != null)
                {
                    comicIds.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
                    {
                        Update();
                    };
                }
            }
        }
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
        public static void Remove(string id)
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
            if (CacheImg.Query().First(x => x.Id == md5) is CacheImg cache)
            {
                cache.ComicId.Add(comicId);
            }
            else
            {
                CacheImg img = new CacheImg
                {
                    Id = md5,
                    Path = path,
                    ComicId = new ObservableCollection<string> { comicId, },
                };
                img.Add();
                System.IO.File.WriteAllBytes(path, bytes);
            }
        }

        [SugarColumn(IsIgnore = true)]
        public static ILogger Logger { get; } = Log.ForContext<CacheImg>();
    }
}
