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
        /// 调试
        /// </summary>
        void Debug();
        /// <summary>
        /// 主题变更
        /// </summary>
        void ThemeChanged();
        /// <summary>
        /// 顶部窗体
        /// </summary>
        void TopGrid(object sender, UIElement element, TopGridMode mode);
        /// <summary>
        /// 
        /// </summary>
        void ChangeOverlapped(AppWindow sender, AppWindowChangedEventArgs args);
    }

}
