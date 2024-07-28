using ShadowPluginLoader.WinUI;
using SqlSugar;

namespace ShadowViewer
{
    /// <summary>
    /// 依赖注入-工厂
    /// </summary>
    public static class DiFactory
    {

        /// <summary>
        /// 依赖注入-容器
        /// </summary>
        public static Container Services { get; }
        static DiFactory()
        {
            var defaultPath = ConfigHelper.IsPackaged ? ApplicationData.Current.LocalFolder.Path : System.Environment.CurrentDirectory;
            Services = new Container(rules => rules.With(FactoryMethod.ConstructorWithResolvableArguments));
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
                            Log.ForContext<ISqlSugarClient>().Debug("{Sql}", sql);
                        };
                    });
            Services.RegisterInstance<ISqlSugarClient>(sqlSugar);
            //Services.RegisterPlaceholder<IPlugin>();
            Services.Register(
                Made.Of(() => Serilog.Log.ForContext(Arg.Index<Type>(0)), r => r.Parent.ImplementationType),
                setup: Setup.With(condition: r => r.Parent.ImplementationType != null));

            AbstractPluginLoader<PluginMetaData, AShadowViewerPlugin>.Services = Services;
            Services.Register<PluginLoader>(reuse: Reuse.Singleton);
            Services.Register<ICallableService, CallableService>(Reuse.Singleton);
            Services.Register<CompressService>(Reuse.Singleton);
            Services.Register<ResponderService>(Reuse.Singleton);
        }

    }
}
