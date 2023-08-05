using CustomExtensions.WinUI;
using ShadowViewer.Plugins;
using System.Diagnostics;

namespace ShadowViewer.ToolKits
{
    public class PluginsToolKit: IPluginsToolKit
    {
        private ILogger Logger { get; } = Log.ForContext<PluginsToolKit>();
        private ICallableToolKit Caller { get; }
        /// <summary>
        /// 插件ID列表
        /// </summary>
        private ObservableCollection<string> AllPlugins { get; } = new ObservableCollection<string>();
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
        public async Task InitAsync()
        {
            var asm = await ApplicationExtensionHost.Current.LoadExtensionAsync(@"D:\VsProjects\WASDK\ShadowViewer.Plugin.Bika\bin\Debug\net6.0-windows10.0.19041.0\ShadowViewer.Plugin.Bika.dll");

            foreach (var instance in asm.ForeignAssembly.GetExportedTypes()
                .Where(type => type.IsAssignableTo(typeof(IPlugin)))
                .Select(type => Activator.CreateInstance(type) as IPlugin))
            {
                Instances.Add(instance);
                AllPlugins.Add(instance.MetaData.Id);
                Log.Information("[插件控制器]加载{name}插件成功", instance.MetaData.Name);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginEnabled(string id)
        {
            if(Instances.FirstOrDefault(x => x.MetaData.Id == id) is IPlugin plugin)
            {
                if (!plugin.IsEnabled)
                {
                    plugin.Enabled();
                    Caller.PluginEnabled(this, id, true);
                    Logger.Information("插件{id}启动成功", id);
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginDisabled(string id)
        {
            if (Instances.FirstOrDefault(x => x.MetaData.Id == id) is IPlugin plugin)
            {
                if (!plugin.IsEnabled)
                {
                    plugin.Disabled();
                    Caller.PluginDisabled(this, id,false);
                    Logger.Information("插件{id}禁用成功", id);
                }
            }
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
        public IEnumerable<IPlugin> EnabledPlugins
        {
            get => Instances.Where(x => x.IsEnabled);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public LocalTag GetAffiliationTag(string id)
        {
            if(id == "Local")
            {
                return new LocalTag(CoreResourcesHelper.GetString(CoreResourceKey.LocalTag), "#000000", "#ffd657");
            }
            return Instances.FirstOrDefault(x => x.MetaData.Id == id).AffiliationTag;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<IPlugin> Plugins
        {
            get => Instances;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPlugin GetEnabledPlugin(string id)
        {
            return Instances.FirstOrDefault(x => x.MetaData.Id == id && x.IsEnabled);
        }
    }
}
