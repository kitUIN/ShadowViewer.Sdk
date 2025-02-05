using System.Collections.Generic;
using Microsoft.UI.Windowing;
using Windows.Foundation;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using AppWindow = Microsoft.UI.Windowing.AppWindow;
using AppWindowChangedEventArgs = Microsoft.UI.Windowing.AppWindowChangedEventArgs;
using Serilog;
using System.Linq;

// Base On WinUI 3 Gallery
namespace ShadowViewer.Helpers
{
    /// <summary>
    /// 窗体帮助类
    /// </summary>
    public static class WindowHelper
    {
        #region Event
        /// <summary>
        /// 窗体最大化,最小化,普通事件
        /// </summary>
        public static event TypedEventHandler<AppWindow, AppWindowChangedEventArgs>? OverlappedChangedEvent;



        #endregion
        
        #region InvokeEvent
        /// <summary>
        /// 触发窗体变更事件
        /// </summary>
        public static void ChangeOverlapped(AppWindow sender, AppWindowChangedEventArgs args)
        {
            OverlappedChangedEvent?.Invoke(sender, args);
            Log.Debug("触发事件{EventName},{Kind}", 
                nameof(OverlappedChangedEvent),
                sender.Presenter.Kind);
        }
        #endregion

        /// <summary>
        /// 新建窗体
        /// </summary>
        public static T CreateWindow<T>() where T : Window, new()
        {
            T newWindow = new();
            TrackWindow(newWindow);
            return newWindow;
        }
        /// <summary>
        /// 跟踪窗体
        /// </summary>
        /// <param name="window"></param>
        public static void TrackWindow(Window window)
        {
            window.Closed += (_, _) => {
                ActiveWindows.Remove(window);
            };
            if (window.AppWindow.Presenter is OverlappedPresenter overlappedPresenter)
            {
                AppWindowStates.Add(window.AppWindow.Id, overlappedPresenter.State);
            }
            window.AppWindow.Changed += (sender, args) =>
            {
                if (sender.Presenter is not OverlappedPresenter presenter) return;
                if(AppWindowStates.ContainsKey(sender.Id))
                {
                    if(presenter.State != AppWindowStates[sender.Id])
                    {
                        ChangeOverlapped(sender, args);
                    }
                    AppWindowStates[sender.Id] = presenter.State;
                }
                else
                {
                    AppWindowStates.Add(sender.Id, presenter.State);
                }
            };
            ActiveWindows.Add(window);
        }


        #region 关闭窗体

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public static void CloseWindow(Window? window)
        {
            window?.Close();
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="title"></param>
        public static void CloseWindow(string? title)
        {
            if (title == null) return;
            foreach (var window in ActiveWindows.Where(window => title == window.Title))
            {
                CloseWindow(window);
                return;
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public static void CloseWindow(WindowId? windowId)
        {
            CloseWindow(GetWindow(windowId));
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public static void CloseWindow(UIElement? element)
        {
            CloseWindow(GetWindow(element));
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public static void CloseWindow(XamlRoot? xamlRoot)
        {
            CloseWindow(GetWindow(xamlRoot));
        }
        #endregion

        #region 获取窗体
        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Window? GetWindow(string? title)
        {
            return title == null ? null : ActiveWindows.FirstOrDefault(window => title == window.Title);
        }

        /// <summary>
        /// 获取窗体
        /// </summary>
        public static Window? GetWindow(WindowId? windowId)
        {
            return windowId == null ? null : ActiveWindows.FirstOrDefault(window => windowId == window.AppWindow.Id);
        }
        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Window? GetWindow(UIElement? element)
        {
            return GetWindow(element?.XamlRoot);
        }
        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="xamlRoot"></param>
        /// <returns></returns>
        public static Window? GetWindow(XamlRoot? xamlRoot)
        {
            return xamlRoot == null ? null : ActiveWindows.FirstOrDefault(window => xamlRoot == window.Content.XamlRoot);
        }
        #endregion



        /// <summary>
        /// 设置窗体标题
        /// </summary>
        public static void SetWindowTitle(string oldTitle, string newTitle)
        {
            foreach (var window in ActiveWindows.Where(window => oldTitle == window.Title))
            {
                window.Title = newTitle;
            }
        }

        /// <summary>
        /// 窗体状态
        /// </summary>
        private static readonly Dictionary<WindowId, OverlappedPresenterState> AppWindowStates = new ();
        
        /// <summary>
        /// 正在活动的窗体
        /// </summary>
        public static List<Window> ActiveWindows { get; } = [];
    }
}
