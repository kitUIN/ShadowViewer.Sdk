using ShadowViewer.ToolKits;

namespace ShadowViewer.ViewModels
{
    public class NavigationViewModel: ObservableObject
    {
        private static ILogger Logger { get; } = Log.ForContext<NavigationViewModel>();
        private ICallableToolKit callableToolKit;
        private readonly IPluginsToolKit pluginsToolKit;
        public NavigationViewModel(ICallableToolKit callableToolKit, IPluginsToolKit pluginsToolKit)
        {
            this.callableToolKit = callableToolKit;
            this.pluginsToolKit = pluginsToolKit;
        }
        /// <summary>
        /// 导航栏插件栏注入
        /// </summary>
        public void LoadPluginItems(NavigationViewItem pluginItem)
        {
            pluginItem.MenuItems.Clear();
            foreach (var plugin in pluginsToolKit.EnabledPlugins)
            {
                plugin.NavigationViewItemsHandler(ref pluginItem);
            }
        }
    }
}
