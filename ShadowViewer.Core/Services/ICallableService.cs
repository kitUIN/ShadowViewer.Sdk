using System;
using Microsoft.UI.Windowing;
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
        /// 
        /// </summary>
        void ChangeOverlapped(AppWindow sender, AppWindowChangedEventArgs args);
    }

}
