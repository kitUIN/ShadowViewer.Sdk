namespace ShadowViewer.Plugins
{
    public interface IPlugin
    {

        /// <summary>
        /// 元数据(包含相关数据)
        /// </summary>
        /// <returns></returns>
        PluginMetaData MetaData { get; }
        /// <summary>
        /// 插件所属标签注入
        /// </summary>
        LocalTag AffiliationTag { get; }
        /// <summary>
        /// 插件设置界面
        /// </summary>
        Type SettingsPage { get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnabled { get; }
        /// <summary>
        /// 启用插件
        /// </summary>
        void Enabled();
        /// <summary>
        /// 禁用插件
        /// </summary>
        void Disabled();
        /// <summary>
        /// 构造函数结束后响应
        /// </summary>
        void Started();
        
        /// <summary>
        /// 导航插件栏注入
        /// </summary>
        /// <param name="nav">插件栏</param>
        void NavigationViewItemsHandler(NavigationViewItem navItem);
        /// <summary>
        /// 导航点击事件注入
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        void NavigationViewItemInvokedHandler(string tag,out Type _page,out object parameter);
        
    }
}
