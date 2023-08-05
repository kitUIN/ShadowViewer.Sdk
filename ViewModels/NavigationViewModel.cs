namespace ShadowViewer.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        private static ILogger Logger { get; } = Log.ForContext<NavigationViewModel>();
        private ICallableToolKit callableToolKit;
        private readonly IPluginsToolKit pluginsToolKit;

        public NavigationViewModel(ICallableToolKit callableToolKit, IPluginsToolKit pluginsToolKit)
        {
            this.callableToolKit = callableToolKit;
            this.pluginsToolKit = pluginsToolKit;
            MenuItems.Add(new NavigationViewItem
            {
                Icon = new SymbolIcon(Symbol.Home),
                Tag = "BookShelf",
                Content = CoreResourcesHelper.GetString(CoreResourceKey.BookShelf)
            });
            MenuItems.Add(new NavigationViewItem
            {
                Icon = new FontIcon() { Glyph = "\uE835" },
                Tag = "Plugins",
                Content = CoreResourcesHelper.GetString(CoreResourceKey.Plugin)
            });
            MenuItems.Add(new NavigationViewItem
            {
                Icon = new SymbolIcon(Symbol.Download),
                Tag = "Download",
                Content = CoreResourcesHelper.GetString(CoreResourceKey.Download)
            });
        }

        public ObservableCollection<NavigationViewItem> MenuItems { get; } =
            new ObservableCollection<NavigationViewItem>();

        public void InitMenuItems()
        {
            foreach (var plugin in pluginsToolKit.EnabledPlugins)
            {
                var item = plugin.PluginNavigationViewItem();
                if (!MenuItems.Contains(item))
                {
                    MenuItems.Add(item);
                }
            }
            foreach (var plugin in pluginsToolKit.EnabledPlugins)
            {
                plugin.NavigationViewMenuItemsHandler(MenuItems);
            }
        }
    }
}