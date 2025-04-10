using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Core.Args;
using System;
using Windows.Foundation;

namespace ShadowViewer.Core.Services
{
    /// <summary>
    /// 通知其他控件的工具类
    /// </summary>
    public interface ICallableService
    {
        /// <summary>
        /// 调试事件
        /// </summary>
        public event EventHandler? DebugEvent;
        /// <summary>
        /// 顶层控件 事件
        /// </summary>
        public event EventHandler<TopLevelControlEventArgs>? TopLevelControlEvent;
        /// <summary>
        /// 应用初始化成功事件
        /// </summary>
        public event EventHandler? AppLoadedEvent;
        /// <summary>
        /// 主题变更事件
        /// </summary>
        public event EventHandler? ThemeChangedEvent;

        /// <summary>
        /// 窗体最大化,最小化,普通事件
        /// </summary>
        public event TypedEventHandler<AppWindow, AppWindowChangedEventArgs>? OverlappedChangedEvent;

        /// <summary>
        /// 调试
        /// </summary>
        void Debug();
        /// <summary>
        /// 主题变更
        /// </summary>
        void ThemeChanged();
        /// <summary>
        /// 应用初始化成功
        /// </summary>
        void AppLoaded();

        /// <summary>
        /// 窗体最大化,最小化,普通事件
        /// </summary>
        void ChangeOverlapped(AppWindow sender, AppWindowChangedEventArgs args);

        /// <summary>
        /// 创建顶层控件
        /// </summary>
        void CreateTopLevelControl(UIElement control);

        /// <summary>
        /// 删除顶层控件
        /// </summary>
        void RemoveTopLevelControl(UIElement control);
    }

}
