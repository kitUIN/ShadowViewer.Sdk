namespace ShadowViewer.Interfaces
{
    public interface IPluginsToolKit
    {
        /// <summary>
        /// 插件是否开启
        /// </summary>
        public bool IsEnabled(string id);
        /// <summary>
        /// 启用插件
        /// </summary>
        public void PluginEnabled(string id);
        /// <summary>
        /// 禁用插件
        /// </summary>
        public void PluginDisabled(string id);
        /// <summary>
        /// 获取插件
        /// </summary>
        public IPlugin GetPlugin(string id);
        /// <summary>
        /// 获取已启动的插件
        /// </summary>
        public IEnumerable<IPlugin> GetEnabledPlugins();
    }
}
