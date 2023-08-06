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
            MenuItems = new ObservableCollection<NavigationViewItem>
            {
                new NavigationViewItem
            {
                Icon = new SymbolIcon(Symbol.Home),
                Tag = "BookShelf",
                Content = CoreResourcesHelper.GetString(CoreResourceKey.BookShelf)
            },new NavigationViewItem
            {
                Icon = new FontIcon() { Glyph = "\uE835" },
                Tag = "Plugins",
                Content = CoreResourcesHelper.GetString(CoreResourceKey.Plugin)
            },new NavigationViewItem
            {
                Icon = new SymbolIcon(Symbol.Download),
                Tag = "Download",
                Content = CoreResourcesHelper.GetString(CoreResourceKey.Download)
            }
            };
        }

        public ObservableCollection<NavigationViewItem> MenuItems { get; } =
            new ObservableCollection<NavigationViewItem>();

        public void InitMenuItems()
        {
            for (int i = MenuItems.Count - 1; i > 3; i--)
            {
                MenuItems.RemoveAt(i);
            }
            foreach (var plugin in pluginsToolKit.EnabledPlugins)
            {
                plugin.NavigationViewMenuItemsHandler(MenuItems);
            }
        }


    }
}