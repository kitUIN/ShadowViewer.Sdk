using Microsoft.UI.Windowing;
using ShadowViewer.Services.Interfaces;
using System.Diagnostics;
using Windows.UI.WindowManagement;

namespace ShadowViewer.Helpers
{
    // Copy From WinUI 3 Gallery
    public static class WindowHelper
    {
        static public Window CreateWindow()
        {
            Window newWindow = new Window();
            TrackWindow(newWindow);
            return newWindow;
        }
        static public void TrackWindow(Window window)
        {
            window.AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            window.AppWindow.TitleBar.ButtonForegroundColor = Microsoft.UI.Colors.Transparent;
            window.Closed += (sender, args) => {
                _activeWindows.Remove(window);
            };
            if (window.AppWindow.Presenter is OverlappedPresenter overlappedPresenter)
            {
                _appWindowStates.Add(window.AppWindow.Id, overlappedPresenter.State);
            }
            window.AppWindow.Changed += (sender, args) =>
            {
                if (sender.Presenter is OverlappedPresenter overlappedPresenter)
                {
                    if(_appWindowStates.ContainsKey(sender.Id))
                    {
                        if(overlappedPresenter.State!= _appWindowStates[sender.Id])
                        {
                            DiFactory.Services.Resolve<ICallableService>().ChangeOverlapped(sender, args);
                        }
                        _appWindowStates[sender.Id] = overlappedPresenter.State;
                    }
                    else
                    {
                        _appWindowStates.Add(sender.Id, overlappedPresenter.State);
                    }
                }
            };
            _activeWindows.Add(window);
        }
        static public void ColseWindow(Window window)
        {
            window.Close();
        }
        static public void ColseWindowFromTitle(string title)
        {
            if (title != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (title == window.Title)
                    {
                        ColseWindow(window);
                        return;
                    }
                }
            }
        }
        static public Window? GetWindowForTitle(string title)
        {
            if (title != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (title == window.Title)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
        static public void SetWindowTitle(string oldTitle,string title)
        {
            foreach (Window window in _activeWindows)
            {
                if (oldTitle == window.Title)
                {
                    window.Title = title;
                }
            }
        }
        static public Window? GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
        static public Window? GetWindowForXamlRoot(XamlRoot xamlRoot)
        {
            if (xamlRoot != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (xamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
        private static Dictionary<WindowId, OverlappedPresenterState> _appWindowStates = new ();
        public static List<Window> ActiveWindows => _activeWindows;
        private static readonly List<Window> _activeWindows = new();
    }
}
