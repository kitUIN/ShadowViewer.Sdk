using DryIoc;
using Microsoft.UI.Xaml;
using ShadowPluginLoader.WinUI;
using ShadowViewer.Sdk.Configs;

namespace ShadowViewer.Sdk.Helpers
{
    // Copy From WinUI 3 Gallery
    /// <summary>
    /// Class providing functionality around switching and restoring theme settings
    /// </summary>
    public static class ThemeHelper
    {

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

                var config = DiFactory.Services.Resolve<CoreConfig>();
                config.Theme = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public static void Initialize(Window window)
        {
            RootTheme = DiFactory.Services.Resolve<CoreConfig>().Theme;
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
