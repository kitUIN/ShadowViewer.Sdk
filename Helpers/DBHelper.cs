using NetTaste;
using SqlSugar;

namespace ShadowViewer.Helpers
{
    public static class DBHelper
    {
        public static string DBPath { get; } = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "ShadowViewer.db");
        
        public static SqlSugarClient GetClient()
        {
            return GetClient(DBPath);
        }
        public static SqlSugarClient GetClient(string dbpath)
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = $"DataSource={dbpath}",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true
            }, db => { db.Aop.OnLogExecuting = (sql, pars) => { Log.ForContext<SqlSugarClient>().Debug(sql); }; });
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="dbpath"></param>
        /// <param name="obj"></param>
        public static void Init(string dbpath, Type obj)
        {
            SqlSugarClient db = GetClient(dbpath);
            db.CodeFirst.InitTables(obj);
        }
        public static void Init(Type obj)
        {
            Init(DBPath, obj);
        }
        /// <summary>
        /// 在数据库添加一个新行
        /// </summary>
        public static void Add<T>(string dbpath, T obj) where T : class, new()
        {
            SqlSugarClient db = GetClient(dbpath);
            db.Insertable(obj).ExecuteCommand();
        }
        public static void Add<T>(T obj) where T : class, new()
        {
            Add(DBPath, obj);
        }
        /// <summary>
        /// 获取数据库中所有的行
        /// </summary>
        public static List<T> GetAll<T>(string dbpath)
        {
            SqlSugarClient db = GetClient(dbpath);
            return db.Queryable<T>().ToList();
        }
        public static List<T> GetAll<T>()
        {
            return GetAll<T>(DBPath);
        }
        /// <summary>
        /// 数据库中更新行
        /// </summary>
        public static void Update<T>(string dbpath, T obj) where T : class, new()
        {
            SqlSugarClient db = GetClient(dbpath);
            db.Updateable(obj).ExecuteCommand();
        }
        public static void Update<T>(T obj) where T : class, new()
        {
            Update<T>(DBPath,obj);
        }

        /// <summary>
        /// 数据库中删除行
        /// </summary>
        public static void Remove<T>(string dbpath, T obj)
        {
            SqlSugarClient db = GetClient(dbpath);
            db.DeleteableByObject(obj).ExecuteCommand();
        }
        public static void Remove<T>(T obj)
        {
            Remove<T>(DBPath, obj);
        }
    }
    
}
