using Microsoft.UI.Xaml.Media.Animation;

namespace ShadowViewer.Helpers
{
    public static class MessageHelper
    {

        /// <summary>
        /// 通知NavigationPage刷新导航栏插件注入
        /// </summary>
        public static void SendNavigationReloadPlugin() {
            WeakReferenceMessenger.Default.Send(new NavigationMessage("PluginReload"));
        }
        /// <summary>
        /// 通知BookShelfPage刷新元素
        /// </summary>
        public static void SendFilesReload()
        {
            WeakReferenceMessenger.Default.Send(new FilesMessage("Reload"));
        }
    }
}
