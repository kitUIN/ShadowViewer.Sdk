using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.UI.Windowing;
using Windows.Foundation;
using Windows.Storage;
using Microsoft.UI.Xaml;
using ShadowViewer.Core.Enums;
using ShadowViewer.Core.Args;

namespace ShadowViewer.Core.Services
{
    /// <summary>
    /// 通知其他控件的工具类
    /// </summary>
    public interface ICallableService
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
        /// 主题变更事件
        /// </summary>
        public event EventHandler? ThemeChangedEvent;

        public event EventHandler<TopGridEventArg>? TopGridEvent;

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

        void ThemeChanged();
        void TopGrid(object sender, UIElement element, TopGridMode mode);

        void ChangeOverlapped(AppWindow sender, AppWindowChangedEventArgs args);
    }

}
