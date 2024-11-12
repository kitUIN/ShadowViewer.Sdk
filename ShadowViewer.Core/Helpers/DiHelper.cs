using SqlSugar;

namespace ShadowViewer.Helpers;

/// <summary>
/// 依赖注入帮助类
/// </summary>
public static class DiHelper
{
    /// <summary>
    /// 初始化DI
    /// </summary>
    public static void Init()
    {
        var defaultPath = ConfigHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : System.Environment.CurrentDirectory;
        DiFactory.Services.RegisterInstance<ISqlSugarClient>(new SqlSugarScope(new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.Sqlite,
                ConnectionString = $"DataSource={Path.Combine(defaultPath, "ShadowViewer.sqlite")}",
                IsAutoCloseConnection = true,
            },
            db =>
            {
                //单例参数配置，所有上下文生效
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Log.ForContext<ISqlSugarClient>().Debug("{Sql}", sql);
                };
            }));
        DiFactory.Services.Register<INotifyService, NotifyService>(Reuse.Singleton);
        DiFactory.Services.Register<PluginLoader>(reuse: Reuse.Singleton);
        DiFactory.Services.Register<ICallableService, CallableService>(Reuse.Singleton);
        DiFactory.Services.Register<CompressService>(Reuse.Singleton);
        DiFactory.Services.Register<ResponderService>(Reuse.Singleton);
    }
}