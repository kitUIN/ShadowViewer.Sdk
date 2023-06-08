using SqlSugar;

namespace ShadowViewer.Helpers
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public static class DBHelper
    {
        public static string DBPath { get; } = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "ShadowViewer.db");
        /// <summary>
        /// DB
        /// </summary>
        public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
        {
            ConnectionString = "DataSource="+ DBPath,
            DbType = DbType.Sqlite,
            IsAutoCloseConnection = true 
        }, db => {db.Aop.OnLogExecuting = (sql, pars) =>{ Log.ForContext<SqlSugarClient>().Debug(sql);};});
        /// <summary>
        /// 初始化数据库-表
        /// </summary>
        public static void InitTable(string table, Type obj)
        {
            Db.DbMaintenance.CreateDatabase();
            if (!Db.DbMaintenance.IsAnyTable(table, false))
            {
                Db.CodeFirst.InitTables(obj);
            }
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void Init()
        {
            InitTable(nameof(LocalComic)   ,typeof(LocalComic)     );
            InitTable(nameof(LocalEpisode) ,typeof(LocalEpisode)   );
            InitTable(nameof(LocalPicture) ,typeof(LocalPicture)   );
            InitTable(nameof(ShadowTag)    ,typeof(ShadowTag)      );
            InitTable(nameof(CacheImg)     ,typeof(CacheImg)       );
            InitTable(nameof(CacheZip)     ,typeof(CacheZip)       );
        }
        /// <summary>
        /// 在数据库添加一个新行
        /// </summary>
        public static void Add<T>(T obj) where T : class, new()
        {
            Db.Insertable(obj).ExecuteCommand();
        }
        /// <summary>
        /// 获取数据库中所有的行
        /// </summary>
        public static List<T> GetAll<T>()
        {
            return Db.Queryable<T>().ToList();
        }
        /// <summary>
        /// 数据库中更新行
        /// </summary>
        public static void Update<T>(T obj) where T : class, new()
        {
            Db.Updateable(obj).ExecuteCommand();
        }

        /// <summary>
        /// 数据库中删除行
        /// </summary>
        public static void Remove<T>(T obj)
        {
            Db.DeleteableByObject(obj).ExecuteCommand();
        }
    }
}
