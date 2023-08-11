namespace ShadowViewer.ViewModels
{
    public partial class NavigationViewModel : ObservableObject
    {
        private static ILogger Logger { get; } = Log.ForContext<NavigationViewModel>();
        private ICallableService callableService;
        private readonly IPluginService pluginService;

        public NavigationViewModel(ICallableService callableService, IPluginService pluginService)
        {
            this.callableService = callableService;
            this.pluginService = pluginService;
            InitItems();
        }
        /// <summary>
        /// 导航栏菜单
        /// </summary>
        public ObservableCollection<ShadowNavigationItem> MenuItems= new ObservableCollection<ShadowNavigationItem>();
        /// <summary>
        /// 导航栏底部菜单
        /// </summary>
        public ObservableCollection<ShadowNavigationItem> FooterMenuItems= new ObservableCollection<ShadowNavigationItem>();
        /// <summary>
        /// 添加导航栏个体
        /// </summary>
        private void AddMenuItem(ShadowNavigationItem item)
        {
            if (MenuItems.All(x => x.Id != item.Id))
            {
                MenuItems.Add(item);
            }
        }
        /// <summary>
        /// 添加导航栏个体
        /// </summary>
        private void DeleteMenuItem(ShadowNavigationItem item)
        {
            if (MenuItems.FirstOrDefault(x => x.Id == item.Id) is ShadowNavigationItem i)
            {
                MenuItems.Remove(i);
            }
        }
        /// <summary>
        /// 添加底部导航栏个体
        /// </summary>
        private void AddFooterMenuItems(ShadowNavigationItem item)
        {
            if (FooterMenuItems.All(x => x.Id != item.Id))
            {
                FooterMenuItems.Add(item);
            }
        }
        /// <summary>
        /// 删除底部导航栏个体
        /// </summary>
        private void DeleteFooterMenuItems(ShadowNavigationItem item)
        {
            if (FooterMenuItems.FirstOrDefault(x => x.Id == item.Id) is ShadowNavigationItem i)
            {
                FooterMenuItems.Remove(i);
            }
        }
        /// <summary>
        /// 初始化导航栏
        /// </summary>
        private void InitItems()
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
        /// <summary>
        /// 重载导航栏
        /// </summary>
        public void ReloadItems()
        {
            foreach (var plugin in pluginService.Plugins)
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