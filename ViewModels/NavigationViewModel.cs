using ShadowViewer.ToolKits;

namespace ShadowViewer.ViewModels
{
    public class NavigationViewModel: ObservableObject
    {
        public static ILogger Logger { get; } = Log.ForContext<NavigationViewModel>();
        private ICallableToolKit _callableToolKit;
        private IPluginsToolKit _pluginsToolKit;
        public NavigationViewModel(ICallableToolKit callableToolKit,IPluginsToolKit pluginsToolKit)
        {
            _callableToolKit = callableToolKit;
            _pluginsToolKit = pluginsToolKit;
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
            foreach (var plugin in _pluginsToolKit.EnabledPlugins)
            {
                plugin.NavigationViewItemsHandler(pluginItem);
                Log.ForContext<NavigationViewModel>().Information("[{name}]插件导航栏注入成功",
                    plugin.MetaData.Name);
            }
        }
    }
}
