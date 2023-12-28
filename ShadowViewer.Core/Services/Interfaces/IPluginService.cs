using Serilog.Core;
using ShadowViewer.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Services.Interfaces
{
    public interface IPluginService
    {
        public static int MinVersion => 20230821;

        /// <summary>
        /// 导入一个插件,从类型T中
        /// </summary>
        void ImportOnePlugin<T>()where T:IPlugin;

        /// <summary>
        /// 导入一个插件,从文件夹中
        /// </summary>
        Task ImportFromPathAsync(string directoryPath);
        /// <summary>
        /// 从插件文件夹导入插件
        /// </summary>
        Task ImportFromPluginsPathAsync();
        /// <summary>
        /// 获取已经启动的插件
        /// </summary>
        IList<IPlugin> GetEnabledPlugins();
        /// <summary>
        /// 获取所有插件
        /// </summary>
        IList<IPlugin> GetPlugins();
        /// <summary>
        /// 获取插件
        /// </summary>
        IPlugin? GetPlugin(string id);
        /// <summary>
        /// 获取已经启动的插件
        /// </summary>
        IPlugin? GetEnabledPlugin(string id);

        /// <summary>
        /// 插件是否启用
        /// </summary>
        bool? IsEnabled(string id);

        /// <summary>
        /// 启用插件
        /// </summary>
        void PluginEnabled(string id);

        /// <summary>
        /// 禁用插件
        /// </summary>
        void PluginDisabled(string id);
        /// <summary>
        /// 删除插件
        /// </summary>
        void Delete(string id);
    }
}
