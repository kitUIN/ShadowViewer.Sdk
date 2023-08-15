namespace ShadowViewer.Plugins;

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
    bool CanDelete { get; }

    /// <summary>
    /// 搜索输入框内文字变化
    /// </summary>
    IEnumerable<IShadowSearchItem>
        SearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args);
    /// <summary>
    /// 合并的资源
    /// </summary>
    IEnumerable<ResourceDictionary> ResourceDictionaries { get; }

    /// <summary>
    /// 搜索选择
    /// </summary>
    void SearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args);

    /// <summary>
    /// 搜索提交
    /// </summary>
    void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args);

    /// <summary>
    /// 搜索框聚焦
    /// </summary>
    void SearchGotFocus(object sender, RoutedEventArgs args);

    /// <summary>
    /// 搜索框失焦
    /// </summary>
    void SearchLostFocus(object sender, RoutedEventArgs args);
    /// <summary>
    /// 导航
    /// </summary>
    void Navigate(Uri uri,string[] urls);
}