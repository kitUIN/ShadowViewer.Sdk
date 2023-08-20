namespace ShadowViewer.ViewModels
{
    public partial class NavigationViewModel : ObservableObject
    {
        
        private ILogger Logger { get; }
        private ResponderService responderService;
        private ICallableService callableService;
        private IPluginService pluginService;

        public NavigationViewModel(ICallableService callableService, IPluginService pluginService, ILogger logger,ResponderService responderService)
        {
            Logger = logger;
            this.callableService = callableService;
            this.pluginService = pluginService;
            this.responderService = responderService;
        }
        /// <summary>
        /// 导航栏菜单
        /// </summary>
        public readonly ObservableCollection<IShadowNavigationItem> MenuItems = new();
        /// <summary>
        /// 导航栏底部菜单
        /// </summary>
        public readonly ObservableCollection<IShadowNavigationItem> FooterMenuItems = new();
        /// <summary>
        /// 添加导航栏个体
        /// </summary>
        private void AddMenuItem(IShadowNavigationItem item)
        {
            if (MenuItems.All(x => x.Id != item.Id))
            {
                MenuItems.Add(item);
            }
        }
        /// <summary>
        /// 添加导航栏个体
        /// </summary>
        private void DeleteMenuItem(IShadowNavigationItem item)
        {
            if (MenuItems.FirstOrDefault(x => x.Id == item.Id) is { } i)
            {
                MenuItems.Remove(i);
            }
        }
        /// <summary>
        /// 添加底部导航栏个体
        /// </summary>
        private void AddFooterMenuItems(IShadowNavigationItem item)
        {
            if (FooterMenuItems.All(x => x.Id != item.Id))
            {
                FooterMenuItems.Add(item);
            }
        }
        /// <summary>
        /// 删除底部导航栏个体
        /// </summary>
        private void DeleteFooterMenuItems(IShadowNavigationItem item)
        {
            if (FooterMenuItems.FirstOrDefault(x => x.Id == item.Id) is { } i)
            {
                FooterMenuItems.Remove(i);
            }
        }
        /// <summary>
        /// 初始化导航栏
        /// </summary>
        private void InitItems()
        {
            
            /*AddMenuItem(
                    new ShadowNavigationItem
                    {
                        Icon = new SymbolIcon(Symbol.Home),
                        Id = "BookShelf",
                        Content = CoreResourcesHelper.GetString(CoreResourceKey.BookShelf)
                    }
                );
            AddMenuItem(
                new ShadowNavigationItem
                {
                    Icon = new FontIcon() { Glyph = "\uE835" },
                    Id = "Plugins",
                    Content = CoreResourcesHelper.GetString(CoreResourceKey.Plugin)
                }
                );
            AddMenuItem(
                new ShadowNavigationItem
                {
                    Icon = new SymbolIcon(Symbol.Download),
                    Id = "Download",
                    Content = CoreResourcesHelper.GetString(CoreResourceKey.Download)
                }
            );*/
        }
        /// <summary>
        /// 重载导航栏
        /// </summary>
        public void ReloadItems()
        {
            foreach (var responder in responderService.NavigationViewResponders)
            {
                if (pluginService.GetPlugin(responder.Id) is not { } plugin) continue;
                foreach (var item2 in responder.NavigationViewMenuItems)
                {
                    if (plugin.IsEnabled)
                        AddMenuItem(item2);
                    else
                        DeleteMenuItem(item2);
                } 
                foreach (var item1 in responder.NavigationViewFooterItems)
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