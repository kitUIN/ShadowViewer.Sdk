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
        /// <param name="pluginItem">The plugin item.</param>
        public void LoadPluginItems(NavigationViewItem pluginItem)
        {
            //this.pluginItem = pluginItem;
            pluginItem.MenuItems.Clear();
            // TODO
            foreach (var plugin in pluginsToolKit.EnabledPlugins)
            {
                plugin.NavigationViewItemsHandler(pluginItem);
                Logger.Information("[{Name}]插件导航栏注入成功",
                    plugin.MetaData.Name);
            }
        }
    }
}
