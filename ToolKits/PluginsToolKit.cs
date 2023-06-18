using System.Linq;

namespace ShadowViewer.ToolKits
{
    public class PluginsToolKit: IPluginsToolKit
    {
        private ILogger Logger { get; } = Log.ForContext<PluginsToolKit>();
        /// <summary>
        /// 启动的插件
        /// </summary>
        private ObservableCollection<string> EnabledPlugins { get; }
        /// <summary>
        /// 插件列表
        /// </summary>
        private ObservableCollection<string> AllPlugins { get; } = new ObservableCollection<string>();
        private IEnumerable<IPlugin> plugins;
        private void EnabledPlugins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ConfigHelper.Set("EnabledPlugins", string.Join(",", EnabledPlugins));
        }
        private ObservableCollection<string> EnabledPluginsInit()
        {
            if (ConfigHelper.GetString("EnabledPlugins") is string enablePlugins)
            {
                return new ObservableCollection<string>(new HashSet<string>(enablePlugins.Split(',').ToList()));
            }
            return new ObservableCollection<string>();
        }
        public PluginsToolKit(IEnumerable<IPlugin> plugins)
        {
            this.plugins = plugins;
            EnabledPlugins = EnabledPluginsInit();
            EnabledPlugins.CollectionChanged += this.EnabledPlugins_CollectionChanged;
            foreach (IPlugin plugin in plugins)
            {
                string id = plugin.MetaData.ID;
                AllPlugins.Add(id);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginEnabled(string id)
        {
            if (!IsEnabled(id) && plugins.Any(x => x.MetaData.ID == id))
            {
                EnabledPlugins.Add(id);
                Logger.Information("插件{id}启用成功", id);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void PluginDisabled(string id)
        {
            if (IsEnabled(id) && plugins.Any(x => x.MetaData.ID == id))
            {
                EnabledPlugins.Remove(id);
                Logger.Information("插件{id}禁用成功", id);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPlugin GetPlugin(string id)
        {
            if (IsEnabled(id))
            {
                return plugins.FirstOrDefault(x => x.MetaData.ID == id);
            }
            Logger.Information("插件{id}已被禁用,无法获取", id);
            return default;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<IPlugin> GetEnabledPlugins()
        {
            return plugins.Where(x => IsEnabled(x.MetaData.ID));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsEnabled(string id)
        {
            return EnabledPlugins.Contains(id);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public LocalTag GetAffiliationTag(string id)
        {
            if(id == "Local")
            {
                return new LocalTag(AppResourcesToolKit.GetString("Shadow.Tag.Local"), "#000000", "#ffd657");
            }
            return plugins.FirstOrDefault(x => x.MetaData.ID == id).AffiliationTag;
        }
    }
}
