namespace ShadowViewer.Interfaces
{
    public interface IPluginsToolKit
    {
        public Task InitAsync();
        /// <summary>
        /// 启用插件
        /// </summary>
        public void PluginEnabled(string id);
        /// <summary>
        /// 禁用插件
        /// </summary>
        public void PluginDisabled(string id);
        /// <summary>
        /// 获取插件(无论是否启动)
        /// </summary>
        public IPlugin GetPlugin(string id);
        /// <summary>
        /// 获取已启动的插件
        /// </summary>
        public IPlugin GetEnabledPlugin(string id);
        /// <summary>
        /// 获取已启动的所有插件
        /// </summary>
        public IEnumerable<IPlugin> EnabledPlugins { get; }
        /// <summary>
        /// 获取所有插件
        /// </summary>
        public ObservableCollection<IPlugin> Plugins { get; }
        /// <summary>
        /// 获取归属标签
        /// </summary>
        public LocalTag GetAffiliationTag(string id);
    }
}
