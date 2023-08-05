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
        bool IsEnabled { get; set; }

        /// <summary>
        /// 导航插件栏注入
        /// </summary>
        void NavigationViewItemsHandler(ref NavigationViewItem navItem);
        /// <summary>
        /// 导航点击事件注入
        /// </summary>
        void NavigationViewItemInvokedHandler(string tag,out Type page,out object parameter);
        
    }
}
