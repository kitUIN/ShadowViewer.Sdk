using Microsoft.Extensions.DependencyInjection;
using CustomExtensions.WinUI;
using ShadowViewer.Interfaces;
using ShadowViewer.Plugins;
using ShadowViewer.ToolKits;
using ShadowViewer.ViewModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using SqlSugar;

namespace ShadowViewer
{
    public class DiFactory
    {
        public DiFactory()
        {
            Services = ConfigureServices();
        }
        public static DiFactory Current;
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            
            var services = new ServiceCollection();
            services.AddSingleton<ISqlSugarClient>(s =>
            {
                var sqlSugar = new SqlSugarScope(new ConnectionConfig()
                    {
                        DbType = SqlSugar.DbType.Sqlite,
                        ConnectionString = $"DataSource={Path.Combine(System.Environment.CurrentDirectory, "ShadowViewer.sqlite")}",
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
