using ShadowViewer.Services.Interfaces;
using SqlSugar;

namespace ShadowViewer
{
    public static class DiFactory
    {
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
            Services.Register<IPluginService, PluginService>(Reuse.Singleton);
            Services.Register<ICallableService, CallableService>(Reuse.Singleton);
            Services.Register<CompressService>(Reuse.Singleton);
            Services.Register<ResponderService>(Reuse.Singleton);
            Services.Register<BookShelfViewModel>(Reuse.Transient);
        }

    }
}
