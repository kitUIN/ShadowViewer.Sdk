namespace ShadowViewer.ViewModels
{
    public class NavigationViewModel: ObservableObject
    {
        public static ILogger Logger { get; } = Log.ForContext<NavigationViewModel>();
        private ICallableToolKit _callableToolKit;
        public NavigationViewModel(ICallableToolKit callableToolKit)
        {
            _callableToolKit = callableToolKit;
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
           /* foreach (string name in PluginHelper.EnabledPlugins)
            {
                PluginHelper.PluginInstances[name].NavigationViewItemsHandler(pluginItem);
                Log.ForContext<NavigationViewModel>().Information("[{name}]插件导航栏注入成功",
                    PluginHelper.PluginInstances[name].MetaData().Name);
            }*/
        }
    }
}
