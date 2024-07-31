using Windows.ApplicationModel;
using ShadowViewer.Responders;
using SqlSugar;
using ShadowPluginLoader.WinUI.Exceptions;

namespace ShadowViewer;

/// <summary>
/// ShadowView 插件加载器
/// </summary>
public class PluginLoader(ILogger logger) : AbstractPluginLoader<PluginMetaData, AShadowViewerPlugin>(logger)
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
    protected override void LoadPluginDi(Type tPlugin, AShadowViewerPlugin aPlugin, PluginMetaData meta)
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
            else if (type.IsAssignableTo(typeof(ISettingFolder)))
            {
                DiFactory.Services.Register(typeof(ISettingFolder),
                    type,
                    Reuse.Singleton,
                    made: Parameters.Of.Type<string>(_ => meta.Id));
                Log.Information(
                    "{Id}{Name} Load {INavigationResponder}:{TNavigationResponder}",
                    meta.Id, meta.Name,
                    typeof(ISettingFolder), type.Name);
            }
        if (navigationViewResponder is not null)
        {
            DiFactory.Services.Register(typeof(INavigationResponder), navigationViewResponder,
                Reuse.Singleton, made: Parameters.Of.Type<string>(_ => meta.Id));
            Log.Information(
                "{Id}{Name} Load INavigationResponder:{TNavigationResponder}",
                meta.Id,meta.Name,
                navigationViewResponder.Name);
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

    /// <inheritdoc />
    protected override string PluginFolder => Config.PluginsPath;

    /// <inheritdoc cref="AbstractPluginLoader{TMeta,TAPlugin}.ImportFromZipAsync"/>
    public new async Task ImportFromZipAsync(string zipPath)
    {
        try
        {
            CheckPluginInZip(zipPath);
            var destinationDirectory = Path.Combine(PluginFolder, Path.GetFileNameWithoutExtension(zipPath));
            await CompressService.DeCompressAsync(zipPath, destinationDirectory);
            await this.ImportFromDirAsync(destinationDirectory);
        }
        catch (PluginImportException ex)
        {
            this.Logger?.Warning("{Pre}{Message}", LoggerPrefix, ex.Message);
        }
    }
}
