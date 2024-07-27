
using Windows.ApplicationModel;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Responders;
using SqlSugar;

namespace ShadowViewer;

/// <summary>
/// ShadowView 插件加载器
/// </summary>
public class PluginLoader(ILogger logger) : AbstractPluginLoader<PluginMetaData, PluginBase>(logger)
{
    /// <summary>
    /// 加载器版本
    /// </summary>
    public static Version CoreVersion { get; } = new(
        Package.Current.Id.Version.Major,
        Package.Current.Id.Version.Minor,
        Package.Current.Id.Version.Build,
        Package.Current.Id.Version.Revision
    );
    /// <inheritdoc/>
    protected override void LoadPluginDi(Type tPlugin, PluginBase aPlugin, PluginMetaData meta)
    {
        Type? navigationViewResponder = null;
        Type? picViewResponder = null;
        Type? historyResponder = null;
        var db = DiFactory.Services.Resolve<ISqlSugarClient>();
        foreach (var type in tPlugin.Assembly.GetExportedTypes())
            if (type.IsAssignableTo(typeof(AbstractNavigationResponder))) navigationViewResponder = type;
            else if (type.IsAssignableTo(typeof(PicViewResponderBase))) picViewResponder = type;
            else if (type.IsAssignableTo(typeof(HistoryResponderBase))) historyResponder = type;
            else if (type.IsAssignableTo(typeof(IHistory)))
            {
                db.CodeFirst.InitTables(type);
            }

        if (navigationViewResponder is not null)
        {
            DiFactory.Services.Register(typeof(INavigationResponder), navigationViewResponder,
                Reuse.Singleton, made: Parameters.Of.Type<string>(_ => meta.Id));
            Log.Information("{Id}{Name} Load INavigationResponder:{TNavigationResponder}",meta.Id,meta.Name, navigationViewResponder.Name);
        }

        if (picViewResponder is not null)
            DiFactory.Services.Register(typeof(IPicViewResponder), picViewResponder,
                Reuse.Singleton, made: Parameters.Of.Type<string>(_ => meta.Id));
        if (historyResponder is not null)
        {
            DiFactory.Services.Register(typeof(IHistoryResponder), historyResponder,
                Reuse.Singleton, made: Parameters.Of.Type<string>(_ => meta.Id));
        }
    }
}
