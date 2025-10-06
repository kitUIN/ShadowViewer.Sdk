using DryIoc;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Checkers;
using ShadowPluginLoader.WinUI.Installer;
using ShadowPluginLoader.WinUI.Scanners;
using SqlSugar;
using System.IO;
using Windows.Storage;
using ShadowViewer.Sdk.Plugins;

namespace ShadowViewer.Sdk.Helpers;

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
        var defaultPath = ApplicationData.Current.LocalFolder.Path;
        DiFactory.Services.RegisterInstance<ISqlSugarClient>(new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.Sqlite,
                ConnectionString = $"DataSource={Path.Combine(defaultPath, "ShadowViewer.sqlite")}",
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    IsNoReadXmlDescription = true,
                    SqliteCodeFirstEnableDefaultValue = true,
                    SqliteCodeFirstEnableDescription = true,
                }
            },
            db =>
            {
                //单例参数配置，所有上下文生效
                db.Aop.OnLogExecuting = (sql, pars) => { Log.ForContext<ISqlSugarClient>().Debug("{Sql}", sql); };
            }));
        DiFactory.Init<AShadowViewerPlugin, PluginMetaData>();
        DiFactory.Services.Register<PluginLoader>(
            reuse: Reuse.Singleton,
            made: Parameters.Of
                .Type<IDependencyChecker<PluginMetaData>>(serviceKey: "base")
                .OverrideWith(Parameters.Of.Type<IRemoveChecker>(serviceKey: "base"))
                .OverrideWith(
                    Parameters.Of.Type<IPluginScanner<AShadowViewerPlugin, PluginMetaData>>(serviceKey: "base"))
                .OverrideWith(Parameters.Of.Type<IPluginInstaller<PluginMetaData>>(serviceKey: "base"))
        );
    }
}