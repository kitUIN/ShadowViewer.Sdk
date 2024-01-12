using Microsoft.UI.Windowing;
using ShadowViewer.Args;
using Windows.Foundation;

namespace ShadowViewer.Services
{
    /// <summary>
    /// 通知其他控件的工具类
    /// </summary>
    public partial interface ICallableService
    {
        /// <summary>
        /// 控制页面跳转事件
        /// </summary>
        public event EventHandler<NavigateToEventArgs>? NavigateToEvent;
        /// <summary>
        /// 刷新书架事件
        /// </summary>
        public event EventHandler? RefreshBookEvent;
        /// <summary>
        /// 导入漫画完成事件
        /// </summary>
        public event EventHandler? ImportComicCompletedEvent;
        /// <summary>
        /// 导入漫画事件
        /// </summary>
        public event EventHandler<ImportComicEventArgs>? ImportComicEvent;
        /// <summary>
        /// 导入漫画[错误]事件
        /// </summary>
        public event EventHandler<ImportComicErrorEventArgs>? ImportComicErrorEvent;
        /// <summary>
        /// 导入漫画[缩略图]事件
        /// </summary>
        public event EventHandler<ImportComicThumbEventArgs>? ImportComicThumbEvent;
        /// <summary>
        /// 导入漫画[进度]事件
        /// </summary>
        public event EventHandler<ImportComicProgressEventArgs>? ImportComicProgressEvent;
        /// <summary>
        /// 调试事件
        /// </summary>
        public event EventHandler? DebugEvent;
        /// <summary>
        /// 导航栏后退事件
        /// </summary>
        public event EventHandler? NavigationViewBackRequestedEvent;
        /// <summary>
        /// 导航栏面板事件
        /// </summary>
        public event EventHandler? NavigationViewPaneEvent;
        /// <summary>
        /// 插件启用事件
        /// </summary>
        public event EventHandler<PluginEventArg>? PluginEnabledEvent;
        /// <summary>
        /// 插件禁用事件
        /// </summary>
        public event EventHandler<PluginEventArg>? PluginDisabledEvent;

        public event EventHandler<TopGridEventArg>? TopGridEvent;
        public event EventHandler<ImportPluginEventArg>? ImportPluginEvent;
        /// <summary>
        /// 窗体最大化,最小化,普通事件
        /// </summary>
        public event TypedEventHandler<AppWindow, AppWindowChangedEventArgs>? OverlappedChangedEvent;

        /// <summary>
        /// 控制页面跳转
        /// </summary>
        void NavigateTo(Type page, object? parameter, bool force = true);
        /// <summary>
        /// 刷新书架
        /// </summary>
        void RefreshBook();
        /// <summary>
        /// 导入漫画
        /// </summary>
        void ImportComic(IReadOnlyList<IStorageItem> items, string[] passwords, int index);
        /// <summary>
        /// 导入漫画[错误]
        /// </summary>
        void ImportComicError(ImportComicError error, string message, IReadOnlyList<IStorageItem> items, int index, string[] password);
        /// <summary>
        /// 导入漫画[缩略图]
        /// </summary>
        void ImportComicThumb(MemoryStream stream);
        /// <summary>
        /// 导入漫画[进度]
        /// </summary>
        void ImportComicProgress(double progress);
        /// <summary>
        /// 导入漫画完成
        /// </summary>
        void ImportComicCompleted();
        /// <summary>
        /// 调试
        /// </summary>
        void Debug();
        void PluginEnabled(object sender, string id, bool enabled);
        void PluginDisabled(object sender, string id, bool enabled);
        void TopGrid(object sender, UIElement element, TopGridMode mode);
        void ImportPlugin(object sender, IReadOnlyList<IStorageItem> items);
        void NavigationViewPane(object sender);
        void NavigationViewBackRequested(object sender);
        void ChangeOverlapped(AppWindow sender, AppWindowChangedEventArgs args);
    }

}
