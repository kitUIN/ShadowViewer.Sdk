namespace ShadowViewer.ToolKits
{
    public class PluginsToolKit: IPluginsToolKit
    {
        private ILogger Logger { get; } = Log.ForContext<PluginsToolKit>();
        
        /// <summary>
        /// 插件ID列表
        /// </summary>
        private ObservableCollection<string> AllPlugins { get; } = new ObservableCollection<string>();
        /// <summary>
        /// 所有插件
        /// </summary>
        private ObservableCollection<IPlugin> Plugins { get; }
        public PluginsToolKit(IEnumerable<IPlugin> plugins)
        {
            this.Plugins = new ObservableCollection<IPlugin>(plugins);
            foreach (IPlugin plugin in plugins)
            { 
                AllPlugins.Add(plugin.MetaData.Id);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginEnabled(string id)
        {
            if(Plugins.FirstOrDefault(x => x.MetaData.Id == id) is IPlugin plugin)
            {
                if (!plugin.IsEnabled)
                {
                    plugin.Enabled();
                    Logger.Information("插件{id}启动成功", id);
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginDisabled(string id)
        {
            if (Plugins.FirstOrDefault(x => x.MetaData.Id == id) is IPlugin plugin)
            {
                if (!plugin.IsEnabled)
                {
                    plugin.Disabled();
                    Logger.Information("插件{id}禁用成功", id);
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPlugin GetPlugin(string id)
        {
            return Plugins.FirstOrDefault(x => x.MetaData.Id == id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<IPlugin> GetEnabledPlugins()
        {
            return Plugins.Where(x => x.IsEnabled);
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
            return Plugins.FirstOrDefault(x => x.MetaData.Id == id).AffiliationTag;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ObservableCollection<IPlugin> GetPlugins()
        {
            return Plugins;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPlugin GetEnabledPlugin(string id)
        {
            return Plugins.FirstOrDefault(x => x.MetaData.Id == id && x.IsEnabled);
        }
    }
}
