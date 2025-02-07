using DryIoc;
using Microsoft.UI.Xaml;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Core.Services;

namespace ShadowViewer.Core.Helpers
{
    // Copy From WinUI 3 Gallery
    /// <summary>
    /// Class providing functionality around switching and restoring theme settings
    /// </summary>
    public static class ThemeHelper
    {
        private const string SelectedAppThemeKey = "ShadowViewer_SelectedAppTheme";

        /// <summary>
        /// Gets or sets (with LocalSettings persistence) the RequestedTheme of the root element.
        /// </summary>
        public static ElementTheme RootTheme
        {
            get
            {
                foreach (var window in WindowHelper.ActiveWindows)
                {
                    if (window.Content is FrameworkElement rootElement)
                    {
                        return rootElement.RequestedTheme;
                    }
                }
                return ElementTheme.Default;
            }
            set
            {
                foreach (var window in WindowHelper.ActiveWindows)
                {
                    if (window.Content is FrameworkElement rootElement)
                    {
                        rootElement.RequestedTheme = value;
                    }
                }
                if (ConfigHelper.GetString(SelectedAppThemeKey) != value.ToString())
                {
                    DiFactory.Services.Resolve<ICallableService>().ThemeChanged();
                }
                ConfigHelper.Set(SelectedAppThemeKey, value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public static void Initialize(Window window)
        {
            var savedTheme = ConfigHelper.GetString(SelectedAppThemeKey);
            if (savedTheme != null)
            {
                RootTheme = EnumHelper.GetEnum<ElementTheme>(savedTheme);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsDarkTheme()
        {
            if (RootTheme == ElementTheme.Default)
            {
                return Application.Current.RequestedTheme == ApplicationTheme.Dark;
            }
            return RootTheme == ElementTheme.Dark;
        }
    }
}
