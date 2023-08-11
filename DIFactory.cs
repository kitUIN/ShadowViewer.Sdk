using SqlSugar;

namespace ShadowViewer
{
    public static class DiFactory
    {
        public static Container Services { get; }
        static DiFactory()
        {
            var defaultPath = ConfigHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : System.Environment.CurrentDirectory;
            Services = new Container();
            var sqlSugar = new SqlSugarScope(new ConnectionConfig()
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
                            Log.Debug("{Sql}", sql);
                        };
                    });
            Services.RegisterInstance<ISqlSugarClient>(sqlSugar);
            Services.RegisterPlaceholder<IPlugin>();
            Services.Register<IPluginsToolKit, PluginsToolKit>(Reuse.Singleton);
            Services.Register<ICallableToolKit, CallableToolKit>(Reuse.Singleton);
            Services.Register<CompressToolKit>(Reuse.Singleton);
            Services.Register<SettingsViewModel>(Reuse.Singleton);
            Services.Register<NavigationViewModel>(Reuse.Singleton);
            Services.Register<BookShelfViewModel>(Reuse.Transient);
            Services.Register<AttributesViewModel>(Reuse.Transient);
        }

    }
}
