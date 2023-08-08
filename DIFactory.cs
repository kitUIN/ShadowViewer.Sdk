using SqlSugar;

namespace ShadowViewer
{
    public class DiFactory
    {
        public static DiFactory Current{ get; set; }
        public IServiceProvider Services { get; } = ConfigureServices();

        private static IServiceProvider ConfigureServices()
        {
            var defaultPath = ConfigHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : System.Environment.CurrentDirectory;
            var services = new ServiceCollection();
            services.AddSingleton<ISqlSugarClient>(s =>
            {
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
                            Log.Debug("{Sql}",sql);
                        };
                    });
                return sqlSugar;
            });
            #region ToolKit
            services.AddSingleton<IPluginsToolKit, PluginsToolKit>();
            services.AddSingleton<ICallableToolKit, CallableToolKit>();
            services.AddSingleton<CompressToolKit>();
            #endregion
            #region ViewModel
            services.AddSingleton<SettingsViewModel>();
            services.AddScoped<BookShelfViewModel>();
            services.AddSingleton<NavigationViewModel>();
            services.AddScoped<AttributesViewModel>();
            #endregion
            return services.BuildServiceProvider();
        }
    }
}
