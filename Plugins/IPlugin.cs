namespace ShadowViewer.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        /// 加载完成
        /// </summary>
        void Loaded(bool isEnabled);
        /// <summary>
        /// 元数据(包含相关数据)
        /// </summary>
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
        /// 添加到导航栏
        /// </summary>
        IEnumerable<ShadowNavigationItem> NavigationViewMenuItems { get; }
        /// <summary>
        /// 添加到导航栏尾部
        /// </summary>
        IEnumerable<ShadowNavigationItem> NavigationViewFooterItems { get; }
        /// <summary>
        /// 导航点击事件注入
        /// </summary>
        void NavigationViewItemInvokedHandler(object tag, ref Type page, ref object parameter);
        /// <summary>
        /// 可以开启/关闭
        /// </summary>
        bool CanSwitch { get; }
        /// <summary>
        /// 允许删除
        /// </summary>
        bool CanDelete { get;}
    }
}
