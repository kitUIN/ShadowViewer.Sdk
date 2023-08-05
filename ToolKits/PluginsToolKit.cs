

using ShadowViewer.Extensions;

namespace ShadowViewer.ToolKits
{
    public class PluginsToolKit: IPluginsToolKit
    {
        private ILogger Logger { get; } = Log.ForContext<PluginsToolKit>();
        private ICallableToolKit Caller { get; }
        /// <summary>
        /// 所有插件
        /// </summary>
        private ObservableCollection<IPlugin> Instances { get; } = new ObservableCollection<IPlugin>();
        public PluginsToolKit(ICallableToolKit callableToolKit) 
        {
            Caller = callableToolKit;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task ImportAsync()
        {
            var path = ConfigHelper.GetString("PluginsPath");
            var dir = new DirectoryInfo(path);
            foreach (var item in dir.GetDirectories())
            {
                foreach (var file in item.GetFiles("ShadowViewer.Plugin.*.dll"))
                {
                    await ImportAsync(file.FullName);
                    break;
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task ImportAsync(string path)
        {
            var asm = await ApplicationExtensionHost.Current.LoadExtensionAsync(path);
            foreach (var instance in asm.ForeignAssembly.GetExportedTypes()
                         .Where(type => type.IsAssignableTo(typeof(IPlugin)))
                         .Select(type => Activator.CreateInstance(type) as IPlugin))
            {
                if(instance is null) continue;
                Instances.Add(instance);
                instance.Loaded();
                Log.Information("[插件控制器]加载{Name}插件成功", instance.MetaData.Name);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginEnabled(string id)
        {
            if (Instances.FirstOrDefault(x => x.MetaData.Id == id) is not { IsEnabled: true } plugin) return;
            plugin.IsEnabled = true;
            Logger.Information("插件{Id}启动成功", id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginDisabled(string id)
        {
            if (Instances.FirstOrDefault(x => x.MetaData.Id == id) is not { IsEnabled: false } plugin) return;
            plugin.IsEnabled = false;
            Logger.Information("插件{Id}禁用成功", id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPlugin GetPlugin(string id)
        {
            return Instances.FirstOrDefault(x => x.MetaData.Id == id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<IPlugin> EnabledPlugins => Instances.Where(x => x.IsEnabled);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public LocalTag GetAffiliationTag(string id)
        {
            return id == "Local" ? new LocalTag(CoreResourcesHelper.GetString(CoreResourceKey.LocalTag), "#000000", "#ffd657") : Instances.FirstOrDefault(x => x.MetaData.Id == id)?.AffiliationTag;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<IPlugin> Plugins => Instances;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPlugin GetEnabledPlugin(string id)
        {
            return Instances.FirstOrDefault(x => x.MetaData.Id == id && x.IsEnabled);
        }
    }
}
