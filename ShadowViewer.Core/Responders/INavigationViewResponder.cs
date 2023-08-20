namespace ShadowViewer.Responders;

public interface INavigationViewResponder
{
    /// <summary>
    /// 插件ID
    /// </summary>
    string Id { get; set; }
    /// <summary>
    /// 添加到导航栏
    /// </summary>
    IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; }

    /// <summary>
    /// 添加到导航栏尾部
    /// </summary>
    IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; }
    
    /// <summary>
    /// 导航点击事件注入
    /// </summary>
    void NavigationViewItemInvokedHandler(IShadowNavigationItem item, ref Type page, ref object parameter);
    
}