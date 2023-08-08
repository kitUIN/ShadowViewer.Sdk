namespace ShadowViewer.ViewModels
{
    public partial class NavigationViewModel : ObservableObject
    {
        private static ILogger Logger { get; } = Log.ForContext<NavigationViewModel>();
        private ICallableToolKit callableToolKit;
        private readonly IPluginsToolKit pluginsToolKit;

        public NavigationViewModel(ICallableToolKit callableToolKit, IPluginsToolKit pluginsToolKit)
        {
            this.callableToolKit = callableToolKit;
            this.pluginsToolKit = pluginsToolKit;
            InitMainItems();
        }
        
        public ObservableCollection<ShadowNavigationItem> MenuItems= new ObservableCollection<ShadowNavigationItem>();
        public ObservableCollection<ShadowNavigationItem> FooterMenuItems= new ObservableCollection<ShadowNavigationItem>();

        void AddMenuItem(ShadowNavigationItem item)
        {
            if (MenuItems.All(x => x.Id != item.Id))
            {
                MenuItems.Add(item);
            }
        }void DeleteMenuItem(ShadowNavigationItem item)
        {
            if (MenuItems.FirstOrDefault(x => x.Id == item.Id) is ShadowNavigationItem i)
            {
                MenuItems.Remove(i);
            }
        }
        void AddFooterMenuItems(ShadowNavigationItem item)
        {
            if (FooterMenuItems.All(x => x.Id != item.Id))
            {
                FooterMenuItems.Add(item);
            }
        }
        void DeleteFooterMenuItems(ShadowNavigationItem item)
        {
            if (FooterMenuItems.FirstOrDefault(x => x.Id == item.Id) is ShadowNavigationItem i)
            {
                FooterMenuItems.Remove(i);
            }
        }
        private void InitMainItems()
        {
            AddMenuItem(
                    new ShadowNavigationItem
                    {
                        IsDefault = true,
                        Icon = new SymbolIcon(Symbol.Home),
                        Id = "BookShelf",
                        Content = CoreResourcesHelper.GetString(CoreResourceKey.BookShelf)
                    }
                );
            AddMenuItem(
                new ShadowNavigationItem
                {
                    IsDefault = true,
                    Icon = new FontIcon() { Glyph = "\uE835" },
                    Id = "Plugins",
                    Content = CoreResourcesHelper.GetString(CoreResourceKey.Plugin)
                }
                );
            AddMenuItem(
                new ShadowNavigationItem
                {
                    IsDefault = true,
                    Icon = new SymbolIcon(Symbol.Download),
                    Id = "Download",
                    Content = CoreResourcesHelper.GetString(CoreResourceKey.Download)
                }
            );
            AddFooterMenuItems(
                new ShadowNavigationItem()
                {
                    IsDefault = true,
                    Icon =  new FontIcon(){Glyph = "\uE77B"},
                    Id = "User",
                    Content = CoreResourcesHelper.GetString(CoreResourceKey.User)
                }
                );
        }
        public void InitMenuItems()
        {
            foreach (var plugin in pluginsToolKit.Plugins)
            {
                foreach (var item2 in plugin.NavigationViewMenuItems)
                {
                    if (plugin.IsEnabled)
                        AddMenuItem(item2);
                    else
                        DeleteMenuItem(item2);
                } 
                foreach (var item1 in plugin.NavigationViewFooterItems)
                {
                    if (plugin.IsEnabled)
                        AddFooterMenuItems(item1);
                    else
                        DeleteFooterMenuItems(item1);
                } 
            }
        }
    }
}